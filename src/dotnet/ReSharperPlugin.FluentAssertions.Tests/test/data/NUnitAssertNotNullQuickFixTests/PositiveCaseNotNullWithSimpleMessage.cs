using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        string result = null;
        
        // assert
        {caret}Assert.NotNull(result, "Simple message");
    }
}