using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day2(string[] input) : IDay
    {
        private readonly Regex _redRegex = new(@"(?<num>\d+) red");
        private readonly Regex _greenRegex = new(@"(?<num>\d+) green");
        private readonly Regex _blueRegex = new(@"(?<num>\d+) blue");

        public void Part1()
        {
            var gameRedMax = 12;
            var gameGreenMax = 13;
            var gameBlueMax = 14;

            var gameIdSuccessSum = 0;

            for (var game = 0; game < input.Length; game++)
            {
                if (GetMaxMatch(input[game], _redRegex) <= gameRedMax && GetMaxMatch(input[game], _greenRegex) <= gameGreenMax && GetMaxMatch(input[game], _blueRegex) <= gameBlueMax)
                {
                    // Game ID actually starts at 1, not 0
                    gameIdSuccessSum += (game + 1);
                }
            }
            Console.WriteLine(gameIdSuccessSum);
        }

        public void Part2()
        {
            var totalPower = input.Select(line => GetMaxMatch(line, _redRegex) * GetMaxMatch(line, _greenRegex) * GetMaxMatch(line, _blueRegex)).Sum();
            Console.WriteLine(totalPower);
        }

        private static int GetMaxMatch(string line, Regex regex)
        {
            var matches = regex.Matches(line);
            return matches.Count > 0 ? matches.Select(x => int.Parse(x.Groups["num"].Value)).Max() : 0;
        }
    }
}