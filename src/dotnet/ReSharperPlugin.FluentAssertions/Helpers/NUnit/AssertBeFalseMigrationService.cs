using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertBeFalseMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "BeFalse";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string>
            {
                "False",
                "IsFalse"
            };
        }
    }
}