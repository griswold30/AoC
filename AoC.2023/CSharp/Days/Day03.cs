using AoC.Shared;
using System.Linq;

namespace AoC._2023.Days
{
    public class Day03 : IAoCDay
    {
        public string Part1(string input)
        {
            var grid = MapFromString(input, x => x.ToString());

            var partNumbers = new List<string>();

            for (var y = 0; y < grid.Length; y++)
            {
                bool wasNumber = false;
                var numberCoordinates = new List<(int Y, int X)>();

                for (var x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x].IsNumber())
                    {
                        numberCoordinates.Add((y, x));
                        wasNumber = true;
                    }

                    if (grid[y][x].IsEmpty() || grid[y][x].IsSymbol() || x == grid[y].Length - 1)
                    {
                        if (wasNumber)
                        {
                            var isPartNumber = HasAdjacentSymbol(grid, numberCoordinates);

                            if (isPartNumber) 
                                partNumbers.Add(string.Concat(numberCoordinates.Select(c => grid[c.Y][c.X])));
                            
                            numberCoordinates.Clear();
                            wasNumber = false;
                        }
                    }
                }
            }

            return partNumbers.Select(int.Parse).Sum().ToString();
        }


        public bool HasAdjacentSymbol(string[][] grid, List<(int Y, int X)> coordinates)
        {
            var y = coordinates.First().Y;

            var maxX = coordinates.MaxBy(x => x.X).X;
            var minX = coordinates.MinBy(x => x.X).X;
            var below = y + 1;
            var above = y - 1;
            var leftX = minX - 1;
            var rightX = maxX + 1;

            //check top
            if (above != -1)
            {
                for (int x = minX - 1; x<= maxX + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= grid[0].Length) continue;

                    if (grid[above][x].IsSymbol()) return true;
                }
            }

            //check bottom
            if (below < grid.Length)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= grid[0].Length) continue;

                    if (grid[below][x].IsSymbol()) return true;
                }
            }

            if (leftX != -1 && grid[y][leftX].IsSymbol()) return true;
            if (rightX < grid[y].Length && grid[y][rightX].IsSymbol()) return true;

            return false;
        }


        public string Part2(string input)
        {
            var grid = MapFromString(input, x => x.ToString());

            var partNumbers = new List<string>();
            var gearPairs = new Dictionary<(int Y, int X), List<string>>();

            for (var y = 0; y < grid.Length; y++)
            {
                bool wasNumber = false;
                var numberCoordinates = new List<(int Y, int X)>();

                for (var x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x].IsNumber())
                    {
                        numberCoordinates.Add((y, x));
                        wasNumber = true;
                    }

                    if (grid[y][x].IsEmpty() || "!@#$%^&*(),/';][<<>?:{}+=-".Contains(grid[y][x]) || x == grid[y].Length - 1)
                    {
                        if (wasNumber)
                        {
                            var gears = FindGears(grid, numberCoordinates);

                            if (gears.Any())
                            {
                                foreach (var gear in gears)
                                {
                                    if (gearPairs.ContainsKey((gear.Y, gear.X)))
                                    {
                                        gearPairs[(gear.Y, gear.X)].Add(string.Concat(numberCoordinates.Select(c => grid[c.Y][c.X])));
                                    }
                                    else
                                    {
                                        gearPairs[(gear.Y, gear.X)] = [string.Concat(numberCoordinates.Select(c => grid[c.Y][c.X]))];
                                    }
                                }
                            }

                            numberCoordinates.Clear();
                            wasNumber = false;
                        }
                    }
                }
            }

            return gearPairs
                .Where(x => x.Value.Count == 2)
                .Aggregate(0, (acc, kvp) => acc + int.Parse(kvp.Value[0]) * int.Parse(kvp.Value[1]))
                .ToString();            
        }

        public List<(int Y, int X)> FindGears(string[][] grid, List<(int Y, int X)> coordinates)
        {
            var y = coordinates.First().Y;

            var maxX = coordinates.MaxBy(x => x.X).X;
            var minX = coordinates.MinBy(x => x.X).X;
            var below = y + 1;
            var above = y - 1;
            var leftX = minX - 1;
            var rightX = maxX + 1;

            var gears = new List<(int Y, int X)>();

            //check top
            if (above != -1)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= grid[0].Length) continue;

                    if ("*".Equals(grid[above][x]))
                        gears.Add((above, x));
                }
            }

            //check bottom
            if (below < grid.Length)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= grid[0].Length) continue;

                    if ("*".Equals(grid[below][x]))
                        gears.Add((below, x));
                }
            }

            if (leftX != -1 && "*".Equals(grid[y][leftX]))
                gears.Add((y, leftX));
            if (rightX < grid[y].Length && "*".Equals(grid[y][rightX]));
                gears.Add((y, rightX));

            return gears;
        }


        public static T[][] MapFromString<T>(string input, Func<string, T> converter, char? delimiter = null)
        {
            return input
                .Split(new[] { "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line =>
                    delimiter.HasValue && line.Contains(delimiter.Value)
                        ? line.Split(delimiter.Value).Select(converter).ToArray()
                        : line.Select(c => converter(c.ToString())).ToArray()
                )
                .ToArray();
        }
    }

    public static class Extensions
    {
        public static bool IsEmpty(this string input)
        {
            return input.Equals(".");
        }

        public static bool IsNumber(this string input)
        {
            return char.IsDigit(input[0]);
        }

        public static bool IsSymbol(this string input)
        {
            return !(input.IsEmpty() || input.IsNumber());
        }
    }
}
