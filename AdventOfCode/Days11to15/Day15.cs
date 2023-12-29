namespace AdventOfCode.Days11to15;

public class Day15(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllText(inputFilename).Split(',');

    public void Part1()
    {
        long total = 0;
        foreach (var item in _input)
        {
            long currVal = 0;
            foreach (var value in item)
            {
                currVal = Hash(value, currVal);
            }
            total += currVal;
        }
        Console.WriteLine(total);
    }

    private static long Hash(int num, long currVal)
    {
        currVal += num;
        currVal *= 17;
        currVal %= 256;
        return currVal;
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}

