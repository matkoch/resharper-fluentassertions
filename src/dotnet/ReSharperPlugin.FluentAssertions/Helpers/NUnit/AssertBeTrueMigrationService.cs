using System.Collections.Generic;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent(Instantiation.DemandAnyThreadSafe)]
    public class AssertBeTrueMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "BeTrue";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string>
            {
                "True",
                "IsTrue"
            };
        }
    }
}
