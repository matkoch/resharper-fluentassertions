using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Helpers;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.Analyzers
{
    [ElementProblemAnalyzer(typeof(IInvocationExpression),
        HighlightingTypes = new[] {typeof(NUnitAssertMigrationHighlighting)})]
    public class NUnitAssertMigrationAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        /// <inheritdoc />
        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            var psiModule = element.GetPsiModule();
            var references = element.GetPsiServices().Modules.GetModuleReferences(psiModule);
            if (references.Count == 0)
            {
                return;
            }

            var testingFrameworkScanner = psiModule.GetSolution().GetComponent<ITestingFrameworkScanner>();

            if (!testingFrameworkScanner.HasNUnit(references) ||
                !testingFrameworkScanner.HasFluentAssertions(references))
            {
                return;
            }


            if (IsMemberAccessExpressionTypeOf<Assert>(element))
            {
                consumer.AddHighlighting(new NUnitAssertMigrationHighlighting(element));
            }
        }

        private bool IsMemberAccessExpressionTypeOf<TType>([CanBeNull] IInvocationExpression invocationExpression)
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

            var info = qualifier.Reference.Resolve();

            return info.ResolveErrorType == ResolveErrorType.MULTIPLE_CANDIDATES
                ? info.Result.Candidates.Any(IsTypeOf<TType>)
                : IsTypeOf<TType>(info.DeclaredElement);
        }

        private bool IsTypeOf<TType>([CanBeNull] IDeclaredElement declaredElement)
        {
            var declaredElementAsString = declaredElement.ConvertToString();

            return declaredElementAsString == $"Class:{typeof(TType).FullName}";
        }
    }
}