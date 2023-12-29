using System.Text.RegularExpressions;

namespace AdventOfCode.Days1to5;
public class Day2(string inputFilename) : IDay
{
    private readonly Regex _redRegex = new(@"(?<num>\d+) red");
    private readonly Regex _greenRegex = new(@"(?<num>\d+) green");
    private readonly Regex _blueRegex = new(@"(?<num>\d+) blue");

    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        var gameRedMax = 12;
        var gameGreenMax = 13;
        var gameBlueMax = 14;

        var gameIdSuccessSum = _input.Select(
            (line, index) => GetMaxMatch(line, _redRegex) <= gameRedMax && GetMaxMatch(line, _greenRegex) <= gameGreenMax && GetMaxMatch(line, _blueRegex) <= gameBlueMax
            // Game ID actually starts at 1, not 0 
            ? index + 1 : 0)
            .Sum();
        Console.WriteLine(gameIdSuccessSum);
    }

    public void Part2()
    {
        var totalPower = _input.Select(line => GetMaxMatch(line, _redRegex) * GetMaxMatch(line, _greenRegex) * GetMaxMatch(line, _blueRegex)).Sum();
        Console.WriteLine(totalPower);
    }

    private static int GetMaxMatch(string line, Regex regex)
    {
        var matches = regex.Matches(line);
        return matches.Count > 0 ? matches.Select(x => int.Parse(x.Groups["num"].Value)).Max() : 0;
    }
}