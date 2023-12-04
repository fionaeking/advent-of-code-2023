namespace AdventOfCode
{
    public class Day4(string[] input) : IDay
    {
        public void Part1()
        {
            var partOne = input.Select(x => Parse(x)).Where(x => x.Item2 > 0).Select(x => Math.Pow(2, x.Item2 - 1)).Sum();
            Console.WriteLine(partOne);
        }

        public void Part2()
        {
            var scratchcardCount = 0;
            var parsedInput = input.Select(x => Parse(x)).ToDictionary();

            var cardNumsQueue = new Queue<int>(parsedInput.Keys);

            while (cardNumsQueue.Count != 0)
            {
                scratchcardCount++;
                var currCard = cardNumsQueue.Dequeue();
                var matchCount = parsedInput[currCard];
                if (matchCount != 0)
                {
                    for (int i = 1; i <= matchCount; i++)
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
            var cardNum = int.Parse(splitLine[0].Split(": ")[0].Replace("Card ", ""));
            var winningNums = splitLine[0].Split(": ")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
            var myNums = splitLine[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
            var matchCount = myNums.Where(winningNums.Contains).Count();
            return (cardNum, matchCount);
        }
    }
}