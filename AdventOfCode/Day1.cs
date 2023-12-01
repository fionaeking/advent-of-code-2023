namespace AdventOfCode;

public class Day1(string[] input) : IDay
{
    private readonly string[] _input = input;

    private readonly Dictionary<string, int> _numWords = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "six", 6 },
        { "four", 4 },
        { "five", 5 },
        { "nine", 9 },
        { "three", 3 },
        { "seven", 7 },
        { "eight", 8 }
    };

    public void Part1()
    {
        var total = 0;
        foreach (var line in _input)
        {
            var firstDigit = line.First(char.IsDigit);
            var lastDigit = line.Last(char.IsDigit);
            total += int.Parse($"{firstDigit}{lastDigit}");
        }
        Console.WriteLine($"Part one total: {total}");
    }

    public void Part2()
    {
        var totalPartTwo = 0;
        foreach (var line in input)
        {
            var firstDigit = GetDigit(line, true);
            var lastDigit = GetDigit(line, false);
            totalPartTwo += int.Parse($"{firstDigit}{lastDigit}");
        }
        Console.WriteLine($"Part two total: {totalPartTwo}");
    }

    private int GetDigit(string line, bool first)
    {
        var skip = first ? 0 : line.Length - 1;
        while (first ? (skip <= line.Length - 1) : (skip >= 0))
        {
            var firstDigit = line.Skip(skip).First();
            if (char.IsDigit(firstDigit))
            {
                return int.Parse(firstDigit.ToString());
            }

            var wordThree = CheckIfNumberWord(line, skip, 3);
            if (wordThree is not null)
            {
                return (int)wordThree;
            }

            var wordFour = CheckIfNumberWord(line, skip, 4);
            if (wordFour is not null)
            {
                return (int)wordFour;
            }

            var wordFive = CheckIfNumberWord(line, skip, 5);
            if (wordFive is not null)
            {
                return (int)wordFive;
            }

            skip += first ? 1 : -1;
            continue;
        }
        throw new Exception("No digit found");
    }

    private int? CheckIfNumberWord(string line, int startIndex, int length)
    {
        var substring = GetSubstring(line, startIndex, length);
        return (substring is not null && _numWords.TryGetValue(substring, out int value)) ? value : null;
    }

    private static string? GetSubstring(string line, int startIndex, int length)
    {
        return (startIndex <= (line.Length - length)) ? line.Substring(startIndex, length) : null;
    }
}