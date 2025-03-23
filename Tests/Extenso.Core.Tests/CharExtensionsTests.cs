namespace Extenso.Core.Tests;

public class CharExtensionsTests
{
    [Fact]
    public void Repeat()
    {
        string expected = "_____";
        string actual = '_'.Repeat(5);
        Assert.Equal(expected, actual);
    }
}