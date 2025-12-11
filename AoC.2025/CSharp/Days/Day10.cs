using System;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.Json;
using AoC.Shared;

namespace AoC._2025.Days
{
    public class Day10 : IAoCDay
    {
        public string Part1(string input)
        {
            var lines = input.Split('\n');

            List<string> allTargetButtonConfigurations = new();
            List<List<List<int>>> allButtonToggles = new();

            foreach (var line in lines)
            {
                var indexOfEndOfTarget = line.IndexOf(']');
                var target = line[1..indexOfEndOfTarget];
                var indexOfJoltage = line.IndexOf('{');
                var toggles = line[(indexOfEndOfTarget+1)..indexOfJoltage]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.TrimStart('(').TrimEnd(')')
                        .Split(',')
                        .Select(x => int.Parse(x))
                    .ToList()).ToList();
                allButtonToggles.Add(toggles);
                allTargetButtonConfigurations.Add(target);
            }

            int pushes = 0;
            for (int i = 0; i < allTargetButtonConfigurations.Count; i++)
            {
                var target = allTargetButtonConfigurations[i];
                var toggles = allButtonToggles[i];
                pushes += CalculateLeastAmountOfPushes(target, toggles);
            }

            return pushes.ToString();
        }

        private int CalculateLeastAmountOfPushes(string targetLightConfiguration, List<List<int>> toggles)
        {
            var q = new Queue<(int, string)>();
            q.Enqueue((0, new string('.', targetLightConfiguration.Length)));
            var buttonPresses = 0;

            while (q.Count > 0)
            {
                var state = q.Dequeue();

                foreach (var toggle in toggles)
                {
                    var lightConfiguration = ToggleLights(state.Item2, toggle);

                    if (lightConfiguration == targetLightConfiguration)
                    {
                        return ++state.Item1;
                    }
                    else
                    {
                        q.Enqueue((state.Item1 + 1, lightConfiguration));
                    }

                }
            }
            return 0;
        }

        private int CalculateLeastAmountOfPushes2(List<int> targetJoltage, List<List<int>> toggles)
        {
            var q = new Queue<(int, List<int>)>();
            q.Enqueue((0, Enumerable.Repeat(0, targetJoltage.Count()).ToList()));
            var buttonPresses = 0;

            while (q.Count > 0)
            {
                var state = q.Dequeue();

                foreach (var toggle in toggles)
                {
                    if (state.Item2.Zip(targetJoltage, (a, b) => a > b).Any(x=> x)) continue;
                    var joltage = ToggleJoltage(state.Item2, toggle);

                    if (joltage.SequenceEqual(targetJoltage))
                    {
                        return ++state.Item1;
                    }
                    else
                    {
                        q.Enqueue((state.Item1 + 1, joltage));
                    }

                }
            }
            return 0;
        }

        private List<int> ToggleJoltage(List<int> joltages, List<int> toggles)
        {
            var newJoltage = new List<int>();
            newJoltage.AddRange(joltages);

            foreach (var toggle in toggles)
            {
                newJoltage[toggle]++;
            }

            return newJoltage;
        }

        private string ToggleLights(string currentLightConfiguration, List<int> toggles)
        {
            var lightConfiguration = currentLightConfiguration.ToCharArray();

            foreach (var toggle in toggles)
            {
                if (lightConfiguration[toggle] == '.')
                {
                    lightConfiguration[toggle] = '#';
                }
                else
                {
                    lightConfiguration[toggle] = '.';
                }
            }

            return new string(lightConfiguration);
        }

        public string Part2(string input)
        {
            var lines = input.Split('\n');

            List<string> allTargetButtonConfigurations = new();
            List<List<List<int>>> allButtonToggles = new();
            List<List<int>> allJoltage = new();

            foreach (var line in lines)
            {
                var indexOfEndOfTarget = line.IndexOf(']');
                var target = line[1..indexOfEndOfTarget];
                var indexOfJoltage = line.IndexOf('{');
                var toggles = line[(indexOfEndOfTarget + 1)..indexOfJoltage]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.TrimStart('(').TrimEnd(')')
                        .Split(',')
                        .Select(x => int.Parse(x))
                    .ToList()).ToList();

                var joltage = line[(indexOfJoltage + 1)..].TrimEnd('}').Split(",").Select(int.Parse).ToList();

                allButtonToggles.Add(toggles);
                allTargetButtonConfigurations.Add(target);
                allJoltage.Add(joltage);
            }

            int pushes = 0;
            for (int i = 0; i < allTargetButtonConfigurations.Count; i++)
            {
                var toggles = allButtonToggles[i];
                var joltage = allJoltage[i];
                pushes += CalculateLeastAmountOfPushes2(joltage, toggles);
            }

            return pushes.ToString();
        }
    }
}