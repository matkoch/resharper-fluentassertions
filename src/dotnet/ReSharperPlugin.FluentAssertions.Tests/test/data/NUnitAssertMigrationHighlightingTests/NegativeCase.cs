using FluentAssertions;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        string result = null;
        
        // assert
        result.Should().NotBeNull();
    }
}