using System.Drawing;

namespace AdventOfCode.Days16to20;

internal class Day18(string inputFilename) : IDay
{
    private readonly IEnumerable<DigPlan> _input = File.ReadAllLines(inputFilename).Select(x => x.Split(" ")).Select(x => new DigPlan()
    {
        Direction = char.Parse(x[0]),
        Metres = int.Parse(x[1])
    });

    public void Part1()
    {
        var visitedPoints = new List<Point>() { new() { X = 0, Y = 0 } };
        foreach (var digPlan in _input)
        {
            switch (digPlan.Direction)
            {
                case 'R':
                    for (var i = 1; i <= digPlan.Metres; i++)
                    {
                        visitedPoints.Add(new() { X = visitedPoints.Last().X + 1, Y = visitedPoints.Last().Y });
                    }
                    break;
                case 'U':
                    for (var i = 1; i <= digPlan.Metres; i++)
                    {
                        visitedPoints.Add(new() { X = visitedPoints.Last().X, Y = visitedPoints.Last().Y - 1 });
                    }
                    break;
                case 'L':
                    for (var i = 1; i <= digPlan.Metres; i++)
                    {
                        visitedPoints.Add(new() { X = visitedPoints.Last().X - 1, Y = visitedPoints.Last().Y });
                    }
                    break;
                case 'D':
                    for (var i = 1; i <= digPlan.Metres; i++)
                    {
                        visitedPoints.Add(new() { X = visitedPoints.Last().X, Y = visitedPoints.Last().Y + 1 });
                    }
                    break;
                default:
                    throw new NotImplementedException($"Unexpected direction {digPlan.Direction}");
            }
        }

        var count = 0;

        var minX = visitedPoints.Min(p => p.X);
        var maxX = visitedPoints.Max(p => p.X);
        var minY = visitedPoints.Min(p => p.Y);
        var maxY = visitedPoints.Max(p => p.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (IsPointOnOrInPolygon(new Point() { X = x, Y = y }, [.. visitedPoints], maxX, minX, maxY, minY))
                {
                    count++;
                }
            }
        }
        Console.WriteLine(count);
    }


    public static bool IsPointOnOrInPolygon(Point point, Point[] polygon, int maxX, int minX, int maxY, int minY)
    {
        if (polygon.Any(p => p.X == point.X && p.Y == point.Y))
        {
            return true;
        }
        if (point.X < minX || point.X > maxX || point.Y < minY || point.Y > maxY)
        {
            return false;
        }

        var inPolygon = false;
        for (var i = 0; i < polygon.Length; i++)
        {
            var j = (i + 1) % polygon.Length;
            if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y) && point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
            {
                inPolygon = !inPolygon;
            }
        }
        return inPolygon;
    }


    public void Part2()
    {
        throw new NotImplementedException();
    }
}

public class DigPlan
{
    public char Direction { get; set; }
    public int Metres { get; set; }
}