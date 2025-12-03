using AoC.Shared;

namespace AoC._2025.Days
{
    public class Day01 : IAoCDay
    {
        public string Part1(string input)
        {
            var instructionArray = input.Split("\n");

            var position = 50;
            var landedOnZero = 0;

            foreach (var instruction in instructionArray)
            {
                char direction = instruction[0];
                int clicks = int.Parse(instruction[1..].ToString());

                position = position + ((direction == 'L' ? -1 : 1) * clicks);
                
                if (position % 100 == 0)
                {
                    landedOnZero++;
                }
            }

            return landedOnZero.ToString();
        }

        public string Part2(string input)
        {
            var instructionArray = input.Split("\n");

            var position = 50;
            var passedZero = 0;

            foreach (var instruction in instructionArray)
            {
                char direction = instruction[0];
                int clicks = int.Parse(instruction[1..].ToString());

                for (var i = 0; i < clicks; i++)
                {
                    position += (direction == 'L' ? -1 : 1);
                    if (position % 100 == 0)
                        passedZero++;
                }    
            }

            return passedZero.ToString();
        }
    }
}