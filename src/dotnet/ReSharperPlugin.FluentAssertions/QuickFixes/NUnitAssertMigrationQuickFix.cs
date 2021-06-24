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
    public class NUnitAssertMigrationQuickFix : QuickFixBase
    {
        private readonly NUnitAssertMigrationHighlighting _highlighting;

        public NUnitAssertMigrationQuickFix(NUnitAssertMigrationHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        /// <inheritdoc />
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var invocationExpression = _highlighting.InvocationExpression;
            var factory = CSharpElementFactory.GetInstance(invocationExpression);
            AddUsing(invocationExpression, factory);

            var expression = CreateNotBeNullExpression(factory);
            if (expression == null)
            {
                return null;
            }

            invocationExpression.ReplaceBy(expression);

            return null;
        }

        private ICSharpExpression CreateNotBeNullExpression(CSharpElementFactory factory)
        {
            var arguments = _highlighting.InvocationExpression.Arguments;
            if (!arguments.Any())
            {
                return null;
            }

            var expression = factory.CreateExpression("$0.Should().NotBeNull()", arguments.FirstOrDefault()?.GetText());
            return expression;
        }

        private static void AddUsing(ITreeNode invocationExpression, CSharpElementFactory factory)
        {
            if (!(invocationExpression?.GetContainingFile() is ICSharpFile file))
            {
                return;
            }

            var fluentAssertionUsing = factory.CreateUsingDirective("FluentAssertions");
            if (file.ImportsEnumerable.OfType<IUsingSymbolDirective>().All(i =>
                i.ImportedSymbolName.QualifiedName != fluentAssertionUsing.ImportedSymbolName.QualifiedName))
            {
                file.AddImport(fluentAssertionUsing, true);
            }
        }

        /// <inheritdoc />
        public override string Text =>
            $"Replace '{_highlighting.InvocationExpression.GetText()}' with FluentAssertion equivalent";

        /// <inheritdoc />
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }
    }
}