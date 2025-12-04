using AoC.Shared;

namespace AoC._2025.Days
{
    public class Day04 : IAoCDay
    {
        public enum Content
        {
            Empty,
            ToiletPaper
        }
            
        public string Part1(string input)
        {
            var lines = input.Split("\n");
            Content[,] grid = new Content[lines.Count(), lines.Length];

            for (int y = 0; y < lines.Count(); y++)
            {
                for (int x = 0; x < lines.Length; x++)
                {
                    var character = lines[y][x];
                    grid[y, x] = character == '.' ? Content.Empty : Content.ToiletPaper;
                }
            }

            var toiletPaperWithLessThan4RollsAdjacent = 0;
            var rollsAdjacent = 0;

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int column = 0; column < grid.GetLength(1); column++)
                {
                    if (grid[row, column] == Content.ToiletPaper)
                    {
                        rollsAdjacent = RollsAdjacentTo(grid, new(row, column));
                        if (rollsAdjacent < 4)
                            toiletPaperWithLessThan4RollsAdjacent++;
                    }
                }
            }

            return toiletPaperWithLessThan4RollsAdjacent.ToString();
        }

        public  int RollsAdjacentTo(Content[,] grid, (int Y, int X) coordinate)
        {
            var rollsAdjacent = 0;

            if (coordinate.Y - 1 >= 0)
            {
                if (coordinate.X - 1 >= 0 && grid[coordinate.Y - 1, coordinate.X - 1] == Content.ToiletPaper) rollsAdjacent++;
                if (grid[coordinate.Y - 1, coordinate.X] == Content.ToiletPaper) rollsAdjacent++;
                if (coordinate.X + 1 < grid.GetLength(1) && grid[coordinate.Y - 1, coordinate.X + 1] == Content.ToiletPaper) rollsAdjacent++;
            }

            if (coordinate.X - 1 >= 0 && grid[coordinate.Y, coordinate.X - 1] == Content.ToiletPaper) rollsAdjacent++;
            if (coordinate.X + 1 < grid.GetLength(1) && grid[coordinate.Y, coordinate.X + 1] == Content.ToiletPaper) rollsAdjacent++;

            if (coordinate.Y + 1 < grid.GetLength(0))
            {
                if (coordinate.X - 1 >= 0 && grid[coordinate.Y + 1, coordinate.X - 1] == Content.ToiletPaper) rollsAdjacent++;
                if (grid[coordinate.Y + 1, coordinate.X] == Content.ToiletPaper) rollsAdjacent++;
                if (coordinate.X + 1 < grid.GetLength(1) && grid[coordinate.Y + 1, coordinate.X + 1] == Content.ToiletPaper) rollsAdjacent++;
            }

            return rollsAdjacent;
        }

        public string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }
}