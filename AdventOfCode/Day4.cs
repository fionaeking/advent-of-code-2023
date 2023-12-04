namespace AdventOfCode
{
    public class Day4(string[] input) : IDay
    {
        public void Part1()
        {
            double sum = 0;
            foreach (var line in input)
            {
                var splitLine = line.Split(" | ");
                var winningNums = splitLine[0].Split(": ")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                var myNums = splitLine[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

                var matchCount = myNums.Where(winningNums.Contains).Count();

                if (matchCount != 0)
                {
                    sum += Math.Pow(2, matchCount - 1);
                }
            }
            Console.WriteLine(sum);
        }

        public void Part2()
        {
            throw new NotImplementedException();
        }
    }
}