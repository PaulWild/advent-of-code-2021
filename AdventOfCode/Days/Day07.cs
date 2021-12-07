namespace AdventOfCode.Days;

public class Day07 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        return CalculateMinFuelFromInput(input, CalculateFuelPart1);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return CalculateMinFuelFromInput(input, CalculateFuelPart2);
    }
    
    private static string CalculateMinFuelFromInput(IEnumerable<string> input, Func<int,int,int> calculateFuelCost)
    {
        var sum = input.First().Split(',').Select(int.Parse).ToList();
        var min = sum.Min();
        var max = sum.Max();

        var minCost = Enumerable.Range(min, max - min)
            .Select(position => sum.Select(x => calculateFuelCost(x, position)).Sum())
            .Min();

        return minCost.ToString();
    }

    private static int CalculateFuelPart1(int x, int position)
    {
        return Math.Abs(x - position); 
    }
    
    private static int CalculateFuelPart2(int x, int position)
    {
        var steps = Math.Abs(x - position);
        var sum = 0;
        for (var i = 1; i <= steps; i++)
        {
            sum += i;
        }
        return sum;
    }

    public int Day => 07;
}
