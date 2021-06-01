using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    public interface ITestingFrameworkScanner
    {
        bool HasNUnit(IEnumerable<IPsiModuleReference> references);
        bool HasFluentAssertion(IEnumerable<IPsiModuleReference> references);
    }
}