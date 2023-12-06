namespace AdventOfCode;

public class Day5 : IDay
{
    private readonly IEnumerable<long> _seeds;
    private readonly IEnumerable<IEnumerable<MapObject>> _mapObjects;
    public Day5(string inputFilename)
    {
        var input = File.ReadAllText(inputFilename).Split(Environment.NewLine + Environment.NewLine).Select(x => x.Split(Environment.NewLine)).ToList();
        _seeds = input.First()[0].Replace("seeds: ", "").Split(" ").Select(long.Parse);
        _mapObjects = input.Skip(1).Select(x => x.Skip(1)).Select(x => x.Select(y => new MapObject(y)));
    }

    public void Part1()
    {
        var source = _seeds;
        foreach (var m in _mapObjects)
        {
            var newSource = new List<long>();
            foreach (var s in source)
            {
                var match = m.FirstOrDefault(n => s <= n.MaxSource && s >= n.MinSource);
                if (match == null)
                {
                    newSource.Add(s);
                }
                else
                {
                    var diff = s - match.MinSource;
                    var newValue = match.MinDestination + diff;
                    newSource.Add(newValue);
                }

            }
            source = newSource;
        }
        Console.WriteLine(source.Min());
    }

    public void Part2()
    {
        var currTolerance = 100000;
        long? previousTolerance = null;

        long? currSeed = null;
        long? minLocation = null;

        while (currTolerance > 0)
        {
            var seeds = GetSeeds(currTolerance, _seeds, currSeed, previousTolerance);
            var min = GetMin(seeds, _mapObjects);
            currSeed = min.Key;
            minLocation = min.Value;
            previousTolerance = currTolerance;
            currTolerance /= 2;
        }
        Console.WriteLine(minLocation);
    }

    private static Dictionary<long, long> GetSeeds(int tolerance, IEnumerable<long> seedsInput, long? prevNum, long? previousTolerance)
    {
        var seedsQueue = new Queue<long>(seedsInput);

        var seeds = new HashSet<long>();
        while (seedsQueue.Count > 0)
        {
            var startSeed = seedsQueue.Dequeue();
            var seedRange = seedsQueue.Dequeue();

            if (prevNum is not null && previousTolerance is not null)
            {
                var prevNumMin = (long)prevNum - (long)previousTolerance;
                var prevNumMax = (long)prevNum + (long)previousTolerance;

                var start = Math.Max(startSeed, prevNumMin);
                var end = Math.Min(start + seedRange, prevNumMax);

                if (prevNum >= start && prevNum <= end)
                {
                    while (start <= end)
                    {
                        seeds.Add(start);
                        start += tolerance;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= seedRange; i += tolerance)
                {
                    seeds.Add(startSeed + i);
                }
            }
        }
        var source = new Dictionary<long, long>();

        foreach (var s in seeds)
        {
            source.Add(s, s);
        }
        return source;
    }

    private static KeyValuePair<long, long> GetMin(Dictionary<long, long> source, IEnumerable<IEnumerable<MapObject>> input)
    {
        foreach (var m in input)
        {
            var newSource = new Dictionary<long, long>();
            foreach (var s in source)
            {
                var match = m.FirstOrDefault(n => s.Value <= n.MaxSource && s.Value >= n.MinSource);
                if (match == null)
                {
                    newSource.Add(s.Key, s.Value);
                }
                else
                {
                    var diff = s.Value - match.MinSource;
                    newSource.Add(s.Key, match.MinDestination + diff);
                }
            }
            source = newSource;
        }
        return source.First(x => x.Value == source.Values.Min());
    }
}

public class MapObject
{
    public long MinDestination;
    public long MinSource;
    public long MaxSource;

    public MapObject(string line)
    {
        var splitLine = line.Split(" ");
        MinDestination = long.Parse(splitLine[0]);
        MinSource = long.Parse(splitLine[1]);
        var range = long.Parse(splitLine[2]) - 1;
        MaxSource = MinSource + range;
    }
}