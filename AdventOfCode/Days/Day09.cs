using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day09 : ISolution
{
    private static IEnumerable<int> AsNumbers(string str)
    {
        return str.ToCharArray().Select(x => int.Parse(x.ToString()));
    }
    
    public static IEnumerable<(int x, int y)> DirectNeighbours2((int x, int y) location)
    {
        var (x, y) = location;
        yield return (x, y - 1);
        yield return (x, y + 1);
        yield return (x - 1, y);
        yield return (x + 1, y);
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => AsNumbers(valueString).Select((height, xIdx) => ((xIdx, yIdx), height)))
            .ToDictionary(x => x.Item1, x => x.height);

        var minima = (
            from kvp in grid 
            let neighbours = DirectNeighbours2(kvp.Key).Where(x => grid.TryGetValue(x, out _)).Select(x => grid[x]) 
            where neighbours.All(x => x > kvp.Value) 
            select (kvp.Value + 1)).Sum();

        return minima.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => AsNumbers(valueString).Select((height, xIdx) => ((xIdx, yIdx), height)))
            .ToDictionary(x => x.Item1, x => x.height);

        var minima = 
            from kvp in grid 
            let neighbours = DirectNeighbours2(kvp.Key).Where(x => grid.TryGetValue(x, out _)).Select(x => grid[x]) 
            where neighbours.All(x => x > kvp.Value) 
            select kvp.Key;

        List<int> basinCounts = new ();
        foreach (var minimum in minima)
        {
            Queue<(int x, int y)> locationsToExplore = new();
            locationsToExplore.Enqueue(minimum);
            HashSet<(int x, int y)> exploredLocation = new() { minimum};
            var basinCount = 1;
            
            do
            {
                var toExplore = locationsToExplore.Dequeue();
                
                var neighbours = DirectNeighbours2(toExplore)
                    .Where(x => grid.TryGetValue(x, out _))
                    .Where(x => !exploredLocation.Contains(x));

                
                foreach (var neighbourLocation in neighbours)
                {
                    if (grid[neighbourLocation] != 9)
                    {
                        basinCount++;
                        if (!locationsToExplore.Contains(neighbourLocation))
                        {
                            locationsToExplore.Enqueue(neighbourLocation);
                        }
                    }

                    exploredLocation.Add(neighbourLocation);
                }
            } while (locationsToExplore.Count > 0);
            basinCounts.Add(basinCount);
        }

        return basinCounts.OrderByDescending(x => x).Take(3).Aggregate((x, y) => x * y).ToString();
    }

    public int Day => 09;
}
