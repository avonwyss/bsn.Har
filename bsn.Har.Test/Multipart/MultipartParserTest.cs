using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace bsn.Har.Multipart {
	public class MultipartParserTest {
		private readonly ITestOutputHelper output;

		public MultipartParserTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Theory]
		[InlineData("This is the preamble.  It is to be ignored, though it\r\n"+
		            "is a handy place for composition agents to include an\r\n"+
		            "explanatory note to non-MIME conformant readers.\r\n"+
		            "\r\n"+
		            "--simple boundary\r\n"+
		            "\r\n"+
		            "This is implicitly typed plain US-ASCII text.\r\n"+
		            "It does NOT end with a linebreak.\r\n"+
		            "--simple boundary\r\n"+
		            "Content-type: text/plain; charset=us-ascii\r\n"+
		            "\r\n"+
		            "This is explicitly typed plain US-ASCII text.\r\n"+
		            "It DOES end with a linebreak.\r\n"+
		            "\r\n"+
		            "--simple boundary--\r\n"+
		            "\r\n"+
		            "This is the epilogue.  It is also to be ignored.", "simple boundary", new string[] {null, null})]
		[InlineData("-----------------------------9051914041544843365972754266\r\n"+
		            "Content-Disposition: form-data; name=\"text\"\r\n"+
		            "\r\n"+
		            "text default\r\n"+
		            "-----------------------------9051914041544843365972754266\r\n"+
		            "Content-Disposition: form-data; name=\"file1\"; filename=\"a.txt\"\r\n"+
		            "Content-Type: text/plain\r\n\r\nContent of a.txt.\r\n"+
		            "\r\n"+
		            "-----------------------------9051914041544843365972754266\r\n"+
		            "Content-Disposition: form-data; name=\"file2\"; filename=\"a.html\"\r\n"+
		            "Content-Type: text/html"+
		            "\r\n"+
		            "\r\n"+
		            "<!DOCTYPE html><title>Content of a.html.</title>\r\n"+
		            "\r\n"+
		            "-----------------------------9051914041544843365972754266--", "---------------------------9051914041544843365972754266", new [] {"text", "file1", "file2"})]
		public void ParseTest(string text, string boundary, string[] partNames) {
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text), false);
			List<string> names = new List<string>(partNames.Length);
			foreach (var param in MultipartParser.Parse(stream, boundary)) {
				names.Add(param.Name);
				output.WriteLine("--Part "+names.Count+"--");
				output.WriteLine(param.Name ?? "(NULL)");
				output.WriteLine((param.ContentType ?? "text/plain")+" "+param.Encoding);
				output.WriteLine(param.Value ?? "(NULL)");
			}
			output.WriteLine("--End--");
			Assert.Equal(partNames, names);
		}
	}
}
