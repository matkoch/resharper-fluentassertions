using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharperPlugin.FluentAssertions.Psi
{
    public static class FluentAssertionsPredefinedTypesExtensions
    {
        private static readonly Key<FluentAssertionsPredefinedTypeCache> s_typeCacheKey =
            new(nameof(FluentAssertionsPredefinedTypeCache));

        public static FluentAssertionsPredefinedType GetFluentAssertionsPredefinedType([NotNull] this IPsiModule module)
        {
            var predefinedTypeCache = module.GetOrCreateDataNoLock(
                s_typeCacheKey,
                module, static x => x.GetPsiServices().GetComponent<FluentAssertionsPredefinedTypeCache>());

            return predefinedTypeCache.GetOrCreateFluentAssertionsPredefinedType(module);
        }

        public static FluentAssertionsPredefinedType GetFluentAssertionsPredefinedType([NotNull] this ITreeNode context)
        {
            return GetFluentAssertionsPredefinedType(context.GetPsiModule());
        }

        [Pure]
        [ContractAnnotation("type:null => false")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPredefinedType([CanBeNull] IType type, [NotNull] IClrTypeName clrName)
        {
            return type is IDeclaredType predefinedCandidate &&
                   IsPredefinedTypeElement(predefinedCandidate.GetTypeElement(), clrName);
        }

        [Pure]
        [ContractAnnotation("typeElement:null => false")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPredefinedTypeElement([CanBeNull] ITypeElement typeElement,
            [NotNull] IClrTypeName clrName)
        {
            if (typeElement == null)
                return false;

            var predefinedType = typeElement.Module.GetFluentAssertionsPredefinedType();
            var truePredefined = predefinedType.TryGetType(clrName).NotNull("NOT PREDEFINED");
            var predefinedTypeElement = truePredefined.GetTypeElement();

            return DeclaredElementEqualityComparer.TypeElementComparer.Equals(typeElement, predefinedTypeElement);
        }

        [PsiComponent]
        internal class FluentAssertionsPredefinedTypeCache : InvalidatingPsiCache
        {
            private readonly ConcurrentDictionary<IPsiModule, FluentAssertionsPredefinedType> _predefinedTypes = new();

            protected override void InvalidateOnPhysicalChange(PsiChangedElementType elementType)
            {
                if (elementType == PsiChangedElementType.InvalidateCached)
                    return;

                _predefinedTypes.Clear();
            }

            public FluentAssertionsPredefinedType GetOrCreateFluentAssertionsPredefinedType(IPsiModule module)
            {
                return _predefinedTypes.GetOrAdd(module, x => new FluentAssertionsPredefinedType(x));
            }
        }
    }
}