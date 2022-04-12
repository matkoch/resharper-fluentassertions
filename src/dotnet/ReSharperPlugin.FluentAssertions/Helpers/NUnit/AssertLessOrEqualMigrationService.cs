using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertLessOrEqualMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "BeLessThanOrEqualTo";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string> { "LessOrEqual" };
        }

        protected override ICSharpExpression GetActualValue(IEnumerable<ICSharpExpression> arguments)
        {
            return arguments.Skip(1).FirstOrDefault();
        }

        /// <inheritdoc />
        protected override ICSharpExpression GetExpectedValue(IEnumerable<ICSharpExpression> arguments)
        {
            return arguments.FirstOrDefault();
        }
    }
}
