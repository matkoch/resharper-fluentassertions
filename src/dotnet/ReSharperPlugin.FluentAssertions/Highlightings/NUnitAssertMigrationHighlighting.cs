using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.FluentAssertions.Highlightings
{
    /// <inheritdoc />
    [StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.IdentifierHighlightings))]
    public class NUnitAssertMigrationHighlighting : IHighlighting
    {
        internal readonly IInvocationExpression InvocationExpression;

        public NUnitAssertMigrationHighlighting(IInvocationExpression invocationExpression)
        {
            InvocationExpression = invocationExpression;
        }

        /// <inheritdoc />
        public bool IsValid() => InvocationExpression.IsValid();

        /// <inheritdoc />
        public DocumentRange CalculateRange() => InvocationExpression.GetDocumentRange();

        /// <inheritdoc />
        public string ToolTip => "NUnit assertion can be replaced with FluentAssertion equivalents";

        /// <inheritdoc />
        public string ErrorStripeToolTip => ToolTip;
    }
}