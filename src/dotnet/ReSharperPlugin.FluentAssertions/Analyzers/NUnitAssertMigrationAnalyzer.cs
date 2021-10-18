using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;
using ReSharperPlugin.FluentAssertions.Highlightings;
using ReSharperPlugin.FluentAssertions.Psi;

namespace ReSharperPlugin.FluentAssertions.Analyzers
{
    /// <inheritdoc />
    [ElementProblemAnalyzer(typeof(IInvocationExpression),
        HighlightingTypes = new[] { typeof(NUnitAssertMigrationHighlighting) })]
    public class NUnitAssertMigrationAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        private readonly IClrTypeName _nUnit = new ClrTypeName("NUnit.Framework.Assert");

        /// <inheritdoc />
        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            // TODO: not entirely sure if this is even needed
            var psiModule = element.GetPsiModule();
            var references = element.GetPsiServices().Modules.GetModuleReferences(psiModule);
            if (references.Count == 0)
            {
                return;
            }

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

            var info = qualifier.Reference.Resolve();

            // TODO: i don't think we have to check for candidates as long as we expect the code is compilable
            return info.ResolveErrorType == ResolveErrorType.MULTIPLE_CANDIDATES
                ? info.Result.Candidates.Any(IsTypeOf)
                : IsTypeOf(info.DeclaredElement);
        }

        private bool IsTypeOf([CanBeNull] IDeclaredElement declaredElement)
        {
            var declaredElementAsString = declaredElement.ConvertToString();

            return declaredElementAsString == $"Class:{_nUnit.FullName}";
        }
    }
}