using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertAreEqualMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName() => "Be";

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement() => new List<string> { "AreEqual" };

        /// <inheritdoc />
        protected override ICSharpExpression GetActualValue(IEnumerable<ICSharpExpression> arguments)
        {
            return arguments.Skip(1).First();
        }

        /// <inheritdoc />
        protected override ICSharpExpression GetExpectedValue(IEnumerable<ICSharpExpression> arguments)
        {
            return arguments.First();
        }
    }
}
