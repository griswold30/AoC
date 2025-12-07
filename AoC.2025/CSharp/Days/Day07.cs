using AoC.Shared;
using System.Diagnostics;

namespace AoC._2025.Days
{
    public class Day07 : IAoCDay
    {
        public string Part1(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            int rows = lines.Length;
            int cols = lines[0].Length;  

            string[,] grid = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    grid[r, c] = lines[r][c].ToString();
                }
            }

            var startingPosition = FindStartingPosition(grid);
            var splits = FindTachyonBeamSplits(grid, startingPosition, 0);

            return splits.ToString();
        }
        public string Part2(string input)
        {
            string[] lines = input
               .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            int rows = lines.Length;
            int cols = lines[0].Length;

            string[,] grid = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    grid[r, c] = lines[r][c].ToString();
                }
            }

            var startingPosition = FindStartingPosition(grid);
            var memoizedSplits = new Dictionary<Coordinate, long>();
            var splits = FindTachyonBeamSplits2(grid, startingPosition, memoizedSplits);

            return splits.ToString();
        }

        private int FindTachyonBeamSplits(string[,] grid, Coordinate currentPosition, int splits)
        {
            while (currentPosition.Y + 1 < grid.GetLength(0))
            {
                currentPosition = new Coordinate(currentPosition.X, currentPosition.Y + 1);

                if (grid[currentPosition.Y, currentPosition.X] == ".")
                {
                    grid[currentPosition.Y, currentPosition.X] = "|";
                }
                else if (grid[currentPosition.Y, currentPosition.X] == "|")
                    return splits;
                else
                {
                    splits++;
                    grid[currentPosition.Y, currentPosition.X - 1] = "|";
                    grid[currentPosition.Y, currentPosition.X + 1] = "|";
                    splits = FindTachyonBeamSplits(grid, new Coordinate(currentPosition.X - 1, currentPosition.Y), splits);
                    splits = FindTachyonBeamSplits(grid, new Coordinate(currentPosition.X + 1, currentPosition.Y), splits);
                    return splits;
                }
            }

            return splits;
        }

        private long FindTachyonBeamSplits2(string[,] grid, Coordinate currentPosition, Dictionary<Coordinate, long> memoizedSplits)
        {
            var splits = 0L;

            while (currentPosition.Y + 1 < grid.GetLength(0))
            {
                currentPosition = new Coordinate(currentPosition.X, currentPosition.Y + 1);

                if (grid[currentPosition.Y, currentPosition.X] != "^")
                    grid[currentPosition.Y, currentPosition.X] = "|";
                else
                {
                    if (memoizedSplits.TryGetValue(currentPosition, out long memoizedTotal))
                        return memoizedTotal;

                    grid[currentPosition.Y, currentPosition.X - 1] = "|";
                    grid[currentPosition.Y, currentPosition.X + 1] = "|";

                    splits = FindTachyonBeamSplits2(grid, new Coordinate(currentPosition.X - 1, currentPosition.Y), memoizedSplits);
                    splits += FindTachyonBeamSplits2(grid, new Coordinate(currentPosition.X + 1, currentPosition.Y), memoizedSplits);

                    memoizedSplits[currentPosition] = splits;
                    return splits;
                }
            }

            return ++splits;
        }

        private void ToDebugString(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            var sb = new System.Text.StringBuilder(rows * (cols + 1));

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                    sb.Append(grid[r, c]);

                sb.Append('\n');
            }

            Debug.WriteLine(sb.ToString());
        }

        private Coordinate FindStartingPosition(string[,] grid)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] == "S")
                        return new Coordinate(c, r);
                }
            }

            return null;
        }

        public record Coordinate(int X, int Y) { };
    }
}