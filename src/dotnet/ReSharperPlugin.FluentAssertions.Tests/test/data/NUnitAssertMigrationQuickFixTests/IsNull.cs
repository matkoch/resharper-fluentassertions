using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        string result = string.Empty;
        
        // assert
        {caret}Assert.IsNull(result);
    }
}