using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        bool result = true;
        
        // assert
        {caret}Assert.False(result);
    }
}