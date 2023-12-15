
namespace AdventOfCode;

public class Day9(string inputFilename) : IDay
{
    private readonly IEnumerable<long[]> _input = File.ReadAllLines(inputFilename).Select(x => x.Split(" ").Select(x => long.Parse(x)).ToArray());

    public void Part1()
    {
        var histSum = _input.Select(line => GetSequences(line).ToList().Select(x => x.Last()).Sum()).Sum();
        Console.WriteLine(histSum);
    }

    private List<long[]> GetSequences(long[] line)
    {
        var newLines = new List<long[]>();
        var diffs = line;
        while (!diffs.All(x => x == 0))
        {
            newLines.Add(diffs);
            diffs = GetDiffs(diffs);
        }
        return newLines;
    }

    private static long[] GetDiffs(long[] line)
    {
        var diffs = new List<long>();
        for (var i = 0; i < line.Length - 1; i++)
        {
            diffs.Add(line[i + 1] - line[i]);
        }
        return [.. diffs];
    }

    public void Part2()
    {
        long histSum = 0;
        foreach (var line in _input)
        {
            var sequences = GetSequences(line);

            sequences.Reverse();
            var start = sequences.First()[0];
            foreach (var a in sequences.Skip(1).Select(x => x[0]))
            {
                start = a - start;
            }
            histSum += start;
        }
        Console.WriteLine(histSum);
    }
}
