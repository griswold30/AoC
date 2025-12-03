using AoC.Shared;
using System.Text;

namespace AoC._2025.Days
{
    public class Day02 : IAoCDay
    {
        public string Part1(string input)
        {
            var lines = input.Split(',');
            var sumOfInvalidIds = 0L;

            foreach (var line in lines)
            {
                var ids = line
                    .Split('-')
                    .Select(x => long.Parse(x.TrimStart("0")))
                    .ToArray();

                var numberOfIds = ids[1] - ids[0];
                var invalidIds = LongRange(ids[0], numberOfIds + 1).Where(x =>
                {
                    var id = x.ToString();

                    if (id.Length % 2 == 0)
                    {
                        return id[..(id.Length / 2)] == id[(id.Length / 2)..];
                    }

                    return false;
                });

                sumOfInvalidIds += invalidIds.Sum();
            }

            return sumOfInvalidIds.ToString();
        }

        private IEnumerable<long> LongRange(long start, long count)
        {
            for (long i = start; i < start + count; i++)
                yield return i;
        }

        public string Part2(string input)
        {
            var lines = input.Split(',');
            var sumOfInvalidIds = 0L;
            var invalidIdsToPrint = new StringBuilder();

            foreach (var line in lines)
            {
                var ids = line
                    .Split('-')
                    .Select(x => long.Parse(x.TrimStart("0")))
                    .ToArray();

                var numberOfIds = ids[1] - ids[0];
                var invalidIds = LongRange(ids[0], numberOfIds + 1).Where(x =>
                {
                    var id = x.ToString();

                    var repeatCandidate = id[0].ToString();

                    for (int i = 1; i <= id.Length / 2; i++)
                    {
                        if (id == string.Concat(Enumerable.Repeat(repeatCandidate, id.Length / repeatCandidate.Length)))
                        {
                            invalidIdsToPrint.Append($"{id},");
                            return true;
                        }
                        else
                            repeatCandidate += id[i];
                    }

                    return false;
                });

                sumOfInvalidIds += invalidIds.Sum();
            }

            return sumOfInvalidIds.ToString();
        }
    }
}