using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
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
            using (WriteLockCookie.Create())
            {
                var invocationExpression = _highlighting.InvocationExpression;
                
                ModificationUtil.ReplaceChild(invocationExpression,
                    _migrationService.CreateMigrationExpression(invocationExpression));
            }

            return null;
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