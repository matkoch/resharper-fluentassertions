using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Highlightings;

namespace ReSharperPlugin.FluentAssertions.Tests
{
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

        [TestNet50("NUnit/3.12.0", "FluentAssertions/5.10.3")]
        [TestCase("PositiveCaseIsNotNull")]
        [TestCase("PositiveCaseNotNull")]
        public void ShouldDetectHighlightingWhenProjectHasReferenceToNUnitAndFluentAssertions(string testName) =>
            DoOneTest(testName);

        [TestCase("PositiveCaseWithoutReferences")]
        [TestCase("NegativeCase")]
        public void ShouldNotBeDetectHighlighting(string testName) => DoOneTest(testName);

        [TestNet50("FluentAssertions/5.10.3")]
        [TestCase("PositiveCaseWithoutReferences")]
        public void ShouldNotBeDetectHighlightingWhenProjectHasNoReferenceToNUnit(string testName) =>
            DoOneTest(testName);

        [TestNet50("NUnit/3.12.0")]
        [TestCase("PositiveCaseWithoutReferences")]
        public void ShouldNotBeDetectHighlightingWhenProjectHasNoReferenceToFluentAssertions(string testName) =>
            DoOneTest(testName);
    }
}