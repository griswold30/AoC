using AoC.Shared;

namespace AoC._2023.Days
{
    public class Day02 : IAoCDay
    {
        //Wasn't feeling Regex tonight
        public string Part1(string input)
        {
            List<string> gameStrings = input.Split("\n").ToList();

            var gameSum = 0;

            for(int i = 0; i < gameStrings.Count(); i++)
            {
                var game = new Game(gameStrings[i]);
                if (game.IsValid()) gameSum += i + 1;
            }

            return gameSum.ToString();
        }

        public string Part2(string input)
        {
            List<string> gameStrings = input.Split("\n").ToList();

            var sumOfPower = 0;

            for (int i = 0; i < gameStrings.Count(); i++)
            {
                var game = new Game(gameStrings[i]);
                sumOfPower += game.Power();
            }

            return sumOfPower.ToString();
        }
    }

    public class Game
    {
        public List<Set> Sets;

        private int _maxBlue = 0;
        private int _maxRed = 0;
        private int _maxGreen = 0;

        public Game(string input)
        {
            input = input.Trim();
            var sets = input.Split(":")[1].Split(";");

            Sets = new List<Set>();
            foreach (var setString in sets)
            {
                var set = new Set(setString);
                Sets.Add(set);

                var countBlue = set.Cubes.Where(x => x.CubeColor == Cube.Color.blue).FirstOrDefault()?.Count ?? 0;
                var countRed = set.Cubes.Where(x => x.CubeColor == Cube.Color.red).FirstOrDefault()?.Count ?? 0;
                var countGreen = set.Cubes.Where(x => x.CubeColor == Cube.Color.green).FirstOrDefault()?.Count ?? 0;
                if (countBlue > _maxBlue) _maxBlue = countBlue;
                if (countGreen > _maxGreen) _maxGreen = countGreen;
                if (countRed > _maxRed) _maxRed = countRed;
            }
        }

        public bool IsValid()
        {
            return Sets.All(x => x.IsValid());
        }

        public int Power() => _maxBlue * _maxGreen * _maxRed;
    }

    public class Set
    {
        public List<Cube> Cubes { get; }

        public Set(string input)
        {
            input = input.Trim();
            var parts = input.Split(", ");

            Cubes = new List<Cube>();

            foreach (var part in parts)
            {
                Cubes.Add(new Cube(part));
            }
        }

        public bool IsValid()
        {
            return !Cubes.Any(x => _validCounts[x.CubeColor] < x.Count);
        }

        private Dictionary<Cube.Color, int> _validCounts = new Dictionary<Cube.Color, int>
        {
            { Cube.Color.red, 12 },
            { Cube.Color.green, 13 },
            { Cube.Color.blue, 14 }
        };

    }

    public class Cube
    {
        public enum Color
        {
            blue = 0,
            green = 1,
            red = 2
        }

        public int Count { get; }
        public Color CubeColor { get; }

        public Cube (string input)
        {
            input = input.Trim();
            var parts = input.Split(" ");
            Count = int.Parse(parts[0]);
            CubeColor = Enum.Parse<Color>(parts[1]);
        }
    }
}