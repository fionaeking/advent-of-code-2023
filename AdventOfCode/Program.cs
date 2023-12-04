namespace AdventOfCode
{
    internal class Program
    {
        private static void Main()
        {
            var input = File.ReadAllLines("PuzzleInput.txt");
            var day = new Day4(input);
            day.Part1();
            //sday.Part2();
        }
    }
}
