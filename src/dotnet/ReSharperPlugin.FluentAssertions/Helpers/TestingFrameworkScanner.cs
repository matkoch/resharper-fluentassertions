using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    [SolutionComponent]
    public class TestingFrameworkScanner : ITestingFrameworkScanner
    {
        private readonly ClrTypeName _nunit = new ClrTypeName("nunit.framework");

        /// <inheritdoc />
        public bool IsNUnit(IEnumerable<IPsiModuleReference> references)
        {
            return references.Any(x => x.Module.DisplayName == _nunit.FullName);
        }
    }
}