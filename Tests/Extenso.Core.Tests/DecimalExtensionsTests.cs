namespace Extenso.Core.Tests;

public class DecimalExtensionsTests
{
    [Fact]
    public void Between_True()
    {
        decimal source = 33.45m;
        int lower = 10;
        int higher = 50;

        Assert.True(source.Between(lower, higher));
    }

    [Fact]
    public void Between_False()
    {
        decimal source = 98.67m;
        int lower = 10;
        int higher = 50;

        Assert.False(source.Between(lower, higher));
    }
}