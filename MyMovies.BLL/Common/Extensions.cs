using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MyMovies.BLL.Common
{
    public static class Extensions
    {
        public static IQueryable<T> OrderByPropertyOrField<T> (this IQueryable<T> queryable, string propertyOrFieldName, bool ascending = true)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }

        public static bool RegExContains(this string input, IEnumerable<string> keywords)
        {
            const string deleteSymbolsPattern = @"([+()-/\s])";

            if (input == null)
            {
                return false;
            }

            input = Regex.Replace(input, deleteSymbolsPattern, "");

            return keywords.Select(x => input.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
        }

        public static bool NumberInStringArrayContains(this int input, string[] keywords)
        {
            foreach (var t in keywords)
            {
                var result = int.TryParse(t, out int parsedNumber);

                if (result)
                {
                    return parsedNumber == input;
                }
            }

            return false;
        }
    }
}
