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

            var methodIdentifier = element.GetSolution().GetComponent<ITestingFrameworkScanner>();

            if (!methodIdentifier.IsNUnit(references))
            {
                return;
            }

            if (element.InvokedExpression.GetText() == $"{nameof(Assert)}.{nameof(Assert.IsNotNull)}")
            {
                consumer.AddHighlighting(new NUnitAssertNotNullHighlighting(element));
            }
        }
    }
}