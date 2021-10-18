using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Psi
{
    public class FluentAssertionsPredefinedType
    {
        private static readonly Dictionary<IClrTypeName, int> s_typeNameIndex;

        public static readonly IClrTypeName ASSERT_FQN =
            new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.Assert));

        static FluentAssertionsPredefinedType()
        {
            // Collect predefined type names through reflection
            s_typeNameIndex = new Dictionary<IClrTypeName, int>();

            foreach (var info in typeof(FluentAssertionsPredefinedType).GetFields())
            {
                if (info.IsStatic && typeof(IClrTypeName).IsAssignableFrom(info.FieldType))
                {
                    var clrTypeName = (IClrTypeName) info.GetValue(obj: null);
                    s_typeNameIndex.Add(clrTypeName, s_typeNameIndex.Count);
                }
            }
        }

        [NotNull] private readonly IDeclaredType[] _types = new IDeclaredType[s_typeNameIndex.Count];

        internal FluentAssertionsPredefinedType([NotNull] IPsiModule module)
        {
            Module = module;
        }

        [NotNull] public IPsiModule Module { get; }

        [NotNull] public IDeclaredType Assert => CreateType(ASSERT_FQN);

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
                        _types[index] = TypeFactory.CreateTypeByCLRName(clrName, Module);
                }
            }

            return _types[index];
        }

        private IDeclaredType CreateType([NotNull] IClrTypeName clrName)
        {
            return CreateType(s_typeNameIndex[clrName], clrName);
        }
    }
}