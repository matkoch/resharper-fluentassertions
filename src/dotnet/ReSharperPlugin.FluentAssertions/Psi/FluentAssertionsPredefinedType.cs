using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Util;

namespace ReSharperPlugin.FluentAssertions.Psi
{
    public class FluentAssertionsPredefinedType
    {
        private static readonly Dictionary<IClrTypeName, int> s_typeNameIndex;

        private static readonly IClrTypeName SHOULD_FQN =
            new ClrTypeName(nameof(FluentAssertions) + ".AssertionExtensions");

        private static readonly IClrTypeName ENUM_SHOULD_FQN =
            new ClrTypeName(nameof(FluentAssertions) + ".EnumAssertionsExtensions");

        static FluentAssertionsPredefinedType()
        {
            // Collect predefined type names through reflection
            s_typeNameIndex = new Dictionary<IClrTypeName, int>();

            foreach (var info in typeof(FluentAssertionsPredefinedType).GetFields())
                if (info.IsStatic && typeof(IClrTypeName).IsAssignableFrom(info.FieldType))
                {
                    var clrTypeName = (IClrTypeName) info.GetValue(null);
                    s_typeNameIndex.Add(clrTypeName, s_typeNameIndex.Count);
                }
        }

        [NotNull] private readonly IDeclaredType[] _types = new IDeclaredType[s_typeNameIndex.Count];

        internal FluentAssertionsPredefinedType([NotNull] IPsiModule module)
        {
            Module = module;
        }

        [NotNull] public IPsiModule Module { get; }


        [CanBeNull]
        public IMethod GetShouldMethod(IType type)
        {
            if (type.IsEnumType() || type.IsNullable() && type.Unlift().IsEnumType())
            {
                return TypeFactory
                    .CreateTypeByCLRName(ENUM_SHOULD_FQN, NullableAnnotation.Unknown, Module)
                    .GetTypeElement()
                    ?.Methods
                    .Where(x => x.ShortName == "Should")
                    .FirstOrDefault(x => x.Parameters.Any(t => !type.IsNullable() || t.Type.IsNullable()));
            }

            var methods = TypeFactory
                .CreateTypeByCLRName(SHOULD_FQN, NullableAnnotation.Unknown, Module)
                .GetTypeElement()
                ?.Methods
                .Where(x => x.ShortName == "Should");

            if (methods is null)
            {
                return null;
            }

            var objectType =
                TypeFactory.CreateTypeByCLRName(PredefinedType.OBJECT_FQN, NullableAnnotation.Unknown, Module);

            return methods.FirstOrDefault(
                x => x.Parameters.Any(t =>
                    t.Type.Equals(type.IsSimplePredefined() || type.IsNullable() ? type : objectType)));
        }

        [CanBeNull]
        public IDeclaredType TryGetType([NotNull] IClrTypeName clrTypeName)
        {
            return s_typeNameIndex.TryGetValue(clrTypeName, out var index)
                ? CreateType(index, clrTypeName)
                : null;
        }

        private IDeclaredType CreateType(int index, [NotNull] IClrTypeName clrName)
        {
            if (_types[index] == null)
            {
                lock (_types)
                {
                    if (_types[index] == null)
                        _types[index] = TypeFactory.CreateTypeByCLRName(clrName, NullableAnnotation.Unknown, Module);
                }
            }

            return _types[index];
        }
    }
}