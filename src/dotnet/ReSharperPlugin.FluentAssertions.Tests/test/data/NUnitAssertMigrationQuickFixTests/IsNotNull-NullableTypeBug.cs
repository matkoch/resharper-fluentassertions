using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        TempClass result = new();
        
        // assert
        {caret}Assert.IsNotNull(result.Type);
    }
}

public class TempClass
{
    public int? Type { get; set; }
}