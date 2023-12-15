namespace AdventOfCode;

public class Day8(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    private readonly string _partOneStart = "AAA";
    private readonly string _partOneEnd = "ZZZ";
    private readonly string _partTwoStart = "A";
    private readonly string _partTwoEnd = "Z";

    private readonly char _leftInstruction = 'L';

    public void Part1()
    {
        var nodes = ReadInNodes();
        var stepCount = GetStepCountForNode(nodes.First(x => x.Key == _partOneStart), ReadInInstructions(), nodes, _partOneEnd);
        Console.WriteLine(stepCount);
    }

    private char[] ReadInInstructions()
    {
        return _input.First().ToCharArray();
    }

    private Dictionary<string, Tuple<string, string>> ReadInNodes()
    {
        return _input.Skip(2).Select(x => (x.Split(" = ")[0], GetDirections(x))).ToDictionary();
    }

    private static long GCD(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return GCD(n2, n1 % n2);
        }
    }

    public void Part2()
    {
        var nodes = ReadInNodes();
        var stepCounts = nodes.Where(x => x.Key.EndsWith(_partTwoStart)).Select(x => GetStepCountForNode(x, ReadInInstructions(), nodes, _partTwoEnd));

        // Get lowest common multiple of stepCounts
        var lcp = stepCounts.Aggregate((x, val) => x * val / GCD(x, val));


        // 1 2 3 6
        // 6 to 12

        // 2 3 4 5
        // 120
        // 120/2 = 60
        // 120/3 = 40
        // 120/4 = 30
        // 120/5 = 24
        // 

        // 6o
        // 

        Console.WriteLine(lcp);

    }

    private long GetStepCountForNode(KeyValuePair<string, Tuple<string, string>> currNode, char[] charArray, Dictionary<string, Tuple<string, string>> nodes, string endCondition)
    {
        var instructions = new Queue<char>(charArray);
        long stepCount = 0;
        while (!currNode.Key.EndsWith(endCondition))
        {
            var currInstruction = instructions.Dequeue();
            currNode = GetCurrNodes(currInstruction, currNode, nodes);
            instructions.Enqueue(currInstruction);
            stepCount++;
        }
        return stepCount;
    }

    private KeyValuePair<string, Tuple<string, string>> GetCurrNodes(char currInstruction, KeyValuePair<string, Tuple<string, string>> currNode, Dictionary<string, Tuple<string, string>> nodes)
    {
        return currInstruction == _leftInstruction
            ? nodes.First(x => x.Key == currNode.Value.Item1)
            : nodes.First(x => x.Key == currNode.Value.Item2);
    }

    private static Tuple<string, string> GetDirections(string line)
    {
        var splitLine = line.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ");
        return new Tuple<string, string>(splitLine[0], splitLine[1]);
    }
}
