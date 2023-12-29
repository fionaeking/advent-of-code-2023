namespace AdventOfCode.Days1to5;

public class Day3(string _inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(_inputFilename);
    private readonly char _dotChar = '.';
    private readonly char _gearChar = '*';

    public void Part1()
    {
        var sum = 0;
        for (var currY = 0; currY < _input.Length; currY++)
        {
            var currLine = _input[currY];
            var currInput = currLine.SkipWhile(x => !char.IsDigit(x));
            var currX = currLine.Length - currInput.Count();
            while (currX <= currLine.Length)
            {
                var nextNumChars = currInput.TakeWhile(x => char.IsDigit(x));
                if (!nextNumChars.Any())
                {
                    break;
                }

                var currNum = int.Parse(string.Join("", nextNumChars));
                var currNumLength = nextNumChars.Count();

                var down = currY + 1 < _input.Length && _input[currY + 1].Skip(currX - 1).Take(currX == 0 || currX == _input[0].Length - 1 ? currNumLength + 1 : currNumLength + 2).Any(x => x != _dotChar);
                var up = currY - 1 >= 0 && _input[currY - 1].Skip(currX - 1).Take(currX == 0 || currX == _input[0].Length - 1 ? currNumLength + 1 : currNumLength + 2).Any(x => x != _dotChar);
                var left = currX - 1 >= 0 && currLine.Skip(currX - 1).First() != _dotChar;
                var right = currX + currNumLength < currLine.Length && currLine.Skip(currX + currNumLength).First() != _dotChar;

                if (down || up || left || right)
                {
                    sum += currNum;
                }

                currInput = currInput.Skip(nextNumChars.Count()).SkipWhile(x => !char.IsDigit(x));
                currX = currLine.Length - currInput.Count();
            }
        }
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var dict = new Dictionary<(int, int), HashSet<int>>();

        for (var currY = 0; currY < _input.Length; currY++)
        {
            var currLine = _input[currY];
            var currInput = currLine.SkipWhile(x => !char.IsDigit(x));
            var currX = currLine.Length - currInput.Count();
            while (currX < currLine.Length)
            {
                var nextNumChars = currInput.TakeWhile(x => char.IsDigit(x));
                if (!nextNumChars.Any())
                {
                    break;
                }

                var currNum = int.Parse(string.Join("", nextNumChars));
                var currNumLength = nextNumChars.Count();

                // DOWN
                if (currY + 1 < _input.Length)
                {
                    var startIndex = currX > 0 ? currX - 1 : currX;
                    var maxIndex = startIndex + currNumLength + 1 == _input[currY + 1].Length ? startIndex + currNumLength : startIndex + currNumLength + 1;

                    for (var i = startIndex; i <= maxIndex; i++)
                    {
                        ProcessChar(i, currY + 1, dict, currNum, _input);
                    }
                }

                // UP
                if (currY - 1 >= 0)
                {
                    var startIndex = currX > 0 ? currX - 1 : currX;
                    var maxIndex = startIndex + currNumLength + 1 == _input[currY - 1].Length ? startIndex + currNumLength : startIndex + currNumLength + 1;

                    for (var i = startIndex; i <= maxIndex; i++)
                    {
                        ProcessChar(i, currY - 1, dict, currNum, _input);
                    }
                }

                // LEFT
                if (currX - 1 >= 0)
                {
                    ProcessChar(currX - 1, currY, dict, currNum, _input);
                }

                // RIGHT
                if (currX + currNumLength < currLine.Length)
                {
                    ProcessChar(currX + currNumLength, currY, dict, currNum, _input);
                }

                currInput = currInput.Skip(nextNumChars.Count()).SkipWhile(x => !char.IsDigit(x));
                currX = currLine.Length - currInput.Count();
            }
        }

        var sumGearRatios = dict.Where(x => x.Value.Count == 2).Select(x => x.Value.Aggregate(1, (a, b) => a * b)).Sum();
        Console.WriteLine(sumGearRatios);
    }

    private void ProcessChar(int x, int y, Dictionary<(int, int), HashSet<int>> dict, int valToAdd, string[] _input)
    {
        if (_input[y][x] == _gearChar)
        {
            if (dict.ContainsKey((x, y)))
            {
                dict[(x, y)].Add(valToAdd);
            }
            else
            {
                dict.Add((x, y), [valToAdd]);
            }
        }
    }
}