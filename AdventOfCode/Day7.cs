namespace AdventOfCode;

public class Day7(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        Console.WriteLine(GetTotalWinnings());
    }

    public void Part2()
    {
        Console.WriteLine(GetTotalWinnings(true));
    }

    private int GetTotalWinnings(bool isJackWildcard = false)
    {
        return _input.Select(x => x.Split(" "))
            .Select(x => (x[0].ToCharArray(), int.Parse(x[1])))
            .OrderBy(x => GetPokerType(x.Item1, isJackWildcard))
            .ThenBy(x => ConvertToInt(x.Item1[0], isJackWildcard))
            .ThenBy(x => ConvertToInt(x.Item1[1], isJackWildcard))
            .ThenBy(x => ConvertToInt(x.Item1[2], isJackWildcard))
            .ThenBy(x => ConvertToInt(x.Item1[3], isJackWildcard))
            .ThenBy(x => ConvertToInt(x.Item1[4], isJackWildcard))
            .Select((item, rank) => item.Item2 * (rank + 1)).Sum();
    }

    private static int ConvertToInt(char v, bool isJackWildcard = false)
    {
        if (v == 'T') return 10;
        if (v == 'J') return isJackWildcard ? 1 : 11;
        if (v == 'Q') return 12;
        if (v == 'K') return 13;
        if (v == 'A') return 14;
        return int.Parse(v.ToString());
    }

    private static int GetPokerType(char[] item1, bool isJackWildcard = false)
    {
        var grouped = item1.GroupBy(x => x).Select((p) => new { Grp = p.Key, Cnt = p.Count() });

        var maxGroup = grouped.OrderByDescending(x => x.Cnt).First();

        var numJacks = item1.Where(x => x == 'J').Count();
        int maxCount;
        if (isJackWildcard && numJacks > 0 && numJacks < 5)
        {
            grouped = grouped.Where(x => x.Grp != 'J').OrderByDescending(x => x.Cnt);
            maxGroup = grouped.First();
            maxCount = maxGroup.Cnt + numJacks;
        }
        else
        {
            maxCount = maxGroup.Cnt;
        }

        if (maxCount == 5) return 6;
        else if (maxCount == 4) return 5;
        else if (maxCount == 3) return (grouped.Any(x => x.Cnt == 2 && x.Grp != maxGroup.Grp)) ? 4 : 3;
        else if (maxCount == 2) return (grouped.Where(x => x.Cnt == 2).Count() == 2) ? 2 : 1;
        return 0;
    }
}