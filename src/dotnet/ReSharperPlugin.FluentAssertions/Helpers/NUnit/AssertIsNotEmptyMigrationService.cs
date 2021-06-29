using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using NUnit.Framework;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertIsNotEmptyMigrationService : INUnitAssertMigrationService
    {
        /// <inheritdoc />
        public ICSharpExpression CreateMigrationExpression(CSharpElementFactory factory,
            IInvocationExpression invocationExpression)
        {
            var arguments = invocationExpression.Arguments
                .Select(x => x.GetText())
                .Cast<object>()
                .ToArray();

            if (!arguments.Any())
            {
                return null;
            }

            var expressionFormat = $"$0.Should().NotBeEmpty({arguments.GetExpressionFormatParameters()})";
           
            var expression = factory.CreateExpression(expressionFormat, arguments);

            return expression;
        }

        /// <inheritdoc />
        public bool CanMigrate(IInvocationExpression invocationExpression)
        {
            return invocationExpression.InvokedExpression is IReferenceExpression invokedExpression &&
                   invokedExpression.NameIdentifier.Name == nameof(Assert.IsNotEmpty);
        }
    }
}