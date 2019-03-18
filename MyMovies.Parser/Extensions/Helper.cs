using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace MyMovies.Parser.Extensions
{
    public static class Helper
    {
        public static string GetStyle(this IElement element, string propName)
        {
            try
            {
                var style = element.GetAttribute("style");
                var startIndex = style.IndexOf(":", style.IndexOf(propName, StringComparison.Ordinal) + propName.Length, StringComparison.Ordinal);
                var res = style.Substring(startIndex + 1, style.IndexOf(";", startIndex, StringComparison.Ordinal) - startIndex - 1);
                return res;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static IHtmlDocument GetDocument(this HttpClient client, string href, string charset = null)
        {
            string res;
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

        public static string InnerString(this string source, string startPattern, string endPattern)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(startPattern) ||
                string.IsNullOrWhiteSpace(endPattern)) return "";
            var startIndex = source.IndexOf(startPattern, StringComparison.Ordinal);
            if (startIndex < 0) return "";
            var endIndex = source.IndexOf(endPattern, startIndex + startPattern.Length, StringComparison.Ordinal);
            if (endIndex < 0) return "";

            return source.Substring(startIndex + startPattern.Length, endIndex - startIndex - startPattern.Length);
        }
    }
}