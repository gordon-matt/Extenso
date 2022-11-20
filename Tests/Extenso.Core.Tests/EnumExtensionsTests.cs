using System.ComponentModel.DataAnnotations;

namespace Extenso.Core.Tests
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void GetDisplayName_DisplayAttribute()
        {
            var source = TestEnum.ValueTwo;
            string expected = "2nd Value";
            string actual = EnumExtensions.GetDisplayName(source);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDisplayName_NoDisplayAttribute()
        {
            var source = DayOfWeek.Wednesday;
            string expected = "Wednesday";
            string actual = EnumExtensions.GetDisplayName(source);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDisplayNames()
        {
            var expected = new[] { "1st Value", "2nd Value", "3rd Value", "4th Value", "5th Value" };
            var actual = EnumExtensions.GetDisplayNames<TestEnum>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFlags()
        {
            var flags = TestEnum.ValueOne | TestEnum.ValueThree | TestEnum.ValueFive;
            var expected = new[] { TestEnum.ValueOne, TestEnum.ValueThree, TestEnum.ValueFive };
            var actual = EnumExtensions.GetFlags<TestEnum>(flags);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValues()
        {
            var expected = new[] { TestEnum.ValueOne, TestEnum.ValueTwo, TestEnum.ValueThree, TestEnum.ValueFour, TestEnum.ValueFive };
            var actual = EnumExtensions.GetValues<TestEnum>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parse()
        {
            var expected = TestEnum.ValueFour;
            var actual = EnumExtensions.Parse<TestEnum>("ValueFour");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToEnum()
        {
            var expected = TestEnum.ValueFour;
            var actual = "ValueFour".ToEnum<TestEnum>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryParse()
        {
            var expected = TestEnum.ValueFour;
            bool succeeded = EnumExtensions.TryParse("ValueFour", out TestEnum actual);
            Assert.True(succeeded && expected == actual);
        }

        [Flags]
        private enum TestEnum
        {
            [Display(Name = "1st Value")]
            ValueOne = 1,

            [Display(Name = "2nd Value")]
            ValueTwo = 2,

            [Display(Name = "3rd Value")]
            ValueThree = 4,

            [Display(Name = "4th Value")]
            ValueFour = 8,

            [Display(Name = "5th Value")]
            ValueFive = 16
        }
    }
}