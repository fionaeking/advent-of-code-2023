using AdventOfCode.Days6to10;

namespace AdventOfCode.Days16to20;

public class Day17(string inputFilename) : IDay
{
    private readonly int[][] _input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

    public void Part1()
    {
        var xLength = _input[0].Length;
        var yLength = _input.Length;

        IEnumerable<CityBlock> currPoints = new List<CityBlock>
        {
            new() { X = xLength - 1, Y = yLength - 1, CurrDirection = Direction.West, TotalHeatLoss = _input[yLength - 1][xLength - 1] },
            new() {  X = xLength - 1, Y = yLength - 1, CurrDirection = Direction.North, TotalHeatLoss = _input[yLength - 1][xLength - 1] }
        };

        // quickly find a min path value so we can cut down more quickly later
        // 1/5 of the total sum of all the cells possibly seems reasonable (small enough that filter on it
        // will remove entries that are getting too long, but large enough that it won't obscure the real max value
        var minHeatLoss = 1294; // _input.SelectMany(x => x).Sum() * 1 / 5;
        //Console.WriteLine($"Debug: {minHeatLoss}");

        var numiterations = 0;
        while (currPoints.Any())
        {

            numiterations++;
            currPoints = currPoints.SelectMany(point => GetNewPoints(point, xLength, yLength, minHeatLoss))
                .OrderBy(x => x.X + x.Y).Take(500)
                .GroupBy(x => new { x.X, x.Y, x.CurrDirection, x.DirectionCount })
                .OrderBy(x => x.Min(y => y.TotalHeatLoss))
                .Select(x => x.First());


            // What ones have finished?
            if (numiterations >= xLength + yLength - 20)
            {
                var finished = currPoints.Where(IsFinished); //, xLength, yLength));
                if (finished.Any())
                {
                    Console.WriteLine("Finished");
                    minHeatLoss = Math.Min(minHeatLoss, finished.Select(x => x.TotalHeatLoss - _input[0][0]).Min());
                    currPoints = currPoints.Except(finished);  //newCurrPoints = newCurrPoints.Where(point => !IsFinished(point)).ToList(); //, xLength, yLength)).ToList();
                }
            }

            // Console.WriteLine(currPoints.Count());

            // if (numiterations % 30 == 0) //currPoints.Count() > 5000)
            //{
            // var min = currPoints.OrderBy(z => z.X + z.Y).ThenBy(z => z.TotalHeatLoss).First().TotalHeatLoss;

            //var maxDistance = xLength - 1 + yLength - 1;

            // currPoints = currPoints.Where(x => x.TotalHeatLoss < min + 30 && (maxDistance - x.X - x.Y) * 4 >= numiterations);
            //}


            //.OrderBy(z => z.X + z.Y) // Math.Sqrt(Math.Pow(z.X, 2) + Math.Pow(z.Y, 2)))
            //.ThenBy(x => x.TotalHeatLoss);

            // var m = currPoints.OrderBy(z => z.X + z.Y).ThenBy(z => z.TotalHeatLoss).First();
            // var min = m.TotalHeatLoss;
            //  currPoints = currPoints.Where(x => x.TotalHeatLoss < min + 50);
        }
        //
        //Console.WriteLine(min + " " + minHeatLoss);

        // Console.WriteLine("\n");
        // for (int i = 0; i < yLength; i++)
        // {
        //     var stringtowrite = "";
        //     for (int j = 0; j < xLength; j++)
        //     {
        //         if (m.visitedPoints.Any(p => p.X == j && p.Y == i))
        //         {
        //             stringtowrite += "#";
        //         }
        //         else
        //         {
        //             stringtowrite += $"{_input[i][j]}";
        //         }
        //     }
        //     Console.WriteLine(stringtowrite);
        // }

        //if (numiterations > 140)
        //     newCurrPoints = newCurrPoints.OrderBy(z => z.X + z.Y).ThenBy(z => z.TotalHeatLoss).Take(2000); // numiterations > 100 ? 200000 : 1000); // 00);

        // 1299 too high. Also not 1294 (which is 1299 - initial start of 5)

        //currPoints = newCurrPoints.ToList();


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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.X - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X - 1,
                        Y = point.Y,
                        CurrDirection = Direction.West,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X - 1],
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.X - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X - 1,
                        Y = point.Y,
                        CurrDirection = Direction.West,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y][point.X - 1],
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.Y - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y - 1,
                        CurrDirection = Direction.North,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y - 1][point.X],
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                if (point.Y - 1 >= 0)
                    newCurrPoints.Add(new CityBlock
                    {
                        X = point.X,
                        Y = point.Y - 1,
                        CurrDirection = Direction.North,
                        DirectionCount = 1,
                        TotalHeatLoss = point.TotalHeatLoss + _input[point.Y - 1][point.X],
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
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
                        visitedPoints = point.visitedPoints.Concat(new[] { new CurrPoint() { X = point.X, Y = point.Y } })
                    });
                }
                break;
        }

        return newCurrPoints
            // Assume visiting the same point twice won't minimise heat loss
            .Where(point => !IsLoop(point) && point.TotalHeatLoss - _input[0][0] < minHeatLoss);
        // Ignore any where heat loss has exceeded current min value
    }

    private static bool IsFinished(CityBlock point) //, int xLength, int yLength)
    {
        return point.X == 0 && point.Y == 0; // xLength - 1 && point.Y == yLength - 1;
    }

    private static bool IsLoop(CityBlock obj)
    {
        return obj.visitedPoints.SkipLast(1).Any(o => (o.X == obj.X && o.Y == obj.Y) || (Math.Abs(o.X - obj.X) == 1 && o.Y == obj.Y) || (Math.Abs(o.Y - obj.Y) == 1 && o.X == obj.X)); // || obj.visitedPoints.SkipLast(1).Any(o => );
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}

public class CityBlock : CurrPoint
{
    public int DirectionCount = 0;
    public int TotalHeatLoss = 0;
    public bool IsFinished = false;
    public IEnumerable<CurrPoint> visitedPoints = new List<CurrPoint>();
}