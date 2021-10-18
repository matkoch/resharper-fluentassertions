using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertNotBeNullMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetMigrationExpressionFormat()
        {
            return "$0.Should().NotBeNull({0})";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedNameIdentifiers()
        {
            return new List<string>
            {
                "NotNull",
                "IsNotNull"
            };
        }
    }
}