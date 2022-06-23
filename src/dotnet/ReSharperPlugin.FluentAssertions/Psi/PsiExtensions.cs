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
            return IsProjectReferencedTo(project, FluentAssertionsAssemblyName);
        }

        /// <summary>
        /// Determine whether a project referencing to concrete assembly name for ITreeNode
        /// </summary>
        /// <param name="project">Project for determine referencing to concrete assembly name</param>
        /// <param name="assemblyName">Name referencing assembly</param>
        /// <returns>
        /// <c>true</c> when project referenced to concrete assembly name, else <c>false</c>
        /// </returns>
        [UsedImplicitly]
        public static bool IsProjectReferencedTo(this IProject project, string assemblyName)
        {
            return ReferencedAssembliesService.IsProjectReferencingAssemblyByName(
                project,
                project.GetCurrentTargetFrameworkId(),
                AssemblyNameInfoFactory.Create2(assemblyName, version: null),
                out _);
        }
    }
}