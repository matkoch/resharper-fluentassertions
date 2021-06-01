using System.Collections.Generic;
using FluentAssertions;
using JetBrains.ReSharper.Psi.Modules;
using Moq.AutoMock;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Helpers;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class TestingFrameworkScannerTests
    {
        [Test]
        public void ProjectContainsReferenceToNUnit()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.Setup<IPsiModule, string>(x => x.DisplayName).Returns("nunit.framework");
            mocker.Setup<IPsiModule, bool>(x => x.IsValid()).Returns(true);
            var service = mocker.CreateInstance<TestingFrameworkScanner>();

            // Act
            var result = service.HasNUnit(new List<IPsiModuleReference>
            {
                new PsiModuleReference(mocker.GetMock<IPsiModule>().Object)
            });

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ProjectDoesNotContainsReferenceToNUnit()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.Setup<IPsiModule, string>(x => x.DisplayName).Returns("xunit");
            mocker.Setup<IPsiModule, bool>(x => x.IsValid()).Returns(true);
            var service = mocker.CreateInstance<TestingFrameworkScanner>();

            // Act
            var result = service.HasNUnit(new List<IPsiModuleReference>
            {
                new PsiModuleReference(mocker.GetMock<IPsiModule>().Object)
            });

            // Assert
            result.Should().BeFalse();
        }
        [Test]
        public void ProjectContainsReferenceToFluentAssertion()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.Setup<IPsiModule, string>(x => x.DisplayName).Returns("FluentAssertions");
            mocker.Setup<IPsiModule, bool>(x => x.IsValid()).Returns(true);
            var service = mocker.CreateInstance<TestingFrameworkScanner>();

            // Act
            var result = service.HasFluentAssertions(new List<IPsiModuleReference>
            {
                new PsiModuleReference(mocker.GetMock<IPsiModule>().Object)
            });

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ProjectDoesNotContainsReferenceToFluentAssertion()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.Setup<IPsiModule, string>(x => x.DisplayName).Returns("xunit");
            mocker.Setup<IPsiModule, bool>(x => x.IsValid()).Returns(true);
            var service = mocker.CreateInstance<TestingFrameworkScanner>();

            // Act
            var result = service.HasFluentAssertions(new List<IPsiModuleReference>
            {
                new PsiModuleReference(mocker.GetMock<IPsiModule>().Object)
            });

            // Assert
            result.Should().BeFalse();
        }
    }
}