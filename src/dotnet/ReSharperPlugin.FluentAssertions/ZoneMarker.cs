using JetBrains.Application.BuildScript.Application.Zones;

namespace ReSharperPlugin.FluentAssertions
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<IFluentAssertionsPluginZone>
    {
    }
}
