using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using ReSharperPlugin.FluentAssertions.Psi;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <summary>
    /// Service for creation migration expression from NUnit Assert to FluentAssertion equivalent
    /// </summary>
    public abstract class NUnitAssertMigrationServiceBase
    {
        private const string MessageTemplate = "Replace {0} with {1}";

        /// <summary>
        /// Get migration expression format
        /// </summary>
        /// <returns>Migration expression format</returns>
        protected abstract string GetReplacementMethodName();

        /// <summary>
        /// Get allowed method names to replacement
        /// </summary>
        /// <returns>Allowed method names to replacement</returns>
        protected abstract List<string> GetAllowedMethodNamesToReplacement();

        /// <summary>
        /// Create FluentAssertion equivalent expression
        /// </summary>
        /// <returns>FluentAssertion equivalent expression</returns>
        [NotNull]
        public IExpression CreateMigrationExpression(IInvocationExpression invocationExpression)
        {
            var arguments = invocationExpression.Arguments
                .Select(x => x.Value)
                .ToArray();
            if (!arguments.Any())
                return invocationExpression;

            var factory = CSharpElementFactory.GetInstance(invocationExpression);
            var actualValue = GetActualValue(arguments);
            var shouldMethod = invocationExpression.GetFluentAssertionsPredefinedType()
                .GetShouldMethod(actualValue.Type());
            if (shouldMethod is null)
                return invocationExpression;

            var shouldExpression = factory.CreateReferenceExpression("$0.$1", actualValue, shouldMethod);
            var replacementMethod = GetReplacementMethod(shouldMethod);
            if (replacementMethod is null)
                return invocationExpression;

            var replacementMethodExpression = factory.CreateReferenceExpression("$0().$1", shouldExpression, replacementMethod);
            var list = new List<object> { replacementMethodExpression };
            var expectedValue = GetExpectedValue(arguments);
            if (expectedValue != null)
            {
                list.Add(expectedValue);
                list.AddRange(arguments.Skip(2));
            }
            else
            {
                list.AddRange(arguments.Skip(1));
            }

            var expressionFormat = $"$0({list.GetExpressionFormatArguments()})";
            return factory.CreateExpression(expressionFormat, list.ToArray());
        }

        /// <summary>
        /// Check whether expression can be replaced
        /// </summary>
        /// <param name="invocationExpression">Replaced expression</param>
        /// <returns><c>true</c> - when expression can be replaced, else <c>false</c></returns>
        public bool CanMigrate(IInvocationExpression invocationExpression)
        {
            return invocationExpression.InvokedExpression is IReferenceExpression invokedExpression &&
                   GetAllowedMethodNamesToReplacement().Contains(invokedExpression.NameIdentifier.Name);
        }

        /// <summary>
        /// Get text message for quickfix description
        /// </summary>
        /// <param name="invocationExpression">Replaced expression</param>
        /// <returns>Text message for quickfix description</returns>
        public string GetTextMessage(IInvocationExpression invocationExpression)
        {
            var nUnitAssert = invocationExpression.GetText();
            var expression = CreateMigrationExpression(invocationExpression);

            return string.Format(MessageTemplate, nUnitAssert, expression.GetText());
        }

        /// <summary>
        /// Get actual value for generate equivalent expression
        /// </summary>
        /// <param name="arguments">Assert invocation expression arguments</param>
        /// <returns>Actual argument expression</returns>
        protected virtual ICSharpExpression GetActualValue(IEnumerable<ICSharpExpression> arguments)
        {
            return arguments.FirstOrDefault();
        }

        /// <summary>
        /// Get expected value for generate equivalent expression
        /// </summary>
        /// <param name="arguments">Assert invocation expression arguments</param>
        /// <returns>Expected argument expression</returns>
        protected virtual ICSharpExpression GetExpectedValue(IEnumerable<ICSharpExpression> arguments)
        {
            return null;
        }

        private IMethod GetReplacementMethod(IParametersOwner shouldMethod)
        {
            var returnType = shouldMethod.ReturnType.GetTypeElement();

            var result = returnType
                ?.Methods
                .FirstOrDefault(x => x.ShortName == GetReplacementMethodName());

            if (result != null) return result;

            return returnType.GetSuperTypesWithoutCircularDependent()
                .SelectMany(x => x.GetTypeElement()?.Methods)
                .FirstOrDefault(x => x.ShortName == GetReplacementMethodName());
        }
    }
}
