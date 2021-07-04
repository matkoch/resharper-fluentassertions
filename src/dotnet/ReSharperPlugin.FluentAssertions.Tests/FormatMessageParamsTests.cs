using System;
using FluentAssertions;
using NUnit.Framework;
using ReSharperPlugin.FluentAssertions.Helpers;

namespace ReSharperPlugin.FluentAssertions.Tests
{
    public class FormatMessageParamsTests
    {
        [Test]
        public void ShouldBeEmptyWhenArgumentsIsEmpty()
        {
            // Arrange
            var arguments = Array.Empty<string>();

            // Act
            var result = arguments.GetExpressionFormatParameters();

            result.Should().BeEmpty();
        }

        [Test]
        public void ShouldBeEmptyWhenHaveSingleArgument()
        {
            // Arrange
            var arguments = new[] {"result"};

            // Act
            var result = arguments.GetExpressionFormatParameters();

            result.Should().BeEmpty();
        }

        [Test]
        public void ShouldBeEmptyWhenHaveTwoArguments()
        {
            // Arrange
            var arguments = new[] {"result", "simple message"};

            // Act
            var result = arguments.GetExpressionFormatParameters();

            result.Should().Be("$1");
        }

        [Test]
        public void ShouldBeEmptyWhenHaveFiveArguments()
        {
            // Arrange
            var arguments = new[] {"result", "simple message", "param1", "param2", "param3"};

            // Act
            var result = arguments.GetExpressionFormatParameters();

            result.Should().Be("$1, $2, $3, $4");
        }
    }
}