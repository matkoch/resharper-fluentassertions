using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    public static class ExpressionFormatParametersExtensions
    {
        public static string GetExpressionFormatArguments<TType>(this IEnumerable<TType> arguments)
        {
            var groups = arguments.Skip(1).Select((item, index) => FormattableString.Invariant($"${index + 1}"));
            return string.Join(", ", groups);
        }
    }
}