using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.FluentAssertions.Helpers.NUnit
{
    /// <summary>
    /// Service for creation migration expression from NUnit Assert to FluentAssertion equivalent
    /// </summary>
    public interface INUnitAssertMigrationService
    {
        /// <summary>
        /// Create FluentAssertion equivalent expression
        /// </summary>
        /// <param name="factory">CSharp Element Factory</param>
        /// <param name="invocationExpression">Replaced expression</param>
        /// <returns>FluentAssertion equivalent expression</returns>
        ICSharpExpression CreateMigrationExpression(CSharpElementFactory factory,
            IInvocationExpression invocationExpression);

        /// <summary>
        /// Check whether expression can be replaced
        /// </summary>
        /// <param name="invocationExpression">Replaced expression</param>
        /// <returns><c>true</c> - when expression can be replaced, else <c>false</c></returns>
        bool CanMigrate(IInvocationExpression invocationExpression);
    }
}