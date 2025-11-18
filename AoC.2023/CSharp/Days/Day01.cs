using AoC.Shared;
using System.Diagnostics;

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
                ["nine"] = 9
            };

            var reversedDictionary = ReversedDictionary(translationDictionary);

            var numbersAsWords = translationDictionary.Keys.ToList();
            var reversedNumberAsWords = reversedDictionary.Keys.ToList();

            int sumOfNumbers = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var foundFirst = false;
                var foundLast = false;

                int firstDigit = 0;
                int lastDigit = 0;

                for (int j = 0; j < line.Length; j++)
                {
                    for (int k = 0; k < line.Length; k++)
                    {
                        var startString = line.Substring(j, k + 1);
                        var endString = line[^(j + k + 1)..^j];

                        if (!foundFirst)
                        {
                            foundFirst = int.TryParse(startString, out firstDigit);
                            foundFirst = foundFirst || translationDictionary.TryGetValue(startString, out firstDigit);
                        }

                        if (!foundLast)
                        {
                            foundLast = int.TryParse(endString, out lastDigit);
                            foundLast = foundLast || translationDictionary.TryGetValue(endString, out lastDigit);
                        }

                        if (foundFirst && foundLast) break;

                        if ((!foundFirst && numbersAsWords.Any(x => x.StartsWith(startString, StringComparison.CurrentCultureIgnoreCase))) ||
                            (!foundLast && numbersAsWords.Any(x => x.EndsWith(endString, StringComparison.CurrentCultureIgnoreCase))))
                        {
                            continue;
                        }

                        break;
                    }

                    if (foundFirst && foundLast)
                    {
                        Debug.WriteLine($"Found Numbers in line {firstDigit} and {lastDigit}: {line}");
                        sumOfNumbers += int.Parse($"{firstDigit}{lastDigit}");
                        break;
                    }
                }
            }

            return sumOfNumbers.ToString();
        }

        private Dictionary<string, int> ReversedDictionary(Dictionary<string, int> dict)
        {
            Dictionary<string, int> reversedDictionary = new();

            foreach (var (key, value) in dict)
            {
                reversedDictionary[new String(key.Reverse().ToArray())] = value;
            }

            return reversedDictionary;
        }
    }
}