using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.FluentAssertions.Tests
{
    [ZoneDefinition]
    public class FluentAssertionsTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<IFluentAssertionsPluginZone>
    {
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>,
        IRequire<FluentAssertionsTestEnvironmentZone>
    {
    }
    
    [SetUpFixture]
    public class FluentAssertionsTestsAssembly : ExtensionTestEnvironmentAssembly<FluentAssertionsTestEnvironmentZone> { }
}