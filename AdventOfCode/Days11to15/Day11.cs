namespace AdventOfCode.Days11to15;

public class Day11(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        var galaxyCoords = GetGalaxyCoords();
        var sum = galaxyCoords.SelectMany(x => galaxyCoords, GetMinDistance).Sum() / 2;
        Console.WriteLine(sum);
    }

    private static long GetMinDistance(Tuple<long, long> a1, Tuple<long, long> a2)
    {
        return Math.Abs(a2.Item1 - a1.Item1) + Math.Abs(a2.Item2 - a1.Item2);

    }

    private List<Tuple<long, long>> GetGalaxyCoords(int limit = 2)
    {
        var galaxyCoords = new List<Tuple<long, long>>();
        for (var i = 0; i < _input.Length; i++)
        {
            if (_input[i].Contains('#'))
            {
                for (var j = 0; j < _input[i].Length; j++)
                {
                    if (_input[i][j] == '#')
                    {
                        galaxyCoords.Add(new Tuple<long, long>(j, i));
                    }
                }
            }
        }

        var horizontalIndicesToExpand = new List<long>();
        for (var i = 0; i < _input[0].Length; i++)
        {
            if (!_input.Select(x => x.Skip(i).First()).Contains('#'))
            {
                horizontalIndicesToExpand.Add(i);
            }
        }

        var verticalIndicesToExpand = new List<long>();
        for (var i = 0; i < _input.Length; i++)
        {
            if (!_input[i].Contains('#'))
            {
                verticalIndicesToExpand.Add(i);
            }
        }

        var finalGalaxyCoords = new List<Tuple<long, long>>();
        foreach (var coord in galaxyCoords)
        {
            var a = horizontalIndicesToExpand.Count(x => coord.Item1 > x);
            var b = verticalIndicesToExpand.Count(x => coord.Item2 > x);
            finalGalaxyCoords.Add(new Tuple<long, long>(coord.Item1 + (limit - 1) * a, coord.Item2 + (limit - 1) * b));
        }
        return finalGalaxyCoords;
    }

    public void Part2()
    {
        var galaxyCoords = GetGalaxyCoords(1000000);
        var sum = galaxyCoords.SelectMany(x => galaxyCoords, GetMinDistance).Sum() / 2;
        Console.WriteLine($"Part two: {sum}");
    }
}