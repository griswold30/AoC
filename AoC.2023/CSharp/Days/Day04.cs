using System;
using System.Security.Cryptography.X509Certificates;
using AoC.Shared;

namespace AoC._2023.Days
{
    public class Day04 : IAoCDay
    {
        public string Part1(string input)
        {
            List<string> cardStrings = input.Split("\n").ToList();
            double total = 0;

            foreach (var cardString in cardStrings)
            {
                var card = new Card(cardString);
                total += card.Worth();
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Card
    {
        private List<int> _winningNumbers = new();
        private List<int> _candidates = new();

        public Card(string input)
        {
            //"card 1: 10 | 13"
            var cardString = input.Split(":");
            var cardSplit = cardString[1].Split("|");
            var winningString = cardSplit[0];
            var candidateString = cardSplit[1];

            foreach (var winner in winningString.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                _winningNumbers.Add(int.Parse(winner));
            }

            foreach (var candidate in candidateString.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                _candidates.Add(int.Parse(candidate));
            }
        }

        public double Worth()
        {
            var numMatches = _winningNumbers.Intersect(_candidates).Count();

            if (numMatches == 0) return 0;

            return Math.Pow(2, numMatches - 1);
        }
    }
}