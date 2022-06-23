using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.Util;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.FluentAssertions.Psi;

public static class NUnitPredefinedTypeExtensions
{
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
        return project.IsProjectReferencedTo(NUnitPredefinedType.AssemblyName);
    }

    private static readonly Key<NUnitPredefinedTypeCache> NUnitTypeCacheKey =
        new(nameof(NUnitPredefinedTypeCache));


    public static bool IsNUnitAssert(this ITypeElement element)
    {
        if (element == null)
        {
            return false;
        }

        var nUnitTypes = element.Module.GetNUnitPredefinedType().PredefinedNUnitTypes;
        return nUnitTypes.Any(x => element.IsDescendantOf(x.GetTypeElement()));
    }

    private static NUnitPredefinedType GetNUnitPredefinedType([NotNull] this IPsiModule module)
    {
        var predefinedTypeCache = module.GetOrCreateDataNoLock(
            NUnitTypeCacheKey,
            module, static x => x.GetPsiServices().GetComponent<NUnitPredefinedTypeCache>());

        return predefinedTypeCache.GetOrCreateNUnitPredefinedType(module);
    }
}