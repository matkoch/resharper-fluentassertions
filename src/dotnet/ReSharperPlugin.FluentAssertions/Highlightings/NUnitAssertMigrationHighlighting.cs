using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.FluentAssertions.Highlightings
{
    /// <inheritdoc />
    [RegisterConfigurableSeverity(
        SeverityId,
        CompoundItemName: null,
        Group: HighlightingGroupIds.NUnit,
        Title: Message,
        Description: Message,
        DefaultSeverity: Severity.WARNING)]
    [ConfigurableSeverityHighlighting(
        SeverityId,
        CSharpLanguage.Name,
        OverlapResolve = OverlapResolveKind.ERROR,
        OverloadResolvePriority = 0,
        ToolTipFormatString = Message)]
    public class NUnitAssertMigrationHighlighting : IHighlighting
    {
        public const string SeverityId = "NUnitAssertMigration";
        public const string Message = "NUnit assertion can be replaced with FluentAssertions equivalents";

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
        public string ToolTip => Message;

        /// <inheritdoc />
        public string ErrorStripeToolTip => ToolTip;
    }
}
