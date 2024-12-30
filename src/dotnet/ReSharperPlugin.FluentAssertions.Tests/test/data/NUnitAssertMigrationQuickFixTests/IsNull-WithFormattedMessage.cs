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
        {caret}Assert.IsNull(result, "Format message with param1: {0} and param2: {1}", true, false);
    }
}