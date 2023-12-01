
var input = File.ReadAllLines("PuzzleInput.txt");

//var totalPartOne = 0;
//foreach (var line in input)
//{
//    var firstDigit = line.First(char.IsDigit);
//    var lastDigit = line.Last(char.IsDigit);
//    totalPartOne += int.Parse($"{firstDigit}{lastDigit}");
//}
//Console.WriteLine($"Part one total: {totalPartOne}");
//

var numWordsThree = new Dictionary<string, int>()
{
    { "one", 1 },
    { "two", 2 },
    { "six", 6 },
};

var numWordsFour = new Dictionary<string, int>()
{
    { "four", 4 },
    { "five", 5 },
    { "nine", 9 }
};

var numWordsFive = new Dictionary<string, int>()
{
    { "three", 3 },
    { "seven", 7 },
    { "eight", 8 }
};

var totalPartTwo = 0;
foreach (var line in input)
{
    var firstDigit = GetDigit(line, true);
    var lastDigit = GetDigit(line, false);
    totalPartTwo += int.Parse($"{firstDigit}{lastDigit}");
}
Console.WriteLine($"Part two total: {totalPartTwo}");


int GetDigit(string line, bool first)
{
    var skip = first ? 0 : line.Length - 1;
    var condition = first ? skip <= line.Length - 1 : skip >= 0;
    while (condition)
    {
        var firstDigit = line.Skip(skip).First();
        if (char.IsDigit(firstDigit))
        {
            return int.Parse(firstDigit.ToString());
        }

        if (skip > (line.Length - 3))
        {
            skip += first ? 1 : -1;
            continue;
        }
        var lineThree = line.Substring(skip, 3);
        if (numWordsThree.TryGetValue(lineThree, out int valueThree))
        {
            return valueThree;
        }

        if (skip > (line.Length - 4))
        {
            skip += first ? 1 : -1;
            continue;
        }
        var lineFour = line.Substring(skip, 4);
        if (numWordsFour.TryGetValue(lineFour, out int valueFour))
        {
            return valueFour;
        }

        if (skip > (line.Length - 5))
        {
            skip += first ? 1 : -1;
            continue;
        }
        var lineFive = line.Substring(skip, 5);
        if (numWordsFive.TryGetValue(lineFive, out int valueFive))
        {
            return valueFive;
        }

        skip += first ? 1 : -1;
        continue;
    }
    throw new Exception("No digit found");
}