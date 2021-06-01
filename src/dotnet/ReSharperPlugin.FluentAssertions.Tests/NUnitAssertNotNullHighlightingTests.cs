using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    [TestNet50("NUnit/3.12.0")]
    public class NUnitAssertNotNullHighlightingTests : CSharpHighlightingTestBase
    {
        protected override string RelativeTestDataPath => nameof(NUnitAssertNotNullHighlightingTests);

        protected override bool HighlightingPredicate(
            IHighlighting highlighting,
            IPsiSourceFile sourceFile,
            IContextBoundSettingsStore settingsStore)
        {
            return highlighting is NUnitAssertNotNullHighlighting;
        }

        [TestCase("PositiveCase")]
        public void ShouldDetectHighlighting(string testName)=> DoOneTest(testName);

        [TestCase("NegativeCase")]
        public void ShouldNotBeDetectHighlighting(string testName)=> DoOneTest(testName);
    }
}