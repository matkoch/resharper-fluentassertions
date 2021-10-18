using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.I18n.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

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
        protected abstract string GetMigrationExpressionFormat();

        /// <summary>
        /// Get allowed name identifiers
        /// </summary>
        /// <returns>Allowed name identifiers</returns>
        protected abstract List<string> GetAllowedNameIdentifiers();

        /// <summary>
        /// Create FluentAssertion equivalent expression
        /// </summary>
        /// <returns>FluentAssertion equivalent expression</returns>
        public IExpression CreateMigrationExpression(IInvocationExpression invocationExpression)
        {
            var arguments = invocationExpression.Arguments
                .Select(x => x.GetText())
                .Cast<object>()
                .ToArray();

            if (!arguments.Any())
            {
                return null;
            }

            var expressionFormat =
                string.Format(GetMigrationExpressionFormat(), arguments.GetExpressionFormatParameters());

            return invocationExpression.CreateExpression(expressionFormat, arguments);
        }

        /// <summary>
        /// Check whether expression can be replaced
        /// </summary>
        /// <param name="invocationExpression">Replaced expression</param>
        /// <returns><c>true</c> - when expression can be replaced, else <c>false</c></returns>
        public bool CanMigrate(IInvocationExpression invocationExpression)
        {
            return invocationExpression.InvokedExpression is IReferenceExpression invokedExpression &&
                   GetAllowedNameIdentifiers().Contains(invokedExpression.NameIdentifier.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationExpression"></param>
        /// <returns></returns>
        public string GetTextMessage(IInvocationExpression invocationExpression)
        {
            var nUnitAssert = invocationExpression.GetText();
            var expression = CreateMigrationExpression(invocationExpression);
            return expression is null
                ? string.Format(MessageTemplate, nUnitAssert, " with FluentAssertion equivalent")
                : string.Format(MessageTemplate, nUnitAssert, expression.GetText());
        }
    }
}