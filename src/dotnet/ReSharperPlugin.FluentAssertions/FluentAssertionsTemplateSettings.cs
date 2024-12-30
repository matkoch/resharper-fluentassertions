using System.IO;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;

namespace ReSharperPlugin.FluentAssertions
{
    [MacroImplementation(Definition = typeof (LastResultMacroDef))]
    public class LastResultMacroImpl : SimpleMacroImplementation
    {
        public override HotspotItems GetLookupItems(IHotspotContext context)
        {
            return MacroUtil.SimpleEvaluateResult(Evaluate(context) ?? "");
        }

        private static string Evaluate(IHotspotContext context)
        {
            var solution = context.SessionContext.Solution;
            var psiSourceFile = context.ExpressionRange.Document.GetPsiSourceFile(solution);
            if (psiSourceFile == null)
                return null;

            using (ReadLockCookie.Create())
            {
                solution.GetPsiServices().Files.AssertAllDocumentAreCommitted();

                var primaryPsiFile = psiSourceFile.GetPrimaryPsiFile();
                if (primaryPsiFile == null)
                    return null;

                var treeTextRange = primaryPsiFile.Translate(context.ExpressionRange);
                if (!(primaryPsiFile.FindTokenAt(treeTextRange.StartOffset) is ITokenNode tokenAt))
                    return "";

                var previousShouldInvocation = tokenAt.FindPreviousNode(x =>
                {
                    if (x is ITypeMemberDeclaration)
                        return TreeNodeActionType.IGNORE_SUBTREE;

                    if (x is IInvocationExpression invocationExpression)
                    {
                        var referenceExpression = invocationExpression.InvokedExpression as IReferenceExpression;
                        return referenceExpression?.NameIdentifier.Name == "Should" &&
                               referenceExpression.QualifierExpression?.GetText() != "a" &&
                               !invocationExpression.ContainsLineBreak()
                            ? TreeNodeActionType.ACCEPT
                            : TreeNodeActionType.CONTINUE;
                    }

                    return TreeNodeActionType.CONTINUE;
                }) as IInvocationExpression;

                if (previousShouldInvocation == null)
                    return "result";

                var referenceExpression2 = (IReferenceExpression) previousShouldInvocation.InvokedExpression;
                var qualifier = referenceExpression2.QualifierExpression;
                return qualifier?.GetText() ?? "result";
            }
        }
        // public override HotspotItems GetLookupItems(IHotspotContext context) => MacroUtil.SimpleEvaluateResult(LastResultMacroImpl.Evaluate(this.myArgument));
        //
        // private static string Evaluate(IMacroParameterValueNew argument)
        // {
        //     Logger.Assert(argument != null, "The condition (argument != null) is false.");
        //     if (argument == null)
        //         return "<Format not specified>";
        //     string format = argument.GetValue();
        //     try
        //     {
        //         return format.Length > 0 ? DateTime.Now.ToString(format) : DateTime.Now.ToLongTimeString();
        //     }
        //     catch (FormatException ex)
        //     {
        //         return "<Invalid Format>";
        //     }
        // }
    }

    [MacroDefinition("getAssertionObject",
        ResourceType = typeof(Resources),
        DescriptionResourceName = nameof(Resources.LastResultMacroDefDescription),
        LongDescriptionResourceName = nameof(Resources.LastResultMacroDefDescription))]
    public class LastResultMacroDef : SimpleMacroDefinition
    {
        // public override ParameterInfo[] Parameters => new ParameterInfo[1]
        // {
        //     new ParameterInfo(ParameterType.String)
        // };
    }

    [ShellComponent]
    public class FluentAssertionsTemplateSettings : IHaveDefaultSettingsStream
    {
        public string Name => "FluentAssertions Template Settings";

        public Stream GetDefaultSettingsStream(Lifetime lifetime)
        {
            var manifestResourceStream = typeof(FluentAssertionsTemplateSettings).Assembly
                .GetManifestResourceStream(typeof(FluentAssertionsTemplateSettings).Namespace + ".Templates.DotSettings").NotNull();
            lifetime.OnTermination(manifestResourceStream);
            return manifestResourceStream;
        }
    }
}
