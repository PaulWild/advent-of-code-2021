using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day06 : ISolution
{
    

    public string PartOne(IEnumerable<string> input)
    {
        var lanternFish = input.First()
            .Split(",")
            .Select(x => Convert.ToInt32(x)).GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());
        
        for (var day  = 1; day<= 80; day++)
        {
            var newLanternFishCycle = new Dictionary<int, int>();
            foreach (var (key, value) in lanternFish)
            {
                if (key == 0)
                {
                    newLanternFishCycle.Add(8,value);
                    newLanternFishCycle.AddOrUpdate(6, value,x => x+value);
                }
                else
                {
                    newLanternFishCycle.AddOrUpdate(key-1, value,x => x+value);
                }
            }

            lanternFish = newLanternFishCycle;
        }

        return lanternFish.Values.Sum().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var lanternFish = input.First()
            .Split(",")
            .Select(x => Convert.ToInt32(x)).GroupBy(x => x)
            .ToDictionary(x => x.Key, x => Convert.ToInt64(x.Count()));
        
        for (var day  = 1; day<= 256; day++)
        {
            var newLanternFishCycle = new Dictionary<int, long>();
            foreach (var (key, value) in lanternFish)
            {
                if (key == 0)
                {
                    newLanternFishCycle.Add(8,value);
                    newLanternFishCycle.AddOrUpdate(6, value,x => x+value);
                }
                else
                {
                    newLanternFishCycle.AddOrUpdate(key-1, value,x => x+value);
                }
            }

            lanternFish = newLanternFishCycle;
        }

        return lanternFish.Values.Sum().ToString();
    }

    public int Day => 06;
}
