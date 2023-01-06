using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace bsn.Har.Handler {
	public static class Binder {
		// ReSharper disable InconsistentNaming
		private static readonly MethodInfo meth_RequestData_TryGetFromPath = typeof(RequestData).GetMethod(nameof(RequestData.TryGetFromPath), BindingFlags.Instance|BindingFlags.Public);
		private static readonly MethodInfo meth_RequestData_TryGetFromHeader = typeof(RequestData).GetMethod(nameof(RequestData.TryGetFromHeader), BindingFlags.Instance|BindingFlags.Public);
		private static readonly MethodInfo meth_RequestData_TryGetFromQuery = typeof(RequestData).GetMethod(nameof(RequestData.TryGetFromQuery), BindingFlags.Instance|BindingFlags.Public);
		private static readonly MethodInfo meth_RequestData_TryGetFromContent = typeof(RequestData).GetMethod(nameof(RequestData.TryGetFromContent), BindingFlags.Instance|BindingFlags.Public);
		private static readonly MethodInfo meth_ICollectionOfString_Add = typeof(ICollection<string>).GetMethod(nameof(ICollection<string>.Add), BindingFlags.Instance|BindingFlags.Public);
		// ReSharper restore InconsistentNaming
		private static readonly ConcurrentDictionary<Type, Action<object, RequestData, ICollection<string>>> typeBinder = new ConcurrentDictionary<Type, Action<object, RequestData, ICollection<string>>>();

		public static void Bind(object entity, RequestData requestData) {
			var missing = new List<string>();
			GetBinder(entity.GetType())(entity, requestData, missing);
			if (missing.Count > 0) {
				throw new RequestBindingException($"Missing data from request: {string.Join(", ", missing)}");
			}
		}

		public static bool TryBind(object entity, RequestData requestData) {
			var missing = new List<string>();
			GetBinder(entity.GetType())(entity, requestData, missing);
			return missing.Count == 0;
		}

		private static Action<object, RequestData, ICollection<string>> GetBinder(Type entityType) {
			return typeBinder.GetOrAdd(entityType, t => {
				var paraEntity = Expression.Parameter(typeof(object), "entity");
				var varEntity = Expression.Parameter(t, "entity");
				var paraRequestData = Expression.Parameter(typeof(RequestData), "requestData");
				var paraMissing = Expression.Parameter(typeof(ICollection<string>), "missing");
				var vars = new Dictionary<Type, ParameterExpression>();
				var body = new List<Expression>() {
						Expression.Assign(varEntity, Expression.Convert(paraEntity, t))
				};
				foreach (var memberBinding in t
						         .GetMembers(BindingFlags.Instance|BindingFlags.Public)
						         .SelectMany(m => m.GetCustomAttributes(typeof(RequestBindingAttribute), true)
								         .Cast<RequestBindingAttribute>()
								         .Where(a => a.Source != 0)
								         .OrderBy(a => a.Priority)
								         .ThenBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
								         .Select(a => new KeyValuePair<MemberInfo, RequestBindingAttribute>(m, a)))) {
					var exprMember = memberBinding.Key switch {
							PropertyInfo property => Expression.Property(varEntity, property),
							FieldInfo field => Expression.Field(varEntity, field),
							_ => throw new InvalidOperationException("Unexpected member type "+memberBinding.Key.MemberType)
					};
					if (!vars.TryGetValue(exprMember.Type, out var varMember)) {
						varMember = Expression.Variable(exprMember.Type, "member");
						vars.Add(exprMember.Type, varMember);
					}
					var name = memberBinding.Value.Name ?? memberBinding.Key.Name;
					var expr = default(Expression);

					void CallTryGetFrom(MethodInfo meth) {
						var exprCall = Expression.Call(paraRequestData, meth.MakeGenericMethod(varMember.Type),
								Expression.Constant(name),
								varMember);
						expr = expr == null ? (Expression)exprCall : Expression.Or(exprCall, expr);
					}

					if ((memberBinding.Value.Source&BindingSource.Content) != 0) {
						CallTryGetFrom(meth_RequestData_TryGetFromContent);
					}
					if ((memberBinding.Value.Source&BindingSource.Query) != 0) {
						CallTryGetFrom(meth_RequestData_TryGetFromQuery);
					}
					if ((memberBinding.Value.Source&BindingSource.Header) != 0) {
						CallTryGetFrom(meth_RequestData_TryGetFromHeader);
					}
					if ((memberBinding.Value.Source&BindingSource.Path) != 0) {
						CallTryGetFrom(meth_RequestData_TryGetFromPath);
					}
					body.Add(memberBinding.Value.Required 
							? Expression.IfThenElse(expr, Expression.Assign(exprMember, varMember), Expression.Call(paraMissing, meth_ICollectionOfString_Add, Expression.Constant(name))) 
							: Expression.IfThen(expr, Expression.Assign(exprMember, varMember)));
				}
				return Expression.Lambda<Action<object, RequestData, ICollection<string>>>(
						Expression.Block(vars.Values.Append(varEntity), body), 
						paraEntity, paraRequestData, paraMissing).Compile();
			});
		}
	}
}
