namespace AdventOfCode.Days1to5;

public class Day4(string inputFilename) : IDay
{
    private readonly IEnumerable<(int, int)> _input = File.ReadAllLines(inputFilename).Select(Parse);
    public void Part1()
    {
        var partOne = _input.Where(x => x.Item2 > 0).Select(x => Math.Pow(2, x.Item2 - 1)).Sum();
        Console.WriteLine(partOne);
    }

    public void Part2()
    {
        var scratchcardCount = 0;
        var parsedInput = _input.ToDictionary();

        var cardNumsQueue = new Queue<int>(parsedInput.Keys);

        while (cardNumsQueue.Count != 0)
        {
            scratchcardCount++;
            var currCard = cardNumsQueue.Dequeue();
            var matchCount = parsedInput[currCard];
            if (matchCount != 0)
            {
                for (var i = 1; i <= matchCount; i++)
                {
                    cardNumsQueue.Enqueue(currCard + i);
                }
            }
        }

        Console.WriteLine(scratchcardCount);
    }

    private static (int, int) Parse(string line)
    {
        var splitLine = line.Split(" | ");
        // cardNum only required for part 2
        var cardNum = int.Parse(splitLine[0].Split(": ")[0].Replace("Card ", ""));
        var winningNums = splitLine[0].Split(": ")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
        var myNums = splitLine[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
        return (cardNum, myNums.Where(winningNums.Contains).Count());
    }
}