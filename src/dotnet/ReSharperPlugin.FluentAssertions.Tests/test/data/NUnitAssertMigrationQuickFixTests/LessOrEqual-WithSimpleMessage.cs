using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        int result = 10;
        
        // assert
        {caret}Assert.LessOrEqual(19, result, "Simple message");
    }
}