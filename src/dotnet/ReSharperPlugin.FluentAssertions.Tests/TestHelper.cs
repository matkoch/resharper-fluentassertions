using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class TestHelper
    {
        public static TestCaseData[] FileNames(string folder)
        {
            var strings = new[] {TestContext.CurrentContext.TestDirectory, @"..\..\..\test\data", folder}.ToArray();
            var testFileDirectory = Path.Combine(strings);

            return Directory
                .GetFiles(testFileDirectory, "*.cs")
                .Select(x => new TestCaseData(Path.GetFileNameWithoutExtension(x)))
                .ToArray();
        }
    }
}