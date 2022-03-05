using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.QuickFixes;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class NUnitAssertMigrationQuickFixTests : CSharpQuickFixTestBase<NUnitAssertMigrationQuickFix>
    {
        protected override string RelativeTestDataPath => nameof(NUnitAssertMigrationQuickFixTests);

        [TestNet60("NUnit/3.12.0", "FluentAssertions/6.5.1")]
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames),
            new object[] {nameof(NUnitAssertMigrationQuickFixTests)})]
        public void ShouldApplyQuickFix(string testName) =>
            DoOneTest(testName);
    }
}