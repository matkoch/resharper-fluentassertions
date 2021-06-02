using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.QuickFixes
{
    [QuickFix]
    public class NUnitAssertNotNullQuickFix : QuickFixBase
    {
        private readonly NUnitAssertNotNullHighlighting _highlighting;

        public NUnitAssertNotNullQuickFix(NUnitAssertNotNullHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        /// <inheritdoc />
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var invocationExpression = _highlighting.InvocationExpression;
            if (!(invocationExpression?.GetContainingFile() is ICSharpFile file))
            {
                return null;
            }

            var arguments = _highlighting.InvocationExpression.Arguments;
            if (!arguments.Any())
            {
                return null;
            }

            var factory = CSharpElementFactory.GetInstance(invocationExpression);
            var expression = factory.CreateExpression("$0.Should().BeNotNull()", arguments.FirstOrDefault()?.GetText());

            invocationExpression.ReplaceBy(expression);
            var fluentAssertionUsing = factory.CreateUsingDirective("FluentAssertions");
            if (file.ImportsEnumerable.OfType<IUsingSymbolDirective>().All(i =>
                i.ImportedSymbolName.QualifiedName != fluentAssertionUsing.ImportedSymbolName.QualifiedName))
            {
                file.AddImport(fluentAssertionUsing, true);
            }

            return null;
        }

        /// <inheritdoc />
        public override string Text =>
            $"Replace '{_highlighting.InvocationExpression.GetText()}' with '{_highlighting.InvocationExpression.Arguments.FirstOrDefault()?.GetText()}.Should().BeNotNull()'";

        /// <inheritdoc />
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }
    }
}