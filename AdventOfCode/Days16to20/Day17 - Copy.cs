using AdventOfCode.Days6to10;
using System.Drawing;

namespace AdventOfCode.Days16to20;

public class Day17Copy(string inputFilename) : IDay
{
    private readonly int[][] _input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

    public void Part1()
    {
        var currPoints = new List<CityBlock>
        {
            new() { X = 0, Y = 0, CurrDirection = Direction.East },
            new() { X = 0, Y = 0, CurrDirection = Direction.South }
        };

        var xLength = _input[0].Length;
        var yLength = _input.Length;

        // quickly find a min path value so we can cut down more quickly later
        // 1/5 of the total sum of all the cells possibly seems reasonable (small enough that filter on it
        // will remove entries that are getting too long, but large enough that it won't obscure the real max value
        var minHeatLoss = 1304; // _input.SelectMany(x => x).Sum() * 1 / 5;
        Console.WriteLine($"Debug: {minHeatLoss}");

        var numiterations = 0;
        while (currPoints.Count > 0)
        {
            Console.WriteLine($"{currPoints.First().X} {currPoints.First().Y}");

            var newCurrPoints = currPoints.SelectMany(point => GetNewPoints(point, xLength, yLength, minHeatLoss)); //.ToList();

            // What ones have finished?

            var finished = newCurrPoints.Where(x => IsFinished(x, xLength, yLength));
            if (finished.Any())
            {
                minHeatLoss = Math.Min(minHeatLoss, finished.Select(x => x.TotalHeatLoss).Min());
                newCurrPoints = newCurrPoints.Where(point => !IsFinished(point, xLength, yLength)).ToList();
            }

            newCurrPoints = newCurrPoints
                .GroupBy(x => new { x.X, x.Y, x.CurrDirection, x.DirectionCount })
                .Select(x => x.OrderBy(y => y.TotalHeatLoss).First())
                .OrderBy(z => Math.Sqrt(Math.Pow(Math.Abs((xLength - 1) - z.X), 2) + Math.Pow(Math.Abs((yLength - 1) - z.Y), 2)))
                .ThenBy(x => x.TotalHeatLoss)
                .Take(numiterations > 100 ? 200000 : 1000); // 00);

            // 1304 too high

            currPoints = newCurrPoints.ToList();
            numiterations++;
        }

        Console.WriteLine(minHeatLoss);
    }

    private IEnumerable<CityBlock> GetNewPoints(CityBlock point, int xLength, int yLength, int minHeatLoss)
    {
        var newCurrPoints = new List<CityBlock>();
        switch (point.CurrDirection)
        {
            case Direction.North:
                if (point.X + 1 < xLength)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X + 1,
                        Y = point.Y,
                        CurrDirection = Direction.East,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X + 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.X - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X - 1,
                        Y = point.Y,
                        CurrDirection = Direction.West,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X - 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.DirectionCount < 3 && point.Y - 1 >= 0)
                {
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y - 1,
                        CurrDirection = Direction.North,
                        DirectionCount = point.DirectionCount + 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y - 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                }
                break;
            case Direction.South:
                if (point.X + 1 < xLength)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X + 1,
                        Y = point.Y,
                        CurrDirection = Direction.East,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X + 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.X - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X - 1,
                        Y = point.Y,
                        CurrDirection = Direction.West,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X - 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.DirectionCount < 3 && point.Y + 1 < yLength)
                {
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y + 1,
                        CurrDirection = Direction.South,
                        DirectionCount = point.DirectionCount + 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y + 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                }
                break;
            case Direction.East:
                if (point.Y + 1 < yLength)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y + 1,
                        CurrDirection = Direction.South,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y + 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.Y - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y - 1,
                        CurrDirection = Direction.North,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y - 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.DirectionCount < 3 && point.X + 1 < xLength)
                {
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X + 1,
                        Y = point.Y,
                        CurrDirection = Direction.East,
                        DirectionCount = point.DirectionCount + 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X + 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                }
                break;
            case Direction.West:
                if (point.Y + 1 < yLength)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y + 1,
                        CurrDirection = Direction.South,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y + 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.Y - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y - 1,
                        CurrDirection = Direction.North,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y - 1][point.X],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.DirectionCount < 3 && point.X - 1 >= 0)
                {
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X - 1,
                        Y = point.Y,
                        CurrDirection = Direction.West,
                        DirectionCount = point.DirectionCount + 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X - 1],
                        visitedPoints = point.visitedPoints.TakeLast(20).Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                }
                break;
        }

        return newCurrPoints
            // Assume visiting the same point twice won't minimise heat loss
            .Where(point => !IsLoop(point))
            // Ignore any where heat loss has exceeded current min value
            .Where(point => point.TotalHeatLoss < minHeatLoss);
    }

    private static bool IsFinished(CityBlock point, int xLength, int yLength)
    {
        return point.X == xLength - 1 && point.Y == yLength - 1;
    }

    private static bool IsLoop(CityBlock obj)
    {
        return obj.visitedPoints.Any(o => o.X == obj.X && o.Y == obj.Y) || obj.visitedPoints.SkipLast(1).Any(o => (Math.Abs(o.X - obj.X) == 1 && o.Y == obj.Y) || (Math.Abs(o.Y - obj.Y) == 1 && o.X == obj.X));
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}