using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates.Behaviors;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates.Contexts;
using JetBrains.ReSharper.Feature.Services.PostfixTemplates;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions
{
    [PostfixTemplate("result", "Introduce result variable", "var result = expr;")]
    public class ResultPostfixTemplate : CSharpPostfixTemplate
    {
        public override PostfixTemplateInfo TryCreateInfo(CSharpPostfixTemplateContext context)
        {
            var withValuesContexts = CSharpPostfixUtils.FindExpressionWithValuesContexts(context);
            return withValuesContexts.Length == 0
                ? null
                : new PostfixTemplateInfo("result", withValuesContexts, availableInPreciseMode: true);
        }

        public override PostfixTemplateBehavior CreateBehavior(PostfixTemplateInfo info)
        {
            return new ResultPostfixBehavior(info);
        }

        private sealed class ResultPostfixBehavior : CSharpStatementPostfixTemplateBehavior<ICSharpStatement>
        {
            public ResultPostfixBehavior([NotNull] PostfixTemplateInfo info)
                : base(info)
            {
            }

            protected override string ExpressionSelectTitle => "Select expression to introduce as variable";

            protected override ICSharpStatement CreateStatement(CSharpElementFactory factory,
                ICSharpExpression expression)
            {
                return factory.CreateStatement("var result = $0;", expression);
            }
        }
    }
}