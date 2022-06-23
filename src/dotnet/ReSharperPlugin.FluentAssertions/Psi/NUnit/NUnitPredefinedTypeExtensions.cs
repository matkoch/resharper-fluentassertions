using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.Util;

namespace ReSharperPlugin.FluentAssertions.Psi;

public static class NUnitPredefinedTypeExtensions
{
    private static readonly Key<NUnitPredefinedTypeCache> NUnitTypeCacheKey =
        new(nameof(NUnitPredefinedTypeCache));

    public static NUnitPredefinedType GetNUnitPredefinedType([NotNull] this IPsiModule module)
    {
        var predefinedTypeCache = module.GetOrCreateDataNoLock(
            NUnitTypeCacheKey,
            module, static x => x.GetPsiServices().GetComponent<NUnitPredefinedTypeCache>());

        return predefinedTypeCache.GetOrCreateNUnitPredefinedType(module);
    }
}