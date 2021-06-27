using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using NUnit.Framework;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertNotBeNullMigrationService : INUnitAssertMigrationService
    {
        /// <inheritdoc />
        public ICSharpExpression CreateMigrationExpression(CSharpElementFactory factory,
            IInvocationExpression invocationExpression)
        {
            var arguments = invocationExpression.Arguments;

            if (!arguments.Any())
            {
                return null;
            }

            var expression = factory.CreateExpression("$0.Should().NotBeNull()", arguments.FirstOrDefault()?.GetText());

            return expression;
        }

        /// <inheritdoc />
        public bool CanMigrate(IInvocationExpression invocationExpression)
        {
            return invocationExpression.InvokedExpression is IReferenceExpression invokedExpression &&
                   (invokedExpression.NameIdentifier.Name == nameof(Assert.NotNull) ||
                    invokedExpression.NameIdentifier.Name == nameof(Assert.IsNotNull));
        }
    }
}