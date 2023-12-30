using System.Text.RegularExpressions;

namespace AdventOfCode.Days16to20;

public class Day19(string inputFilename) : IDay
{
    private readonly string[][] _input = File.ReadAllText(inputFilename).Split($"{Environment.NewLine}{Environment.NewLine}").Select(x => x.Split(Environment.NewLine)).ToArray();

    public void Part1()
    {
        var workflows = _input[0].Select(workflow => new Workflow()
        {
            Name = workflow.Split('{')[0],
            Conditions = GetMatches(workflow),
            Default = RemoveCurlyBrackets(workflow.Split('{')[1]).Split(",").Last()
        });
        var parts = _input[1].Select(x => new PartRatings()
        {
            Parts = RemoveCurlyBrackets(x).Split(',').Select(y => new Part()
            {
                Category = (Category)(y.Split('=')[0][0]),
                Value = int.Parse(y.Split('=')[1])
            })
        });

        var sum = 0;
        foreach (var part in parts)
        {
            var currName = "in";
            while (currName != "A" && currName != "R")
            {
                var currWorkflow = workflows.First(w => w.Name == currName);
                currName = GetNextName(part, currWorkflow);
            }
            if (currName == "A")
            {
                sum += part.Parts.Select(p => p.Value).Sum();
            }
        }
        Console.WriteLine(sum);
    }

    private static string GetNextName(PartRatings part, Workflow currWorkflow)
    {
        foreach (var condition in currWorkflow.Conditions)
        {
            if (IsConditionSatisfied(part, condition))
            {
                return condition.NextName;
            }
        }
        return currWorkflow.Default;
    }

    private static bool IsConditionSatisfied(PartRatings part, WorkflowCondition condition)
    {
        var partValue = part.Parts.First(p => p.Category == condition.Category).Value;
        return (condition.GreaterThan == true) ? partValue > condition.Value : partValue < condition.Value;
    }

    private static string RemoveCurlyBrackets(string input)
    {
        return input.Replace("}", "").Replace("{", "");
    }

    private static List<WorkflowCondition> GetMatches(string line)
    {
        var regex = new Regex(@"(?<category>[xmas])(?<operator>.)(?<val>\d+):(?<nextName>[a-zA-Z]+)");
        var matches = regex.Matches(line);
        return matches.Select(x => new WorkflowCondition()
        {
            Category = (Category)(x.Groups["category"].Value[0]),
            GreaterThan = x.Groups["operator"].Value == ">",
            Value = int.Parse(x.Groups["val"].Value),
            NextName = x.Groups["nextName"].Value
        }
        ).ToList();
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}

public class PartRatings
{
    public IEnumerable<Part> Parts { get; set; }
}

public class Part
{
    public Category Category { get; set; }
    public int Value { get; set; }
}

public class Workflow
{
    public string Name { get; set; }
    public List<WorkflowCondition> Conditions { get; set; }
    public string Default { get; set; }

}

public class WorkflowCondition
{
    public Category Category { get; set; }
    public bool GreaterThan { get; set; }
    public int Value { get; set; }
    public string NextName { get; set; }
}

public enum Category
{
    x = 'x',
    m = 'm',
    a = 'a',
    s = 's'
}
