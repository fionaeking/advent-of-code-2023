namespace AdventOfCode;

public class Day16(string inputFilename) : IDay
{
    private readonly char[][] _input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();

    public void Part1()
    {
        var answer = GetTilesEnergised(0, 0, Direction.East);
        Console.WriteLine(answer);
    }

    private bool IsValidPoint(CurrPoint obj)
    {
        var maxY = _input.Length - 1;
        var maxX = _input[0].Length - 1;
        return obj.X <= maxX && obj.Y <= maxY && obj.X >= 0 && obj.Y >= 0;
    }

    public void Part2()
    {
        // Could be more efficient here by looking at previous results...
        var eastWest = _input.Select((val, i) => Math.Max(GetTilesEnergised(_input[0].Length - 1, i, Direction.West), GetTilesEnergised(0, i, Direction.East))).Max();
        var northSouth = _input[0].Select((val, i) => Math.Max(GetTilesEnergised(i, 0, Direction.South), GetTilesEnergised(i, 0, Direction.North))).Max();
        var max = Math.Max(eastWest, northSouth);
        Console.WriteLine(max);
    }

    private long GetTilesEnergised(int x, int y, Direction dir)
    {
        var currPoints = new List<CurrPoint>
        {
            new() { X = x, Y = y, CurrDirection = dir }
        };

        var visitedCoords = currPoints.Select(x => x).ToList();

        while (currPoints.Count > 0)
        {
            var newCurrPoints = currPoints.SelectMany(GetNewPoints).Where(IsValidPoint).ToList();
            var visitedCoordsLengthBefore = visitedCoords.Count;
            currPoints.Clear();
            foreach (var point in newCurrPoints)
            {
                if (!visitedCoords.Any(existingPoint => existingPoint.X == point.X && existingPoint.Y == point.Y && existingPoint.CurrDirection == point.CurrDirection))
                {
                    visitedCoords.Add(point);
                    currPoints.Add(point);
                }
            }
        }
        return visitedCoords.DistinctBy(point => new { point.X, point.Y }).Count();
    }

    private List<CurrPoint> GetNewPoints(CurrPoint currPoint)
    {
        var newCurrPoints = new List<CurrPoint>();
        switch (_input[currPoint.Y][currPoint.X])
        {
            case '/':
                if (currPoint.CurrDirection == Direction.East)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y - 1, CurrDirection = Direction.North });
                }
                else if (currPoint.CurrDirection == Direction.West)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y + 1, CurrDirection = Direction.South });
                }
                else if (currPoint.CurrDirection == Direction.North)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X + 1, Y = currPoint.Y, CurrDirection = Direction.East });
                }
                else
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X - 1, Y = currPoint.Y, CurrDirection = Direction.West });
                }
                break;
            case '\\':
                if (currPoint.CurrDirection == Direction.West)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y - 1, CurrDirection = Direction.North });
                }
                else if (currPoint.CurrDirection == Direction.East)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y + 1, CurrDirection = Direction.South });
                }
                else if (currPoint.CurrDirection == Direction.North)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X - 1, Y = currPoint.Y, CurrDirection = Direction.West });
                }
                else
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X + 1, Y = currPoint.Y, CurrDirection = Direction.East });
                }
                break;
            case '|':
                if (currPoint.CurrDirection == Direction.East || currPoint.CurrDirection == Direction.West)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y - 1, CurrDirection = Direction.North });
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y + 1, CurrDirection = Direction.South });
                }
                else if (currPoint.CurrDirection == Direction.North)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y - 1, CurrDirection = Direction.North });
                }
                else
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y + 1, CurrDirection = Direction.South });
                }
                break;
            case '-':
                if (currPoint.CurrDirection == Direction.North || currPoint.CurrDirection == Direction.South)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X + 1, Y = currPoint.Y, CurrDirection = Direction.East });
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X - 1, Y = currPoint.Y, CurrDirection = Direction.West });
                }
                else if (currPoint.CurrDirection == Direction.East)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X + 1, Y = currPoint.Y, CurrDirection = Direction.East });
                }
                else
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X - 1, Y = currPoint.Y, CurrDirection = Direction.West });
                }
                break;
            case '.':
                if (currPoint.CurrDirection == Direction.North)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y - 1, CurrDirection = Direction.North });
                }
                else if (currPoint.CurrDirection == Direction.South)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X, Y = currPoint.Y + 1, CurrDirection = Direction.South });
                }
                else if (currPoint.CurrDirection == Direction.East)
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X + 1, Y = currPoint.Y, CurrDirection = Direction.East });
                }
                else
                {
                    newCurrPoints.Add(new CurrPoint() { X = currPoint.X - 1, Y = currPoint.Y, CurrDirection = Direction.West });
                }
                break;
            default:
                throw new Exception($"Unexpected char: {currPoint.CurrDirection}");
        }
        return newCurrPoints;
    }
}

public class CurrPoint
{
    public Direction CurrDirection;
    public int X;
    public int Y;
}