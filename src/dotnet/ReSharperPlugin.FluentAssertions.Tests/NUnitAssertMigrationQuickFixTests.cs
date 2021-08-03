using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.QuickFixes;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class NUnitAssertMigrationQuickFixTests : CSharpQuickFixTestBase<NUnitAssertMigrationQuickFix>
    {
        protected override string RelativeTestDataPath => nameof(NUnitAssertMigrationQuickFixTests);

        [TestNet50("NUnit/3.12.0", "FluentAssertions/5.10.3")]
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames),
            new object[] {nameof(NUnitAssertMigrationQuickFixTests)})]
        public void ShouldApplyQuickFix(string testName) =>
            DoOneTest(testName);
    }
}