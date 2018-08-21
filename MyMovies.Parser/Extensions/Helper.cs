using AngleSharp.Dom;
using System;

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
    }
}