using System.Collections.Generic;
using JetBrains.ProjectModel;
using NUnit.Framework;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertNotBeNullMigrationService : BaseNUnitAssertMigrationService
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
                nameof(Assert.NotNull),
                nameof(Assert.IsNotNull)
            };
        }
    }
}