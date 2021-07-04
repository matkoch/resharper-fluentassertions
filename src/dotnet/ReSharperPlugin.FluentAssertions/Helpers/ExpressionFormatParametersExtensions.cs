using System.Collections.Generic;
using System.Linq;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    public static class ExpressionFormatParametersExtensions
    {
        public static string GetExpressionFormatParameters<TType>(this IEnumerable<TType> arguments)
        {
            var groups = arguments.Skip(1).Select((item, index) => $"${index + 1}");
            return string.Join(", ", groups);
        }
    }
}