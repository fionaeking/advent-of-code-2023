
using System.Diagnostics;

namespace AdventOfCode.Days11to15;

public class Day12(string inputFilename) : IDay
{
    private readonly string[] _input = File.ReadAllLines(inputFilename);

    public void Part1()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var splitLine = line.Split(' ');
            var nums = splitLine[1].Split(',').Select(x => int.Parse(x)).ToArray();
            var chars = splitLine[0].ToCharArray();

            var possibleCombinations = new List<char[]>() { chars };

            for (var i = 0; i < chars.Length; i++)
            {

                var currChar = chars[i];
                if (currChar == '?')
                {
                    var newCombinations = new List<char[]>();
                    foreach (var combo in possibleCombinations)
                    {
                        var arrayOne = (char[])combo.Clone();
                        arrayOne[i] = '.';
                        newCombinations.Add(arrayOne);
                        var arrayTwo = (char[])combo.Clone();
                        arrayTwo[i] = '#';
                        newCombinations.Add(arrayTwo);
                    }
                    possibleCombinations = newCombinations;
                }
            }

            // Check which combinations fit the criteria


            sum += possibleCombinations.Count(combo => DoesCombinationFit(nums, combo));

        }
        Console.WriteLine(sum);
    }

    private static bool DoesCombinationFit(int[] nums, char[] charArray)
    {
        var split = string.Join("", charArray).Split('.').Where(x => x.Contains('#')).ToList();

        if (split.Count != nums.Length)
        {
            return false;
        }

        for (var i = 0; i < nums.Length; i++)
        {
            var numToCheck = nums[i];
            var inputToCheck = split[i].Replace(".", "");
            if (numToCheck != inputToCheck.Length)
            {
                return false;
            }
        }
        return true;
    }

    private static bool CheckTwo(int[] nums, char[] charArray)
    {
        var a = charArray.TakeWhile(x => x != '?');
        var nextVal = charArray.Skip(a.Count()).FirstOrDefault();
        var split = string.Join("", a).Split('.').Where(x => x.Contains('#')).ToList();

        for (var j = 0; j < split.Count(); j++)
        {
            if (j > nums.Count() - 1)
            {
                return false;
            }
            var numToCheck = nums[j];
            var inputToCheck = split[j].Replace(".", "");

            if (j == split.Count() - 1 && a.LastOrDefault() == '#')
            {
                // We could be in situation where next char is ?. But check we're not in situation where
                // we have more #s than allowed
                return numToCheck >= inputToCheck.Length;
            }

            if (numToCheck != inputToCheck.Length)
            {
                return false;
            }
        }
        return true;
    }

    private long GetSum(string line)
    {
        long sum = 0;
        var splitLine = line.Split(' ');
        // Apply unfolding
        var unfolded = Enumerable.Repeat((splitLine[0] + '?').ToList(), 5).SelectMany(x => x).ToList();

        // remove last element
        unfolded.RemoveAt(unfolded.Count() - 1);
        var chars = unfolded.ToArray();

        var nums = splitLine[1].Split(',').Select(x => int.Parse(x)).ToArray();
        // Apply unfolding
        nums = Enumerable.Repeat(nums.ToList(), 5).SelectMany(x => x).ToArray();

        var possibleCombinations = new HashSet<char[]>() { chars };

        for (var i = 0; i < chars.Length; i++)
        {
            var currChar = chars[i];
            if (currChar == '?')
            {
                var newCombinations = possibleCombinations.SelectMany(combo => GetNewCombinations(combo, nums, i));

                sum += newCombinations.Where(x => !x.Contains('?'))
                    .Count(combo => DoesCombinationFit(nums, combo));

                possibleCombinations = newCombinations.Where(x => x.Contains('?')).ToHashSet();
                if (possibleCombinations.Count == 0)
                {
                    return sum;
                }
            }

        }

        // Check which combinations fit the criteria

        sum += possibleCombinations.Count(combo => DoesCombinationFit(nums, combo));
        Console.WriteLine(sum);
        return sum;
    }

    private HashSet<char[]> GetNewCombinations(char[] combo, int[] nums, int i)
    {
        var newCombinations = new HashSet<char[]>();
        var a = combo.TakeWhile(x => x != '?');
        //if (!a.Any())
        //{
        //    sum += (DoesCombinationFit(nums, combo) ? 1 : 0);
        //    continue;
        //}
        if (a.Count() > 0 && a.Count() + 1 < combo.Length)
        {
            var split = string.Join("", a).Split('.').Where(x => x.Contains('#')).ToList();
            var lastEl = split.LastOrDefault();
            var index = split.Count();
            if (a.LastOrDefault() == '#')
            {
                var toAdd = index < nums.Length ? nums[index - 1] - lastEl.Length : 0;
                if (toAdd > 0)
                {
                    var startIndex = combo.TakeWhile(x => x != '?').Count();
                    var nextChar = combo[startIndex];
                    var iterations = 0;
                    while (nextChar == '?' && iterations < toAdd)
                    {
                        // Get index to add to 
                        combo[startIndex + iterations] = '#';
                        if (startIndex + iterations + 1 > combo.Count() - 1) break;
                        nextChar = combo[startIndex + iterations + 1];
                        iterations++;
                    }

                    // If we've added to the loop, continue to the next iteration
                    newCombinations.Add(combo);
                    return newCombinations;
                }
            }
            else if (index == nums.Length)
            {
                var restOfSequence = combo.SkipWhile(x => x != '?');
                // check whether rest of arrays is just ? and .; if it isn't, don't add it
                if (!restOfSequence.Contains('#'))
                {
                    newCombinations.Add(combo.Select(x => x == '?' ? '.' : x).ToArray());
                }
                return newCombinations;
            }
        }

        var arrayOne = (char[])combo.Clone();
        arrayOne[i] = '.';
        if (CheckTwo(nums, arrayOne)) newCombinations.Add(arrayOne);

        var arrayTwo = (char[])combo.Clone();
        arrayTwo[i] = '#';
        if (CheckTwo(nums, arrayTwo)) newCombinations.Add(arrayTwo);
        return newCombinations;
    }

    public void Part2()
    {
        var sum = _input.Select(GetSum).Sum();
        Console.WriteLine(sum);
    }

    // 233ms

}

