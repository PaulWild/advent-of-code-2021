using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day06 : ISolution
{
    

    public string PartOne(IEnumerable<string> input)
    { 
        var lanternFish = ParseInput(input);
        lanternFish = SimulateLifecylcesForDays(lanternFish, 80);

        return lanternFish.Values.Sum().ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var lanternFish = ParseInput(input);
        lanternFish = SimulateLifecylcesForDays(lanternFish, 256);

        return lanternFish.Values.Sum().ToString();
    }

    private static Dictionary<int, long> SimulateLifecylcesForDays(Dictionary<int, long> startingPopulations, int days)
    {
        var populationCounts = startingPopulations;
        for (var day = 1; day <= days; day++)
        {
            var newLanternFishCycle = new Dictionary<int, long>();
            foreach (var (key, value) in populationCounts)
            {
                if (key == 0)
                {
                    newLanternFishCycle.Add(8, value);
                    newLanternFishCycle.AddOrUpdate(6, value, x => x + value);
                }
                else
                {
                    newLanternFishCycle.AddOrUpdate(key - 1, value, x => x + value);
                }
            }

            populationCounts = newLanternFishCycle;
        }

        return populationCounts;
    }

    private static Dictionary<int, long> ParseInput(IEnumerable<string> input)
    {
        var lanternFish = input.First()
            .Split(",")
            .Select(x => Convert.ToInt32(x)).GroupBy(x => x)
            .ToDictionary(x => x.Key, x => Convert.ToInt64(x.Count()));
        return lanternFish;
    }

    public int Day => 06;
}
