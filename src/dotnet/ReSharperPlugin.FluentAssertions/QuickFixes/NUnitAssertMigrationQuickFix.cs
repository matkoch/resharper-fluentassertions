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
using ReSharperPlugin.FluentAssertions.Helpers.NUnit;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.QuickFixes
{
    /// <inheritdoc />
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
            var migrationServices = solution.GetComponents<INUnitAssertMigrationService>();

            var migrationService = migrationServices.FirstOrDefault(x => x.CanMigrate(invocationExpression));

            var expression = migrationService?.CreateMigrationExpression(factory, invocationExpression);
            if (expression == null)
            {
                return null;
            }

            AddUsing(invocationExpression, factory);
            invocationExpression.ReplaceBy(expression);
            return null;
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