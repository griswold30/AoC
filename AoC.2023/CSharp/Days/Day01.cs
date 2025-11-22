using AoC.Shared;

namespace AoC._2023.Days
{
    public class Day01 : IAoCDay
    {
        public string Part1(string input)
        {
            var lines = input.Split('\n');

            int sumOfNumbers = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                char? firstNumber = null;
                char? lastNumber = null;

                for (int j = 0; j < line.Length; j++)
                {
                    var currentStartingChar = line[j];
                    var currentEndingChar = line[line.Length - 1 - j];

                    if (firstNumber is null && Char.IsDigit(currentStartingChar))
                        firstNumber = currentStartingChar;
                    if (lastNumber is null && Char.IsDigit(currentEndingChar))
                        lastNumber = currentEndingChar;

                    if (firstNumber is not null && lastNumber is not null)
                        break;
                }

                if (firstNumber is not null && lastNumber is not null)
                {
                    var concatenatedNumber = $"{firstNumber}{lastNumber}";
                    sumOfNumbers += int.Parse(concatenatedNumber);
                }
            }

            return sumOfNumbers.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.Split('\n');
            var translationDictionary = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3,
                ["four"] = 4,
                ["five"] = 5,
                ["six"] = 6,
                ["seven"] = 7,
                ["eight"] = 8,
                ["nine"] = 9,
                ["1"] = 1,
                ["2"] = 2,
                ["3"] = 3,
                ["4"] = 4,
                ["5"] = 5,
                ["6"] = 6,
                ["7"] = 7,
                ["8"] = 8,
                ["9"] = 9,
            };

            int sumOfNumbers = 0;
            for (var l = 0; l < lines.Length; l++)
            {
                var line = lines[l];
                int firstNumber = -1;
                int lastNumber = -1;

                int firstIndex = line.Length;
                int lastIndex = -1;

                foreach (var kvp in translationDictionary)
                {
                    var indexOfKey = line.IndexOf(kvp.Key);
                    
                    if (indexOfKey != -1 && indexOfKey < firstIndex)
                    {
                        firstIndex = indexOfKey;
                        firstNumber = kvp.Value;
                    }

                    indexOfKey = line.LastIndexOf(kvp.Key);
                    if (indexOfKey > lastIndex)
                    {
                        lastIndex = indexOfKey;
                        lastNumber = kvp.Value;
                    }

                }
            
                sumOfNumbers += firstNumber * 10 + lastNumber;
            }

            return sumOfNumbers.ToString();
        }
    }
}