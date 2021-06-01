using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    public interface ITestingFrameworkScanner
    {
        bool IsNUnit(IEnumerable<IPsiModuleReference> references);
    }
}