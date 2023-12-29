using System.Drawing;

namespace AdventOfCode.Days16to20;

internal class Day18(string inputFilename) : IDay
{
    private readonly IEnumerable<string[]> _input = File.ReadAllLines(inputFilename).Select(x => x.Split(" "));

    public void Part1()
    {
        var input = _input.Select(x => new DigPlan()
        {
            Direction = char.Parse(x[0]),
            Metres = int.Parse(x[1])
        });

        var count = GetCubicMetresOfLava(input);
        Console.WriteLine(count);
    }

    private long GetCubicMetresOfLava(IEnumerable<DigPlan> input)
    {
        var visitedPoints = new List<Point>() { new() { X = 0, Y = 0 } };
        foreach (var digPlan in input)
        {
            var lastX = visitedPoints.Last().X;
            var lastY = visitedPoints.Last().Y;
            switch (digPlan.Direction)
            {
                case 'R':
                    visitedPoints.AddRange(Enumerable.Range(1, digPlan.Metres).Select(i => new Point() { X = lastX + i, Y = lastY }));
                    break;
                case 'U':
                    visitedPoints.AddRange(Enumerable.Range(1, digPlan.Metres).Select(i => new Point() { X = lastX, Y = lastY - i }));
                    break;
                case 'L':
                    visitedPoints.AddRange(Enumerable.Range(1, digPlan.Metres).Select(i => new Point() { X = lastX - i, Y = lastY }));
                    break;
                case 'D':
                    visitedPoints.AddRange(Enumerable.Range(1, digPlan.Metres).Select(i => new Point() { X = lastX, Y = lastY + i }));
                    break;
                default:
                    throw new NotImplementedException($"Unexpected direction {digPlan.Direction}");
            }
        }

        var minX = visitedPoints.Min(p => p.X);
        var maxX = visitedPoints.Max(p => p.X);
        var minY = visitedPoints.Min(p => p.Y);
        var maxY = visitedPoints.Max(p => p.Y);

        return Enumerable.Range(minY, maxY - minY + 1).AsParallel().Select(y => Enumerable.Range(minX, maxX - minX + 1).AsParallel().Where(x => true == IsPointOnOrInPolygon(new Point() { X = x, Y = y }, [.. visitedPoints], maxX, minX, maxY, minY)).Count()).Sum();
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