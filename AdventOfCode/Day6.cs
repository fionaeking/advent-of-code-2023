namespace AdventOfCode;

public class Day6(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        var parsedInput = _input
            .Select(x => x.Split(":")[1])
            .Select(x => x.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray())
            .ToArray();

        var raceTimes = parsedInput[0];
        var winDistances = parsedInput[1];

        var partOneAnswer = raceTimes.Zip(winDistances, (raceTime, winDistance) => GetNumWins(raceTime, winDistance)).Aggregate(1, (a, b) => a * b);
        Console.WriteLine(partOneAnswer);
    }

    public void Part2()
    {
        var parsedInput = _input.Select(x => long.Parse(x.Split(":")[1].Replace(" ", ""))).ToArray();
        var numWins = GetNumWins(parsedInput[0], parsedInput[1]);
        Console.WriteLine(numWins);
    }

    private static int GetNumWins(long raceTime, long winDistance)
    {
        var numWins = 0;
        // No point excuting while loop for holdTime = 0 and holdTime = raceTime as won't move anywhere!
        var holdTime = 1;
        while (holdTime < raceTime)
        {
            var moveTime = raceTime - holdTime;
            var myDistance = moveTime * holdTime;
            if (myDistance > winDistance)
            {
                numWins++;
            }
            holdTime++;
        }
        return numWins;
    }

}