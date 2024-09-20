using System.Collections.Generic;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent(Instantiation.DemandAnyThreadSafe)]
    public class AssertNotBeNullMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "NotBeNull";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string>
            {
                "NotNull",
                "IsNotNull"
            };
        }
    }
}
