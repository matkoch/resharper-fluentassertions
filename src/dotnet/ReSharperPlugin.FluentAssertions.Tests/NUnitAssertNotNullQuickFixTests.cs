using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.QuickFixes;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class NUnitAssertNotNullQuickFixTests : CSharpQuickFixTestBase<NUnitAssertMigrationQuickFix>
    {
        protected override string RelativeTestDataPath => nameof(NUnitAssertNotNullQuickFixTests);

        [TestNet50("NUnit/3.12.0", "FluentAssertions/5.10.3")]
        [TestCase("PositiveCaseIsNotNull")]
        [TestCase("PositiveCaseNotNull")]
        public void ShouldApplyQuickFix(string testName) =>
            DoOneTest(testName);
    }
}