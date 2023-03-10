using System;
using System.IO;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;

namespace bsn.Har {
	public class HarDocumentTest {
		private readonly ITestOutputHelper output;

		public HarDocumentTest(ITestOutputHelper output) {
			this.output = output;
		}

		[Fact]
		public void ParseDocument() {
			output.WriteLine(HarDocument.Serializer.Deserialize<HarDocument>(new JsonTextReader(new StringReader(
					@"{
	""log"": {
		""version"": ""1.1"", 
		""creator"": {
			""name"": ""Firebug"", 
			""version"": ""1.5X.0b8""
		}, 
		""browser"": {
			""name"": ""Firefox"", 
			""version"": ""3.6b6pre""
		}, 
		""pages"": [
			{
				""startedDateTime"": ""2010-01-02T15:38:46.686+01:00"", 
				""id"": ""page_21396"", 
				""title"": ""Cuzillion"", 
				""pageTimings"": {
					""onContentLoad"": 5605, 
					""onLoad"": 6964
				}, 
				""comment"": ""See the gap between the third and fourth request (click this bar to expand the page log and see all requests). This is caused by execution of an inline script.""
			}, 
			{
				""startedDateTime"": ""2010-01-02T16:12:32.738+01:00"", 
				""id"": ""page_20633"", 
				""title"": ""Cuzillion"", 
				""pageTimings"": {
					""onContentLoad"": 5564, 
					""onLoad"": 5572
				},
				""comment"": ""The script is moved to the bottom of the page in this case.""
			}
		], 
		""entries"": [
			{
				""pageref"": ""page_21396"", 
				""startedDateTime"": ""2010-01-02T15:38:46.686+01:00"", 
				""time"": 525, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://stevesouders.com/cuzillion/?ex=10100&title=Inline+Scripts+Block"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""stevesouders.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}
					], 
					""queryString"": [
						{
							""name"": ""ex"", 
							""value"": ""10100""
						}, 
						{
							""name"": ""title"", 
							""value"": ""Inline Scripts Block""
						}
					], 
					""headersSize"": 444, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 14:38:52 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""X-Powered-By"", 
							""value"": ""PHP/5.2.3""
						}, 
						{
							""name"": ""Vary"", 
							""value"": ""Accept-Encoding""
						}, 
						{
							""name"": ""Content-Encoding"", 
							""value"": ""gzip""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""2725""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""text/html""
						}
					], 
					""content"": {
						""size"": 8836, 
						""mimeType"": ""text/html"", 
						""text"": ""\n<script>\nvar gTop = Number(new Date());\nvar gScriptMsg = \""\"";\nfunction cuz_addHandler(elem, sType, fn, capture) {\n    capture = (capture) ? true : false;\n    if (elem.addEventListener) {\n        elem.addEventListener(sType, fn, capture);\n    }\n    else if (elem.attachEvent) {\n        elem.attachEvent(\""on\"" + sType, fn);\n    }\n    else {\n        // Netscape 4\n        if ( elem[\""on\"" + sType] ) {\n            // Do nothing - we don't want to overwrite an existing handler.\n        }\n        else {\n            elem[\""on\"" + sType] = fn;\n        }\n    }\n}\nfunction doOnload() {\n\tvar end = Number(new Date());\n    document.getElementById('loadtime').innerHTML = 'page load time: ' + (end - gTop) + ' ms';\n\tif ( gScriptMsg && document.getElementById('loadedscripts') ) {\n\t\tdocument.getElementById('loadedscripts').innerHTML += gScriptMsg;\n\t}\n}\ncuz_addHandler(window, 'load', doOnload);\nvar gbEnabled = false;\nfunction enableEdit() {\n\tif ( gbEnabled ) return;\n\tgbEnabled = true;\n\taddStylesheet('cuzillion.1.1.css');\n\taddScript('cuzillion.1.5.js');\n}\nfunction addStylesheet(url) {\n\tvar stylesheet = document.createElement('link');\n\tstylesheet.rel = 'stylesheet';\n\tstylesheet.type = 'text/css';\n\tstylesheet.href =  url;\n\tdocument.getElementsByTagName('head')[0].appendChild(stylesheet);\n}\nfunction addScript(url) {\n\tvar script = document.createElement('script');\n\tscript.src = url;\n\tdocument.getElementsByTagName('head')[0].appendChild(script);\n}\nfunction scriptSleepOnload(sUrl) {\n\tvar now = Number(new Date());\n\tvar msg = \""<nobr>\"" + (now - gTop) + \""ms: \\\""\"" + sUrl + \""\\\"" done</nobr>\\n\"";\n\tif ( document.getElementById('loadedscripts') ) {\n\t\tdocument.getElementById('loadedscripts').innerHTML += msg;\n\t}\n\telse {\n\t\tgScriptMsg += msg;\n\t}\n}\nfunction reloadPage(url) {\n\tdocument.body.innerHTML = '';\n\tdocument.location = url;\n}\nfunction cleanText(sText) {\n    return sText.replace(/<.*?>/g, '');\n}\n</script>\n<html>\n<head>\n<title>Cuzillion</title>\n<link rel=\""icon\"" href=\""favicon.ico\"" type=\""image/x-icon\"">\n<!--\nCopyright 2008 Google Inc.\n\nLicensed under the Apache License, Version 2.0 (the \""License\"");\nyou may not use this file except in compliance with the License.\nYou may obtain a copy of the License at\n\n     http://www.apache.org/licenses/LICENSE-2.0\n\nUnless required by applicable law or agreed to in writing, software\ndistributed under the License is distributed on an \""AS IS\"" BASIS,\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\nSee the License for the specific language governing permissions and\nlimitations under the License.\n-->\n</head>\n\n<body style='margin: 0px; padding: 0px; font-family: \""Trebuchet MS\"", \""Bitstream Vera Serif\"", Utopia, \""Times New Roman\"", times, serif;'>\n\n<div style=\""background: #333; color: white; padding: 8px;\"">\n  <div style=\""float:right; margin-top: 2px;\"">\n    <a href=\""help.php#examples\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">Examples</a>&nbsp;|&nbsp;<a href=\""help.php\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">Help</a><br><a href=\""http://stevesouders.com/\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">stevesouders.com</a>\n  </div>\n  <font style=\""font-size: 2em; font-weight: bold; margin-right: 10px;\""><a href=\"".\"" style=\""color:white; text-decoration: none;\""><img border=0 src=\""logo-32x32.gif\"">&nbsp;Cuzillion</a></font><i>'cuz there are a zillion pages to check</i>\n</div>\n\n<div id=content style=\""margin: 8px;\"">\n\n<div><div style='font-weight: bold; font-size: 1.3em;'><a href='http://stevesouders.com/?ex=10100&title='>Inline Scripts Block</a></div>\n<div id=description style='font-size: 0.9em;'><div style='margin: 0 0 4px 20px; font-size: 0.9em;'><i>posted by <a href='http://stevesouders.com/'>Steve Souders</a>,&nbsp;March 24, 2009 11:41 PM</i></div>Inline scripts block downloads and rendering, just like external scripts. Any resources below an inline script don't get downloaded until the inline script finishes executing. Nothing in the page is rendered until the inline script is done executing. When you click Reload, notice that the page is white for five seconds.</div>\n</div><hr style='width: 100%; margin: 20px 0 20px 0;'>\n<!-- begin resources for inbody -->\n<!-- image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags -->\n<img src='http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=1&t=1262443132'>\n\n<!-- inline script block<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> with a 5 second execute time using HTML tags -->\n<script> var injs_now = Number(new Date()); while( injs_now + (5*1000) > Number(new Date()) ) { var tmp = injs_now; } if ( 'function' == typeof(scriptSleepOnload) ) scriptSleepOnload('inline script');</script>\n\n<!-- image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags -->\n<img src='http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=3&t=1262443132'>\n\n<!-- end resources for inbody -->\n\n<div id=floattray style=\""float: left; width: 170px; margin-right: 30px;\"">\n  <div id=step1text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div id=comptray>\n  &nbsp;\n  </div>\n</div>\n\n<div id=pageavatar style=\""float: left; width: 310px; margin-right: 30px;\"">\n  <div id=step2text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div style=\""background: #CCC; border: 1px solid black; \"">\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;HTML&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;HEAD&gt;</code>\n    <div class=\""drop\"" style=\""border: 1px solid #EEE; background: #EEE; padding: 12px 0 12px 0; width: 300px; margin: 0 0 0 4px;\"">\n\t  <ul style=\""margin: 0; padding: 0;\"" id=inhead></ul>\n\t  <div id=inheadTarget></div>\n\t</div>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/HEAD&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;BODY&gt;</code>\n    <div class=\""drop\"" style=\""border: 1px solid #EEE; background: #EEE; padding: 12px 0 12px 0; width: 300px; margin: 0 0 0 4px;\"">\n\t  <ul style=\""margin: 0; padding: 0;\"" id=inbody><li onclick='enableEdit()' id='acomp1' class='sortitem image' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp1Div class='component image' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #000080; text-align: left;'><span>image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags</span></div>\n<li onclick='enableEdit()' id='acomp2' class='sortitem injs' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp2Div class='component injs' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #080; text-align: left;'><span>inline script block<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> with a 5 second execute time using HTML tags</span></div>\n<li onclick='enableEdit()' id='acomp3' class='sortitem image' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp3Div class='component image' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #000080; text-align: left;'><span>image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags</span></div>\n</ul>\n\t  <div id=inbodyTarget></div>\n\t</div>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/BODY&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/HTML&gt;</code>\n  </div>\n  <div id=loadtime style=\""text-align: left; margin-top: 10px;\""></div>\n  <div id=loadedscripts style=\""text-align: left; margin-top: 10px; width: 300px; font-size: 0.9em;\""></div>\n</div> <!-- end pageavatar -->\n\n<div style=\""position: absolute; left: 560px;\"">\n  <div id=step3text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div id=pagesubmit style=\""text-align: left;\"">\n<nobr>\n<input type=button value=\""Edit\"" onclick=\""enableEdit()\"">&nbsp;&nbsp;\n<input type=button value=\""Reload\"" onclick=\""reloadPage('/cuzillion/?ex=10100&title=Inline+Scripts+Block&t=1262443132')\"">&nbsp;&nbsp;\n<input type=button value=\""Clear\"" onclick=\""document.location='.'\"">&nbsp;&nbsp;\n</nobr>\n </div>\n</div>\n\n<div style=\""clear: both;\"">\n</div>\n\n</div> <!-- content -->\n\n\n\n</body>\n\n</html>\n""
					}, 
					""redirectURL"": """", 
					""headersSize"": 247, 
					""bodySize"": 2725
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 2, 
					""connect"": 183, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 340, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_21396"", 
				""startedDateTime"": ""2010-01-02T15:38:47.238+01:00"", 
				""time"": 193, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://stevesouders.com/cuzillion/logo-32x32.gif"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""stevesouders.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?ex=10100&title=Inline+Scripts+Block""
						}
					], 
					""queryString"": [
					], 
					""headersSize"": 473, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 14:38:52 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Last-Modified"", 
							""value"": ""Mon, 16 Nov 2009 20:19:20 GMT""
						}, 
						{
							""name"": ""Accept-Ranges"", 
							""value"": ""bytes""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""1057""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""max-age=315360000""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Tue, 31 Dec 2019 14:38:52 GMT""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=99""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""image/gif""
						}
					], 
					""content"": {
						""size"": 1057, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 316, 
					""bodySize"": 1057
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 0, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 190, 
					""receive"": 3
				}
			}, 
			{
				""pageref"": ""page_21396"", 
				""startedDateTime"": ""2010-01-02T15:38:47.238+01:00"", 
				""time"": 1430, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=1&t=1262443132"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""1.cuzillion.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?ex=10100&title=Inline+Scripts+Block""
						}
					], 
					""queryString"": [
						{
							""name"": ""n"", 
							""value"": ""1""
						}, 
						{
							""name"": ""sleep"", 
							""value"": ""1""
						}, 
						{
							""name"": ""t"", 
							""value"": ""1262443132""
						}, 
						{
							""name"": ""type"", 
							""value"": ""gif""
						}
					], 
					""headersSize"": 498, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 14:38:52 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Mon, 01 Feb 2010 14:38:53 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""public, max-age=2592000""
						}, 
						{
							""name"": ""Last-Modified"", 
							""value"": ""Sun, 15 Jan 2006 12:00:00 GMT""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""1076""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""image/gif""
						}
					], 
					""content"": {
						""size"": 1076, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 301, 
					""bodySize"": 1076
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 193, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 1237, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_21396"", 
				""startedDateTime"": ""2010-01-02T15:38:52.243+01:00"", 
				""time"": 1400, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=3&t=1262443132"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""1.cuzillion.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?ex=10100&title=Inline+Scripts+Block""
						}
					], 
					""queryString"": [
						{
							""name"": ""n"", 
							""value"": ""3""
						}, 
						{
							""name"": ""sleep"", 
							""value"": ""1""
						}, 
						{
							""name"": ""t"", 
							""value"": ""1262443132""
						}, 
						{
							""name"": ""type"", 
							""value"": ""gif""
						}
					], 
					""headersSize"": 498, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 14:38:57 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Mon, 01 Feb 2010 14:38:58 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""public, max-age=2592000""
						}, 
						{
							""name"": ""Last-Modified"", 
							""value"": ""Sun, 15 Jan 2006 12:00:00 GMT""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""1525""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""image/gif""
						}
					], 
					""content"": {
						""size"": 1525, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 301, 
					""bodySize"": 1525
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 185, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 1215, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_20633"", 
				""startedDateTime"": ""2010-01-02T16:12:32.738+01:00"", 
				""time"": 450, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://stevesouders.com/cuzillion/?c0=bi1hfff1_0_f&c1=bi1hfff1_0_f&c2=bb0hfff0_5_f&t=1262445132270"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""stevesouders.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?ex=10100&title=Inline+Scripts+Block""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""max-age=0""
						}
					], 
					""queryString"": [
						{
							""name"": ""c0"", 
							""value"": ""bi1hfff1_0_f""
						}, 
						{
							""name"": ""c1"", 
							""value"": ""bi1hfff1_0_f""
						}, 
						{
							""name"": ""c2"", 
							""value"": ""bb0hfff0_5_f""
						}, 
						{
							""name"": ""t"", 
							""value"": ""1262445132270""
						}
					], 
					""headersSize"": 579, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 15:12:38 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""X-Powered-By"", 
							""value"": ""PHP/5.2.3""
						}, 
						{
							""name"": ""Vary"", 
							""value"": ""Accept-Encoding""
						}, 
						{
							""name"": ""Content-Encoding"", 
							""value"": ""gzip""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""2456""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""text/html""
						}
					], 
					""content"": {
						""size"": 8125, 
						""mimeType"": ""text/html"", 
						""text"": ""\n<script>\nvar gTop = Number(new Date());\nvar gScriptMsg = \""\"";\nfunction cuz_addHandler(elem, sType, fn, capture) {\n    capture = (capture) ? true : false;\n    if (elem.addEventListener) {\n        elem.addEventListener(sType, fn, capture);\n    }\n    else if (elem.attachEvent) {\n        elem.attachEvent(\""on\"" + sType, fn);\n    }\n    else {\n        // Netscape 4\n        if ( elem[\""on\"" + sType] ) {\n            // Do nothing - we don't want to overwrite an existing handler.\n        }\n        else {\n            elem[\""on\"" + sType] = fn;\n        }\n    }\n}\nfunction doOnload() {\n\tvar end = Number(new Date());\n    document.getElementById('loadtime').innerHTML = 'page load time: ' + (end - gTop) + ' ms';\n\tif ( gScriptMsg && document.getElementById('loadedscripts') ) {\n\t\tdocument.getElementById('loadedscripts').innerHTML += gScriptMsg;\n\t}\n}\ncuz_addHandler(window, 'load', doOnload);\nvar gbEnabled = false;\nfunction enableEdit() {\n\tif ( gbEnabled ) return;\n\tgbEnabled = true;\n\taddStylesheet('cuzillion.1.1.css');\n\taddScript('cuzillion.1.5.js');\n}\nfunction addStylesheet(url) {\n\tvar stylesheet = document.createElement('link');\n\tstylesheet.rel = 'stylesheet';\n\tstylesheet.type = 'text/css';\n\tstylesheet.href =  url;\n\tdocument.getElementsByTagName('head')[0].appendChild(stylesheet);\n}\nfunction addScript(url) {\n\tvar script = document.createElement('script');\n\tscript.src = url;\n\tdocument.getElementsByTagName('head')[0].appendChild(script);\n}\nfunction scriptSleepOnload(sUrl) {\n\tvar now = Number(new Date());\n\tvar msg = \""<nobr>\"" + (now - gTop) + \""ms: \\\""\"" + sUrl + \""\\\"" done</nobr>\\n\"";\n\tif ( document.getElementById('loadedscripts') ) {\n\t\tdocument.getElementById('loadedscripts').innerHTML += msg;\n\t}\n\telse {\n\t\tgScriptMsg += msg;\n\t}\n}\nfunction reloadPage(url) {\n\tdocument.body.innerHTML = '';\n\tdocument.location = url;\n}\nfunction cleanText(sText) {\n    return sText.replace(/<.*?>/g, '');\n}\n</script>\n<html>\n<head>\n<title>Cuzillion</title>\n<link rel=\""icon\"" href=\""favicon.ico\"" type=\""image/x-icon\"">\n<!--\nCopyright 2008 Google Inc.\n\nLicensed under the Apache License, Version 2.0 (the \""License\"");\nyou may not use this file except in compliance with the License.\nYou may obtain a copy of the License at\n\n     http://www.apache.org/licenses/LICENSE-2.0\n\nUnless required by applicable law or agreed to in writing, software\ndistributed under the License is distributed on an \""AS IS\"" BASIS,\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\nSee the License for the specific language governing permissions and\nlimitations under the License.\n-->\n</head>\n\n<body style='margin: 0px; padding: 0px; font-family: \""Trebuchet MS\"", \""Bitstream Vera Serif\"", Utopia, \""Times New Roman\"", times, serif;'>\n\n<div style=\""background: #333; color: white; padding: 8px;\"">\n  <div style=\""float:right; margin-top: 2px;\"">\n    <a href=\""help.php#examples\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">Examples</a>&nbsp;|&nbsp;<a href=\""help.php\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">Help</a><br><a href=\""http://stevesouders.com/\"" style=\""color: white; font-size: 0.9em; text-decoration: none;\"">stevesouders.com</a>\n  </div>\n  <font style=\""font-size: 2em; font-weight: bold; margin-right: 10px;\""><a href=\"".\"" style=\""color:white; text-decoration: none;\""><img border=0 src=\""logo-32x32.gif\"">&nbsp;Cuzillion</a></font><i>'cuz there are a zillion pages to check</i>\n</div>\n\n<div id=content style=\""margin: 8px;\"">\n\n\n<!-- begin resources for inbody -->\n<!-- image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags -->\n<img src='http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=1&t=1262445158'>\n\n<!-- image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags -->\n<img src='http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=2&t=1262445158'>\n\n<!-- inline script block<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> with a 5 second execute time using HTML tags -->\n<script> var injs_now = Number(new Date()); while( injs_now + (5*1000) > Number(new Date()) ) { var tmp = injs_now; } if ( 'function' == typeof(scriptSleepOnload) ) scriptSleepOnload('inline script');</script>\n\n<!-- end resources for inbody -->\n\n<div id=floattray style=\""float: left; width: 170px; margin-right: 30px;\"">\n  <div id=step1text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div id=comptray>\n  &nbsp;\n  </div>\n</div>\n\n<div id=pageavatar style=\""float: left; width: 310px; margin-right: 30px;\"">\n  <div id=step2text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div style=\""background: #CCC; border: 1px solid black; \"">\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;HTML&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;HEAD&gt;</code>\n    <div class=\""drop\"" style=\""border: 1px solid #EEE; background: #EEE; padding: 12px 0 12px 0; width: 300px; margin: 0 0 0 4px;\"">\n\t  <ul style=\""margin: 0; padding: 0;\"" id=inhead></ul>\n\t  <div id=inheadTarget></div>\n\t</div>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/HEAD&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;BODY&gt;</code>\n    <div class=\""drop\"" style=\""border: 1px solid #EEE; background: #EEE; padding: 12px 0 12px 0; width: 300px; margin: 0 0 0 4px;\"">\n\t  <ul style=\""margin: 0; padding: 0;\"" id=inbody><li onclick='enableEdit()' id='acomp1' class='sortitem image' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp1Div class='component image' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #000080; text-align: left;'><span>image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags</span></div>\n<li onclick='enableEdit()' id='acomp2' class='sortitem image' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp2Div class='component image' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #000080; text-align: left;'><span>image<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> on domain1 with a 1 second delay using HTML tags</span></div>\n<li onclick='enableEdit()' id='acomp3' class='sortitem injs' style='cursor: move; list-style: none; border-width: 2px; border-style: solid; border-color: #555; margin: 4px;'><div id=acomp3Div class='component injs' style='padding: 2px; font-family: Arial; text-align: center; display: block; text-decoration: none; color: white; background: #080; text-align: left;'><span>inline script block<p style='margin: 0 4px 4px 12px; font-size: 0.8em;'> with a 5 second execute time using HTML tags</span></div>\n</ul>\n\t  <div id=inbodyTarget></div>\n\t</div>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/BODY&gt;</code>\n    <code style=\""font-size: 1.2em; font-weight: bold; color: #666666; display: block;\"">&lt;/HTML&gt;</code>\n  </div>\n  <div id=loadtime style=\""text-align: left; margin-top: 10px;\""></div>\n  <div id=loadedscripts style=\""text-align: left; margin-top: 10px; width: 300px; font-size: 0.9em;\""></div>\n</div> <!-- end pageavatar -->\n\n<div style=\""position: absolute; left: 560px;\"">\n  <div id=step3text style=\""text-align: left; margin: 0 0 4px 4px; height: 50px; padding-top: 12px;\""></div>\n  <div id=pagesubmit style=\""text-align: left;\"">\n<nobr>\n<input type=button value=\""Edit\"" onclick=\""enableEdit()\"">&nbsp;&nbsp;\n<input type=button value=\""Reload\"" onclick=\""reloadPage('/cuzillion/?c0=bi1hfff1_0_f&c1=bi1hfff1_0_f&c2=bb0hfff0_5_f&t=1262445158')\"">&nbsp;&nbsp;\n<input type=button value=\""Clear\"" onclick=\""document.location='.'\"">&nbsp;&nbsp;\n</nobr>\n </div>\n</div>\n\n<div style=\""clear: both;\"">\n</div>\n\n</div> <!-- content -->\n\n\n\n</body>\n\n</html>\n""
					}, 
					""redirectURL"": """", 
					""headersSize"": 247, 
					""bodySize"": 2456
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 185, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 265, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_20633"", 
				""startedDateTime"": ""2010-01-02T16:12:33.211+01:00"", 
				""time"": 195, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://stevesouders.com/cuzillion/logo-32x32.gif"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""stevesouders.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?c0=bi1hfff1_0_f&c1=bi1hfff1_0_f&c2=bb0hfff0_5_f&t=1262445132270""
						}, 
						{
							""name"": ""If-Modified-Since"", 
							""value"": ""Mon, 16 Nov 2009 20:19:20 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""max-age=0""
						}
					], 
					""queryString"": [
					], 
					""headersSize"": 577, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 304, 
					""statusText"": ""Not Modified"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 15:12:38 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=99""
						}, 
						{
							""name"": ""Etag"", 
							""value"": ""\""231b822-421-b97e8200\""""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Tue, 31 Dec 2019 15:12:38 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""max-age=315360000""
						}
					], 
					""content"": {
						""size"": 1057, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 241, 
					""bodySize"": 1057
				}, 
				""cache"": {
					""afterRequest"": {
						""expires"": ""2019-12-31T15:12:33.000Z"", 
						""lastAccess"": ""2010-01-02T15:12:38.000Z"", 
						""eTag"": """", 
						""hitCount"": 4
					}
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 0, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 195, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_20633"", 
				""startedDateTime"": ""2010-01-02T16:12:33.213+01:00"", 
				""time"": 1403, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=1&t=1262445158"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""1.cuzillion.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?c0=bi1hfff1_0_f&c1=bi1hfff1_0_f&c2=bb0hfff0_5_f&t=1262445132270""
						}
					], 
					""queryString"": [
						{
							""name"": ""n"", 
							""value"": ""1""
						}, 
						{
							""name"": ""sleep"", 
							""value"": ""1""
						}, 
						{
							""name"": ""t"", 
							""value"": ""1262445158""
						}, 
						{
							""name"": ""type"", 
							""value"": ""gif""
						}
					], 
					""headersSize"": 526, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 15:12:38 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Mon, 01 Feb 2010 15:12:39 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""public, max-age=2592000""
						}, 
						{
							""name"": ""Last-Modified"", 
							""value"": ""Sun, 15 Jan 2006 12:00:00 GMT""
						}, 
						{
							""name"": ""Content-Length"", 
							""value"": ""1076""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""image/gif""
						}
					], 
					""content"": {
						""size"": 1076, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 301, 
					""bodySize"": 1076
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 190, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 1213, 
					""receive"": 0
				}
			}, 
			{
				""pageref"": ""page_20633"", 
				""startedDateTime"": ""2010-01-02T16:12:33.213+01:00"", 
				""time"": 1448, 
				""request"": {
					""method"": ""GET"", 
					""url"": ""http://1.cuzillion.com/bin/resource.cgi?type=gif&sleep=1&n=2&t=1262445158"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Host"", 
							""value"": ""1.cuzillion.com""
						}, 
						{
							""name"": ""User-Agent"", 
							""value"": ""Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2b6pre) Gecko/20091230 Namoroka/3.6b6pre (.NET CLR 3.5.30729)""
						}, 
						{
							""name"": ""Accept"", 
							""value"": ""image/png,image/*;q=0.8,*/*;q=0.5""
						}, 
						{
							""name"": ""Accept-Language"", 
							""value"": ""en-us,en;q=0.5""
						}, 
						{
							""name"": ""Accept-Encoding"", 
							""value"": ""gzip,deflate""
						}, 
						{
							""name"": ""Accept-Charset"", 
							""value"": ""ISO-8859-1,utf-8;q=0.7,*;q=0.7""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""115""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""keep-alive""
						}, 
						{
							""name"": ""Referer"", 
							""value"": ""http://stevesouders.com/cuzillion/?c0=bi1hfff1_0_f&c1=bi1hfff1_0_f&c2=bb0hfff0_5_f&t=1262445132270""
						}
					], 
					""queryString"": [
						{
							""name"": ""n"", 
							""value"": ""2""
						}, 
						{
							""name"": ""sleep"", 
							""value"": ""1""
						}, 
						{
							""name"": ""t"", 
							""value"": ""1262445158""
						}, 
						{
							""name"": ""type"", 
							""value"": ""gif""
						}
					], 
					""headersSize"": 526, 
					""bodySize"": -1
				}, 
				""response"": {
					""status"": 200, 
					""statusText"": ""OK"", 
					""httpVersion"": ""HTTP/1.1"", 
					""cookies"": [
					], 
					""headers"": [
						{
							""name"": ""Date"", 
							""value"": ""Sat, 02 Jan 2010 15:12:38 GMT""
						}, 
						{
							""name"": ""Server"", 
							""value"": ""Apache""
						}, 
						{
							""name"": ""Expires"", 
							""value"": ""Mon, 01 Feb 2010 15:12:39 GMT""
						}, 
						{
							""name"": ""Cache-Control"", 
							""value"": ""public, max-age=2592000""
						}, 
						{
							""name"": ""Last-Modified"", 
							""value"": ""Sun, 15 Jan 2006 12:00:00 GMT""
						}, 
						{
							""name"": ""Keep-Alive"", 
							""value"": ""timeout=2, max=100""
						}, 
						{
							""name"": ""Connection"", 
							""value"": ""Keep-Alive""
						}, 
						{
							""name"": ""Transfer-Encoding"", 
							""value"": ""chunked""
						}, 
						{
							""name"": ""Content-Type"", 
							""value"": ""image/gif""
						}
					], 
					""content"": {
						""size"": 1525, 
						""mimeType"": ""image/gif""
					}, 
					""redirectURL"": """", 
					""headersSize"": 307, 
					""bodySize"": 1525
				}, 
				""cache"": {
				}, 
				""timings"": {
					""dns"": 0, 
					""connect"": 190, 
					""blocked"": 0, 
					""send"": 0, 
					""wait"": 1258, 
					""receive"": 0
				}
			}
		]
	}
}"))).ToString());
		}
	}
}
