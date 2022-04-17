using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <inheritdoc />
    [SolutionComponent]
    public class AssertNotZeroMigrationService : NUnitAssertMigrationServiceBase
    {
        /// <inheritdoc />
        protected override string GetReplacementMethodName()
        {
            return "NotBe";
        }

        /// <inheritdoc />
        protected override List<string> GetAllowedMethodNamesToReplacement()
        {
            return new List<string> { "NotZero" };
        }

        /// <inheritdoc />
        protected override ICSharpExpression GetExpectedValue(IEnumerable<ICSharpExpression> arguments)
        {
            var first = arguments.First();
            return CSharpElementFactory.GetInstance(first).CreateExpression("0");
        }

        /// <inheritdoc />
        protected override int GetExpectedValueSkipArgumentsCount()
        {
            return 1;
        }
    }
}