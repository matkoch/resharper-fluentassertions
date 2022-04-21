using NUnit.Framework;

public class ExampleTests
{
    [Test]
    public void Test()
    {
        // arrange

        // act
        var result = Lang.En;
        
        // assert
        {caret}Assert.AreEqual(Lang.En, result);
    }

    public enum Lang
    {
        En,
        Ru
    }
}