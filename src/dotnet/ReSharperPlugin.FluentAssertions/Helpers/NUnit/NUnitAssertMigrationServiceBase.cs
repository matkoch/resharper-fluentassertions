using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
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
        [NotNull]
        public IExpression CreateMigrationExpression(IInvocationExpression invocationExpression)
        {
            var arguments = invocationExpression.Arguments
                .Select(x => x.Value)
                .Cast<object>()
                .ToArray();

            if (!arguments.Any()) return invocationExpression;

            /// this shouldn't be necessary. it also probably wouldn't account for global usings for instance
            /// instead, the expression should be created with the proper arguments
            /// for instance: factory.CreateExpression("$0.$1(x => x > 0)", arrVariable, enumerableCountMethod);
            /// but this probably makes the migration services a bit more complicated

            var factory = CSharpElementFactory.GetInstance(invocationExpression);

            var referenceExpression = factory.CreateReferenceExpression("$0.$1", arguments.First(), "Should");


            var fullExpression = factory.CreateReferenceExpression("$0().$1", referenceExpression, "NotBeNull");
            var list = new List<object>
            {
                fullExpression
            };
            list.AddRange(arguments.Skip(1));
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

            return string.Format(MessageTemplate, nUnitAssert, expression.GetText());
        }
    }
}