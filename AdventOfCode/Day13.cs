namespace AdventOfCode;

public class Day13(string inputFilename) : IDay
{
    private readonly IEnumerable<string[]> _input = File.ReadAllText(inputFilename).Split($"{Environment.NewLine}{Environment.NewLine}").Select(x => x.Split(Environment.NewLine));

    public void Part1()
    {
        Console.WriteLine(_input.Select(GetSum).Sum());
    }

    private int GetSum(string[] section)
    {
        // Horizontal
        var symRowsFromStart = GetNumSymmetricalRows(0, GetAllIndexes(section, section.First()), section);
        if (symRowsFromStart != 0) return 100 * symRowsFromStart;

        var symRowsFromEnd = GetNumSymmetricalRows(section.Length - 1, GetAllIndexes(section, section.Last()), section);
        if (symRowsFromEnd != 0) return 100 * symRowsFromEnd;


        // Vertical
        var vert = TransposeArray(section);

        var symColsFromStart = GetNumSymmetricalRows(0, GetAllIndexes(vert, vert.First()), vert);
        if (symColsFromStart != 0) return symColsFromStart;

        var symColsFromEnd = GetNumSymmetricalRows(vert.Length - 1, GetAllIndexes(vert, vert.Last()), vert);
        if (symColsFromEnd != 0) return symColsFromEnd;

        throw new Exception("Couldn't find symmetry line");
    }

    private static IEnumerable<int> GetAllIndexes(string[] section, string stringToMatch)
    {
        return Enumerable.Range(0, section.Length).Where(i => section[i] == stringToMatch);
    }

    private static string[] TransposeArray(string[] section)
    {
        var v = new List<string>();
        for (var i = 0; i < section.First().Count(); i++)
        {
            v.Add(string.Join("", section.Select(x => x[i])));
        }
        return [.. v];
    }

    private int GetNumSymmetricalRows(int fixedVal, IEnumerable<int> xList, string[] vert)
    {
        foreach (var x in xList)
        {
            if (IsSymmetrical(fixedVal, x, vert)) return (1 + fixedVal + x) / 2;
        }
        return 0;
    }

    private static bool IsSymmetrical(int x, int y, string[] section)
    {
        if (x == y || (x + y) % 2 == 0) return false;

        var increment = 0;
        var start = Math.Min(x, y);
        var end = Math.Max(x, y);

        while (start + increment <= end - increment)
        {
            if (section[start + increment] != section[end - increment]) return false;
            increment++;
        }
        return true;
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}

