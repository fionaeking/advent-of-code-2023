namespace AdventOfCode;

public class Day14(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        var transposedInput = TransposeArray(_input);
        var sum = 0;
        foreach (var line in transposedInput)
        {
            var orderedLine = string.Join("#", line.Split('#').Select(x => string.Join("", x.OrderByDescending(x => x == 'O')))).ToCharArray();
            sum += GetAllIndexes(orderedLine, 'O').Select(x => orderedLine.Length - x).Sum();
        }
        Console.WriteLine(sum);
    }

    private static IEnumerable<int> GetAllIndexes(char[] line, char charToMatch)
    {
        return Enumerable.Range(0, line.Length).Where(i => line[i] == charToMatch);
    }

    private static string[] TransposeArray(string[] section)
    {
        var v = new List<string>();
        for (var i = 0; i < section.First().Length; i++)
        {
            v.Add(string.Join("", section.Select(x => x[i])));
        }
        return [.. v];
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}

