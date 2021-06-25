using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    /// <inheritdoc />
    [SolutionComponent]
    public class TestingFrameworkScanner : ITestingFrameworkScanner
    {
        private readonly ClrTypeName _nunit = new ClrTypeName("nunit.framework");
        private readonly ClrTypeName _fluentAssertion = new ClrTypeName("FluentAssertions");

        /// <inheritdoc />
        public bool HasNUnit(IEnumerable<IPsiModuleReference> references)
        {
            return references.Any(x => x.Module.DisplayName == _nunit.FullName);
        }

        /// <inheritdoc />
        public bool HasFluentAssertions(IEnumerable<IPsiModuleReference> references)
        {
            return references.Any(x => x.Module.DisplayName == _fluentAssertion.FullName);
        }
    }
}