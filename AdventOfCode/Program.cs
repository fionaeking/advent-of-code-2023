namespace AdventOfCode
{
    internal class Program
    {
        private static void Main()
        {
            var input = File.ReadAllLines("PuzzleInput.txt");
            var day = new Day1(input);
            day.Part1();
            day.Part2();
        }
    }
}
