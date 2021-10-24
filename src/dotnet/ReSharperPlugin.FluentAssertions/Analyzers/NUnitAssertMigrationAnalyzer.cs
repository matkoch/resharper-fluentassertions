using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperPlugin.FluentAssertions.Highlightings;
using ReSharperPlugin.FluentAssertions.Psi;

namespace ReSharperPlugin.FluentAssertions.Analyzers
{
    /// <inheritdoc />
    [ElementProblemAnalyzer(typeof(IInvocationExpression),
        HighlightingTypes = new[] { typeof(NUnitAssertMigrationHighlighting) })]
    public class NUnitAssertMigrationAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        /// <inheritdoc />
        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            if (!element.IsProjectReferencedToFluentAssertions() ||
                !element.IsProjectReferencedToNUnit())
            {
                return;
            }

            if (IsMemberAccessExpressionTypeOf(element))
            {
                consumer.AddHighlighting(new NUnitAssertMigrationHighlighting(element));
            }
        }

        private bool IsMemberAccessExpressionTypeOf([CanBeNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression?.Reference == null)
            {
                return false;
            }

            var expression = invocationExpression.InvokedExpression as IReferenceExpression;

            if (!(expression?.ConditionalQualifier is IReferenceExpression qualifier))
            {
                return false;
            }

            var typeMember = qualifier.Reference.Resolve().DeclaredElement as ITypeElement;
            
            return typeMember.IsNUnitAssert();

        }
    }
}