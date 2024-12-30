using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        TempClass result = null;
        
        // assert
        {caret}Assert.IsNotNull(result);
    }
}

public class TempClass
{
    public string Type { get; set; }
}