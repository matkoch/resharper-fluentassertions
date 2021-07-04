using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class TestHelper
    {
        public static TestCaseData[] FileNames(string folder)
        {
            return FilteredFileNames(folder);
        }

        public static TestCaseData[] FilteredFileNames(string folder, string[] excludeItems = null)
        {
            var strings = new[] {TestContext.CurrentContext.TestDirectory, @"..\..\..\test\data", folder}.ToArray();
            var testFileDirectory = Path.Combine(strings);
            var fileNames = Directory
                .GetFiles(testFileDirectory, "*.cs")
                .Select(Path.GetFileNameWithoutExtension);

            if (excludeItems != null && excludeItems.Any())
            {
                fileNames = fileNames.Where(x => !excludeItems.Contains(x));
            }

            return fileNames
                .Select(x => new TestCaseData(x))
                .ToArray();
        }
    }
}