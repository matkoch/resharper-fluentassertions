using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Helpers;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.Analyzers
{
    [ElementProblemAnalyzer(typeof(IInvocationExpression),
        HighlightingTypes = new[] {typeof(NUnitAssertNotNullHighlighting)})]
    public class NUnitAssertNotNullAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        /// <inheritdoc />
        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            var references = element.GetPsiModule().GetPsiServices().Modules
                .GetModuleReferences(element.GetPsiModule());
            if (references.Count == 0)
            {
                return;
            }

            var testingFrameworkScanner = element.GetSolution().GetComponent<ITestingFrameworkScanner>();

            if (!testingFrameworkScanner.HasNUnit(references) ||
                !testingFrameworkScanner.HasFluentAssertions(references))
            {
                return;
            }

            var invocationCodeString = element.InvokedExpression.GetText();
            if (invocationCodeString == $"{nameof(Assert)}.{nameof(Assert.IsNotNull)}" ||
                invocationCodeString == $"{nameof(Assert)}.{nameof(Assert.NotNull)}")
            {
                consumer.AddHighlighting(new NUnitAssertNotNullHighlighting(element));
            }
        }
    }
}