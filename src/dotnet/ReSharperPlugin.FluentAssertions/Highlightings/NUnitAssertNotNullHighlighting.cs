using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.FluentAssertions.Highlightings
{
    [StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.IdentifierHighlightings))]
    public class NUnitAssertNotNullHighlighting : IHighlighting
    {
        private readonly IInvocationExpression _invocationExpression;

        public NUnitAssertNotNullHighlighting(IInvocationExpression invocationExpression)
        {
            _invocationExpression = invocationExpression;
        }

        /// <inheritdoc />
        public bool IsValid() => _invocationExpression.IsValid();

        /// <inheritdoc />
        public DocumentRange CalculateRange() => _invocationExpression.GetDocumentRange();

        /// <inheritdoc />
        public string ToolTip => "NUnit assertion can be replaced with FluentAssertion equivalents";

        /// <inheritdoc />
        public string ErrorStripeToolTip =>
            $"Replace '{_invocationExpression.GetText()}' with '{_invocationExpression.Arguments.FirstOrDefault()?.GetText()}.Should().BeNotNull()'";
    }
}