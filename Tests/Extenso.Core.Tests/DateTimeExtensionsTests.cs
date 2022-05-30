namespace Extenso.Core.Tests
{
    public class DateTimeExtensionsTests
    {
        private static DateTime testDate = new DateTime(2022, 5, 23, 0, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void EndOfWeek()
        {
            var expected = new DateTime(2022, 5, 28);
            var actual = testDate.EndOfWeek();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseUnixTimestamp()
        {
            int unixTimestamp = 1653264000;
            var result = DateTimeExtensions.ParseUnixTimestamp(unixTimestamp);
            Assert.Equal(testDate, result);
        }

        [Fact]
        public void StartOfMonth()
        {
            var expected = new DateTime(2022, 5, 1);
            var actual = testDate.StartOfMonth();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StartOfWeek()
        {
            var expected = new DateTime(2022, 5, 22);
            var actual = testDate.StartOfWeek();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StartOfYear()
        {
            var expected = new DateTime(2022, 1, 1);
            var actual = testDate.StartOfYear();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToISO8601DateString()
        {
            string expected = "2022-05-23T00:00:00.000Z";
            string actual = testDate.ToISO8601DateString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUnixTimestamp()
        {
            long expected = 1653264000;
            long actual = testDate.ToUnixTimestamp();
            Assert.Equal(expected, actual);
        }
    }
}