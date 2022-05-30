namespace Extenso.Core.Tests
{
    public class Int32ExtensionsTests
    {
        [Fact]
        public void Between_True()
        {
            int source = 33;
            int lower = 10;
            int higher = 50;

            Assert.True(source.Between(lower, higher));
        }

        [Fact]
        public void Between_False()
        {
            int source = 98;
            int lower = 10;
            int higher = 50;

            Assert.False(source.Between(lower, higher));
        }

        [Fact]
        public void IsMultipleOf_True()
        {
            Assert.True(10.IsMultipleOf(2));
        }

        [Fact]
        public void IsMultipleOf_False()
        {
            Assert.False(10.IsMultipleOf(3));
        }
    }
}