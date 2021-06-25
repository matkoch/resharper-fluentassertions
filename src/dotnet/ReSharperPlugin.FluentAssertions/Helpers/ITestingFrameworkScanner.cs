using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Modules;

namespace ReSharperPlugin.FluentAssertions.Helpers
{
    /// <summary>
    /// Mandatory Test Package Check Service
    /// </summary>
    public interface ITestingFrameworkScanner
    {
        /// <summary>
        /// Check contains NUnit package in project
        /// </summary>
        /// <param name="references">All references for module</param>
        /// <returns><c>true</c> - when NUnit contain in modules, else <c>false</c></returns>
        bool HasNUnit(IEnumerable<IPsiModuleReference> references);

        /// <summary>
        /// Check contains FluentAssertions package in project
        /// </summary>
        /// <param name="references">All references for module</param>
        /// <returns><c>true</c> - when FluentAssertions contain in modules, else <c>false</c></returns>
        bool HasFluentAssertions(IEnumerable<IPsiModuleReference> references);
    }
}