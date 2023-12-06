namespace AdventOfCode;

public class Day3(string inputFilename) : IDay
{
    private readonly char _dotChar = '.';
    private readonly char _gearChar = '*';

    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename);
        var sum = 0;
        for (var y = 0; y < input.Length; y++)
        {
            var currInput = input[y].SkipWhile(x => !char.IsDigit(x));
            var currX = input[y].Length - currInput.Count();
            while (currX <= input[y].Length)
            {
                var nextNumChars = currInput.TakeWhile(x => char.IsDigit(x));
                if (!nextNumChars.Any())
                {
                    break;
                }

                var currNum = int.Parse(string.Join("", nextNumChars));
                var currNumLength = nextNumChars.Count();

                var down = (y + 1 < input.Length) && input[y + 1].Skip(currX - 1).Take(currX == 0 || currX == input[0].Length - 1 ? currNumLength + 1 : currNumLength + 2).Any(x => x != _dotChar);
                var up = (y - 1 >= 0) && input[y - 1].Skip(currX - 1).Take(currX == 0 || currX == input[0].Length - 1 ? currNumLength + 1 : currNumLength + 2).Any(x => x != _dotChar);
                var left = (currX - 1 >= 0) && input[y].Skip(currX - 1).First() != _dotChar;
                var right = (currX + currNumLength < input[y].Length) && input[y].Skip(currX + currNumLength).First() != _dotChar;

                if (down || up || left || right)
                {
                    sum += currNum;
                }

                currInput = currInput.Skip(nextNumChars.Count()).SkipWhile(x => !char.IsDigit(x));
                currX = input[y].Length - currInput.Count();
            }
        }
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename);
        var dict = new Dictionary<(int, int), HashSet<int>>();

        for (var y = 0; y < input.Length; y++)
        {
            var currInput = input[y].SkipWhile(x => !char.IsDigit(x));
            var currX = input[y].Length - currInput.Count();
            while (currX < input[y].Length)
            {
                var nextNumChars = currInput.TakeWhile(x => char.IsDigit(x));
                if (!nextNumChars.Any())
                {
                    break;
                }

                var currNum = int.Parse(string.Join("", nextNumChars));
                var currNumLength = nextNumChars.Count();

                // DOWN
                if (y + 1 < input.Length)
                {
                    var startIndex = currX > 0 ? currX - 1 : currX;
                    var maxIndex = (startIndex + currNumLength + 1 == input[y + 1].Length) ? startIndex + currNumLength : startIndex + currNumLength + 1;

                    for (var i = startIndex; i <= maxIndex; i++)
                    {
                        ProcessChar(i, y + 1, dict, currNum, input);
                    }
                }

                // UP
                if (y - 1 >= 0)
                {
                    var startIndex = currX > 0 ? currX - 1 : currX;
                    var maxIndex = (startIndex + currNumLength + 1 == input[y - 1].Length) ? startIndex + currNumLength : startIndex + currNumLength + 1;

                    for (var i = startIndex; i <= maxIndex; i++)
                    {
                        ProcessChar(i, y - 1, dict, currNum, input);
                    }
                }

                // LEFT
                if (currX - 1 >= 0)
                {
                    ProcessChar(currX - 1, y, dict, currNum, input);
                }

                // RIGHT
                if (currX + currNumLength < input[y].Length)
                {
                    ProcessChar(currX + currNumLength, y, dict, currNum, input);
                }

                currInput = currInput.Skip(nextNumChars.Count()).SkipWhile(x => !char.IsDigit(x));
                currX = input[y].Length - currInput.Count();
            }
        }

        var sumGearRatios = dict.Where(x => x.Value.Count == 2).Select(x => x.Value.Aggregate(1, (a, b) => a * b)).Sum();
        Console.WriteLine(sumGearRatios);
    }

    private void ProcessChar(int x, int y, Dictionary<(int, int), HashSet<int>> dict, int valToAdd, string[] input)
    {
        if (input[y][x] == _gearChar)
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