using System.Collections.Concurrent;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Psi;

/// <summary>
/// Cache for NUnit predefined type
/// </summary>
/// <remarks>
/// Based on <see cref="FluentAssertionsPredefinedType"/>
/// </remarks>
[PsiComponent]
internal class NUnitPredefinedTypeCache : InvalidatingPsiCache
{
    private readonly ConcurrentDictionary<IPsiModule, NUnitPredefinedType> _predefinedTypes = new();

    /// <inheritdoc />
    protected override void InvalidateOnPhysicalChange(PsiChangedElementType elementType)
    {
        if (elementType == PsiChangedElementType.InvalidateCached)
        {
            return;
        }

        _predefinedTypes.Clear();
    }

    /// <summary>
    /// Get or create NUnit predefined type
    /// </summary>
    /// <param name="module">PSI Module</param>
    /// <returns>NUnit predefined type</returns>
    public NUnitPredefinedType GetOrCreateNUnitPredefinedType(IPsiModule module)
    {
        return _predefinedTypes.GetOrAdd(module, x => new NUnitPredefinedType(x));
    }
}