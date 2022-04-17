using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        var result = 0;
        
        // assert
        {caret}Assert.NotZero(result, "Format message with param1: {0} and param2: {1}", true, false);
    }
}