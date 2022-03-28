using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertBeNullMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "BeNull";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string>
            {
                "Null",
                "IsNull"
            };
        }
    }
}