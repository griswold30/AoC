using Xunit;
using AoC._2025.Days;

namespace AoC.Tests._2025.Days
{
    public class Day03Tests
    {
        private readonly Day03 _day = new();

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