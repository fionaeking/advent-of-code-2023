namespace AdventOfCode.Days6to10;

public class Day10(string inputFilename) : IDay
{
    private readonly char[][] _input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();

    private readonly Dictionary<char, List<Direction>> _charToDirectionMap = new Dictionary<char, List<Direction>>()
    {
        { '|', new List<Direction>() {Direction.North, Direction.South } },
        { '-', new List<Direction>() {Direction.East, Direction.West } },
        { 'L', new List<Direction>() {Direction.North, Direction.East } },
        { 'J', new List<Direction>() {Direction.North, Direction.West } },
        { '7', new List<Direction>() {Direction.South, Direction.West } },
        { 'F', new List<Direction>() {Direction.South, Direction.East } }
    };
    private readonly Dictionary<Direction, Direction> _oppositeMap = new Dictionary<Direction, Direction>()
    {
        { Direction.North, Direction.South },
        { Direction.East, Direction.West },
        { Direction.South, Direction.North },
        { Direction.West, Direction.East },
    };

    public void Part1()
    {
        var pathCount = 0;
        var pointsVisited = new Dictionary<int, Tuple<int, int>>();
        var start = GetStart(_input);
        pointsVisited.Add(pathCount, start);

        // Check surrounding coordinates
        var surroundingCoords = new Dictionary<Direction, Tuple<int, int>>()
        {
            {Direction.West, new(start.Item1 - 1, start.Item2) },
            {Direction.North, new(start.Item1 + 1, start.Item2) },
            {Direction.East, new(start.Item1, start.Item2 + 1) },
            {Direction.South, new(start.Item1, start.Item2 - 1) },
        };
        var coords = surroundingCoords.First(i => _input[i.Value.Item2][i.Value.Item1] == '|' || _input[i.Value.Item2][i.Value.Item1] == '-');
        var currPoint = coords.Value;
        var currDirection = coords.Key;

        pathCount++;
        pointsVisited.Add(pathCount, new Tuple<int, int>(currPoint.Item1, currPoint.Item2));

        var currChar = _input[currPoint.Item2][currPoint.Item1];

        while (currChar != 'S')
        {
            currDirection = GetNextDirection(_input, currChar, currDirection);
            currPoint = GetNextPoint(_input, currPoint, currDirection);
            pathCount++;
            pointsVisited.Add(pathCount, new Tuple<int, int>(currPoint.Item1, currPoint.Item2));
            currChar = _input[currPoint.Item2][currPoint.Item1];
        }

        var halfway = pathCount / 2;

        Console.WriteLine(halfway);
    }

    private Direction GetNextDirection(char[][] input, char currChar, Direction currDirection)
    {
        var direction = _charToDirectionMap[currChar];
        return direction.Single(d => d != _oppositeMap[currDirection]);
    }

    private Tuple<int, int> GetNextPoint(char[][] input, Tuple<int, int> currPoint, Direction currDirection)
    {
        switch (currDirection)
        {
            case Direction.North:
                return new Tuple<int, int>(currPoint.Item1, currPoint.Item2 - 1);
            case Direction.South:
                return new Tuple<int, int>(currPoint.Item1, currPoint.Item2 + 1);
            case Direction.East:
                return new Tuple<int, int>(currPoint.Item1 + 1, currPoint.Item2);
            case Direction.West:
                return new Tuple<int, int>(currPoint.Item1 - 1, currPoint.Item2);
            default:
                throw new NotImplementedException("Unrecognised direction");
        }

    }

    private Tuple<int, int> GetStart(char[][] input)
    {
        for (var i = 0; i < _input.Length; i++)
        {
            var index = Array.IndexOf(_input[i], 'S');
            if (index != -1)
            {
                return new Tuple<int, int>(index, i);
            }

        }
        throw new Exception("Index not found");
    }

    public void Part2()
    {

    }
}

public enum Direction
{
    North, South, East, West
}
