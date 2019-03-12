using AngleSharp.Dom;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using AngleSharp.Html.Dom;

namespace MyMovies.Parser.Extensions
{
    public static class Helper
    {
        public static string GetStyle(this IElement element, string propName)
        {
            try
            {
                var style = element.GetAttribute("style");
                var startIndex = style.IndexOf(":", style.IndexOf(propName) + propName.Length);
                var res = style.Substring(startIndex + 1, style.IndexOf(";", startIndex) - startIndex - 1);
                return res;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static IHtmlDocument GetDocument(this HttpClient client, string href, string charset = null)
        {
            var res = "";
            if (charset != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                res = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(charset), Encoding.UTF8, client.GetAsync(href).Result.Content.ReadAsByteArrayAsync().Result));
            }
            else
            {
                res = client.GetAsync(href).Result.Content.ReadAsStringAsync().Result;
            }
            var document = Program.Parser.ParseDocumentAsync(res, new CancellationToken()).Result;
            return document;
        }
    }
}