using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LinqTools;
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
    public class NUnitAssertMigrationQuickFix : ScopedQuickFixBase
    {
        private readonly NUnitAssertMigrationHighlighting _highlighting;
        private readonly NUnitAssertMigrationServiceBase _migrationService;

        public NUnitAssertMigrationQuickFix(NUnitAssertMigrationHighlighting highlighting) 
        {
            _highlighting = highlighting;

            _migrationService = highlighting.InvocationExpression.PsiModule.GetSolution()
                .GetComponents<NUnitAssertMigrationServiceBase>()
                .FirstOrDefault(x => x.CanMigrate(_highlighting.InvocationExpression));
        }

        /// <inheritdoc />
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var invocationExpression = _highlighting.InvocationExpression;

            // TODO: this shouldn't be necessary. it also probably wouldn't account for global usings for instance
            AddUsing(invocationExpression);
            // TODO: instead, the expression should be created with the proper arguments
            // TODO: for instance: factory.CreateExpression("$0.$1(x => x > 0)", arrVariable, enumerableCountMethod);
            // TODO: but this probably makes the migration services a bit more complicated
            invocationExpression.ReplaceBy(_migrationService.CreateMigrationExpression(invocationExpression));

            return null;
        }

        private static void AddUsing(ITreeNode invocationExpression)
        {
            if (!(invocationExpression?.GetContainingFile() is ICSharpFile file))
            {
                return;
            }

            var fluentAssertionUsing = CSharpElementFactory.GetInstance(invocationExpression)
                .CreateUsingDirective(nameof(FluentAssertions));

            if (file.ImportsEnumerable.OfType<IUsingSymbolDirective>().All(i =>
                i.ImportedSymbolName.QualifiedName != fluentAssertionUsing.ImportedSymbolName.QualifiedName))
            {
                file.AddImport(fluentAssertionUsing, true);
            }
        }

        /// <inheritdoc />
        public override string Text => _migrationService.GetTextMessage(_highlighting.InvocationExpression);

        /// <inheritdoc />
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid() && _migrationService != null;
        }

        /// <inheritdoc />
        protected override ITreeNode TryGetContextTreeNode()
        {
            return _highlighting.InvocationExpression;
        }
    }
}