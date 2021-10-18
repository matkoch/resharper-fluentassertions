using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.Reflection;

namespace ReSharperPlugin.FluentAssertions.Psi
{
    /// <summary>
    /// PSI extensions for determine whether a project referencing a certain assembly
    /// </summary>
    public static class PsiExtensions
    {
        /// <summary>
        /// Fluent Assertions Assembly Name
        /// </summary>
        private const string FluentAssertionsAssemblyName = nameof(FluentAssertions);

        /// <summary>
        /// NUnit Assembly Name
        /// </summary>
        private const string NUnitAssemblyName = nameof(NUnit) + "." + nameof(NUnit.Framework);

        /// <summary>
        /// Determine whether a project referencing to NUnit for ITreeNode
        /// </summary>
        /// <param name="node">Node for determine referencing to NUnit</param>
        /// <returns>
        /// <c>true</c> when project referenced to NUnit, else <c>false</c>
        /// </returns>
        public static bool IsProjectReferencedToNUnit(this ITreeNode node)
        {
            return node.GetSourceFile()?.GetProject().IsProjectReferencedToNUnit() ?? false;
        }

        /// <summary>
        /// Determine whether a project referencing to NUnit for ITreeNode
        /// </summary>
        /// <param name="project">Project for determine referencing to NUnit</param>
        /// <returns>
        /// <c>true</c> when project referenced to NUnit, else <c>false</c>
        /// </returns>
        [UsedImplicitly]
        public static bool IsProjectReferencedToNUnit(this IProject project)
        {
            return ReferencedAssembliesService.IsProjectReferencingAssemblyByName(
                project,
                project.GetCurrentTargetFrameworkId(),
                AssemblyNameInfoFactory.Create2(NUnitAssemblyName, version: null),
                out _);
        }

        /// <summary>
        /// Determine whether a project referencing to FluentAssertions for ITreeNode
        /// </summary>
        /// <param name="node">Node for determine referencing to FluentAssertions</param>
        /// <returns>
        /// <c>true</c> when project referenced to FluentAssertions, else <c>false</c>
        /// </returns>
        public static bool IsProjectReferencedToFluentAssertions(this ITreeNode node)
        {
            return node.GetSourceFile()?.GetProject().IsProjectReferencedToFluentAssertions() ?? false;
        }

        /// <summary>
        /// Determine whether a project referencing to FluentAssertions for ITreeNode
        /// </summary>
        /// <param name="project">Project for determine referencing to FluentAssertions</param>
        /// <returns>
        /// <c>true</c> when project referenced to FluentAssertions, else <c>false</c>
        /// </returns>
        [UsedImplicitly]
        public static bool IsProjectReferencedToFluentAssertions(this IProject project)
        {
            return ReferencedAssembliesService.IsProjectReferencingAssemblyByName(
                project,
                project.GetCurrentTargetFrameworkId(),
                AssemblyNameInfoFactory.Create2(FluentAssertionsAssemblyName, version: null),
                out _);
        }

        public static bool IsNUnitAssert(this ITypeElement element)
        {
            if (element == null)
                return false;

            var type = element.Module.GetFluentAssertionsPredefinedType().Assert.GetTypeElement();
            return element.IsDescendantOf(type);
        }
    }
}