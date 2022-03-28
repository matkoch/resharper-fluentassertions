using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        string result = "qwerty";
        
        // assert
        {caret}Assert.AreEqual(result, "qwerty");
    }
}