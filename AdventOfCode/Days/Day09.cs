using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day09 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => valueString.Select((height, xIdx) => ((xIdx, yIdx), height: int.Parse(height.ToString()))))
            .ToDictionary(x => x.Item1, x => x.height);

        var minima = (
            from kvp in grid 
            let neighbours = grid.DirectNeighbours(kvp.Key).Select(x => grid[x]) 
            where neighbours.All(x => x > kvp.Value) 
            select (kvp.Value + 1)).Sum();

        return minima.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => valueString.Select((height, xIdx) => ((xIdx, yIdx), height: int.Parse(height.ToString()))))
            .ToDictionary(x => x.Item1, x => x.height);

        var minima = 
            from kvp in grid 
            let neighbours = grid.DirectNeighbours(kvp.Key).Select(x => grid[x]) 
            where neighbours.All(x => x > kvp.Value) 
            select kvp.Key;

        List<int> basinCounts = new ();
        foreach (var minimum in minima)
        {
            Queue<(int x, int y)> locationsToExplore = new();
            locationsToExplore.AddDistinct(minimum);
            HashSet<(int x, int y)> exploredLocation = new() { minimum};
            var basinCount = 1;
            
            do
            {
                var toExplore = locationsToExplore.Dequeue();
                
                var neighbours = grid.DirectNeighbours(toExplore)
                    .Where(x => !exploredLocation.Contains(x));
                
                foreach (var neighbourLocation in neighbours)
                {
                    if (grid[neighbourLocation] != 9)
                    {
                        basinCount++;
                        locationsToExplore.AddDistinct(neighbourLocation);
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
