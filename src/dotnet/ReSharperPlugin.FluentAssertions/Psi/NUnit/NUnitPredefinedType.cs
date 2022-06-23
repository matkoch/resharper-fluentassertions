using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Psi;

/// <summary>
/// File based at Resharper class <see cref="PredefinedType"/>
/// </summary>
public class NUnitPredefinedType
{
    /// <summary>
    /// Collection predefined NUnit types
    /// </summary>
    [NotNull] public readonly HashSet<IDeclaredType> PredefinedNUnitTypes =
        new(typeof(NUnitPredefinedType).GetFields().Count(x => x.IsStatic));

    /// <summary>
    /// NUnit.Framework.Assert
    /// </summary>
    public static readonly IClrTypeName AssertFqn =
        new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.Assert));

    /// <summary>
    /// NUnit.Framework.CollectionAssert
    /// </summary>
    public static readonly IClrTypeName CollectionAssertFqn =
        new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.CollectionAssert));

    /// <summary>
    /// NUnit.Framework.DirectoryAssert
    /// </summary>
    public static readonly IClrTypeName DirectoryAssertFqn =
        new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.DirectoryAssert));

    /// <summary>
    /// NUnit.Framework.FileAssert
    /// </summary>
    public static readonly IClrTypeName FileAssertFqn =
        new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.FileAssert));

    /// <summary>
    /// NUnit.Framework.StringAssert
    /// </summary>
    public static readonly IClrTypeName StringAssertFqn =
        new ClrTypeName(nameof(NUnit) + "." + nameof(NUnit.Framework) + "." + nameof(NUnit.Framework.StringAssert));

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="module">PSI module</param>
    internal NUnitPredefinedType([NotNull] IPsiModule module)
    {
        PredefinedNUnitTypes.Add(CreateType(AssertFqn, module));
        PredefinedNUnitTypes.Add(CreateType(CollectionAssertFqn, module));
        PredefinedNUnitTypes.Add(CreateType(DirectoryAssertFqn, module));
        PredefinedNUnitTypes.Add(CreateType(FileAssertFqn, module));
        PredefinedNUnitTypes.Add(CreateType(StringAssertFqn, module));
    }

    private IDeclaredType CreateType(IClrTypeName clrTypeName, IPsiModule module)
    {
        return TypeFactory.CreateTypeByCLRName(clrTypeName, NullableAnnotation.Unknown, module);
    }
}