using System.Text;
using AoC.Shared;

namespace AoC._2025.Days
{
    public class Day03 : IAoCDay
    {
        public string Part1(string input)
        {
            var lines = input.Split('\n');
            var total = 0;

            foreach (var line in lines)
            {
                var tens = 0;
                var ones = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    var digitNum = line[i] - '0';

                    if (i < line.Length - 1 &&  digitNum > tens)
                    {
                        tens = digitNum;
                        ones = 0;
                        continue;
                    }
                    else if (digitNum > ones)
                       ones = digitNum;
                }
                total += (tens * 10) + ones;
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.Split('\n');
            var total = 0L;
            var joltage = new StringBuilder();
           
            foreach (var line in lines)
            {
                var pos = 0;
                var availableSkips = line.Length - 12;

               for (int i = 0; i < 12; i++)
                {
                    var (max, newPosition) = FindBiggestNumber(line.Substring(pos, availableSkips + 1));
                    joltage.Append(max.ToString());
                    availableSkips -= newPosition; // if we skipped numbers we need to subtract them so that we don't run out of available numbers in the string.  
                    pos+= newPosition + 1; //move start of next substring 1 after position of current max
                    
                    if (joltage.Length == 12) break;
                }
                total += long.Parse(joltage.ToString());
            }

            return total.ToString();
        }

        public (int Num, int Position) FindBiggestNumber(string segment)
        {
            var max = 0;
            var position = 0;

            for (int i = 0; i < segment.Length; i++)
            {
                var num = segment[i] - '0';
                if (num > max)
                {
                    max = num;
                    position = i;
                }
            }
            return (max, position);
        }
    }
}