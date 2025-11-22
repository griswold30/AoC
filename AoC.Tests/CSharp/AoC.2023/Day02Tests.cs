using Xunit;
using AoC._2023.Days;

namespace AoC.Tests._2023.Days
{
    public class Day02Tests
    {
        private readonly Day02 _day = new();

        [Fact]
        public void Part1_ExampleInput_ReturnsExpected()
        {
            string input = "";
            var result = _day.Part1(input);
            Assert.Equal("", result);
        }

        [Fact]
        public void Part2_ExampleInput_ReturnsExpected()
        {
            string input = "";
            var result = _day.Part2(input);
            Assert.Equal("", result);
        }
    }
}