using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        bool result = false;
        
        // assert
        {caret}Assert.IsTrue(result);
    }
}