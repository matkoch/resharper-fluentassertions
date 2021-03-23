using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates.Behaviors;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates.Contexts;
using JetBrains.ReSharper.Feature.Services.PostfixTemplates;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions
{
    [PostfixTemplate("testee", "Introduce testee variable", "var testee = expr;")]
    public class TesteePostfixTemplate : CSharpPostfixTemplate
    {
        public override PostfixTemplateInfo TryCreateInfo(CSharpPostfixTemplateContext context)
        {
            var withValuesContexts = CSharpPostfixUtils.FindExpressionWithValuesContexts(context);
            return withValuesContexts.Length == 0
                ? null
                : new PostfixTemplateInfo("testee", withValuesContexts, availableInPreciseMode: true);
        }

        public override PostfixTemplateBehavior CreateBehavior(PostfixTemplateInfo info)
        {
            return new TesteePostfixBehavior(info);
        }

        private sealed class TesteePostfixBehavior : CSharpStatementPostfixTemplateBehavior<ICSharpStatement>
        {
            public TesteePostfixBehavior([NotNull] PostfixTemplateInfo info)
                : base(info)
            {
            }

            protected override string ExpressionSelectTitle => "Select expression to introduce as variable";

            protected override ICSharpStatement CreateStatement(CSharpElementFactory factory,
                ICSharpExpression expression)
            {
                return factory.CreateStatement("var testee = $0;", expression);
            }
        }
    }
}
