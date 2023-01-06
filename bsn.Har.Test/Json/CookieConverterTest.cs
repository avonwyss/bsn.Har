using System;
using System.Net;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;

namespace bsn.Har.Json {
	public class CookieConverterTest {
		private readonly ITestOutputHelper output;

		public CookieConverterTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Fact]
		public void SerializeCookie() {
			output.WriteLine(JsonConvert.SerializeObject(
					new Cookie() {
							Name = "Crunchy",
							Value = "Crispy",
							Path = "/",
							Domain = "bsn.ch",
							Expires = DateTime.Now.AddDays(1)
					}, Formatting.Indented, new CookieConverter()));
		}

		[Fact]
		public void DeserializeCookie() {
			output.WriteLine(JsonConvert.DeserializeObject<Cookie[]>(
					@"[{
  ""name"": ""Crunchy"",
  ""value"": ""Crispy"",
  ""path"": ""/"",
  ""domain"": ""bsn.ch"",
  ""expires"": ""2022-12-15T18:03:07.8191651+01:00"",
  ""httpOnly"": false,
  ""secure"": false
}]", new CookieConverter())[0].ToString());
		}
	}
}
