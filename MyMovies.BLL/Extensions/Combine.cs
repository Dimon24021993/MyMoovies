using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyMovies.BLL.Extensions
{
    public static class PathUtil
    {
        public static string Combine(params string[] relativeParts)
        {
            var regExp = new Regex("^/");
            var parts = new List<string> { Directory.GetCurrentDirectory() };

            relativeParts.All(part =>
                {
                    parts.Add(regExp.Replace(part, ""));
                    return true;
                }
            );

            return Path.Combine(parts.ToArray());
        }
    }
}
