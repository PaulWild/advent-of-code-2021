using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day15 : ISolution
{
    
    public string PartOne(IEnumerable<string> input)
    {
        var grid = ParseGrid(input.ToArray());
        
        return ShortestPath(grid);
    }

    private static Dictionary<(int x, int y), int> ParseGrid(IEnumerable<string> input)
    {
        return input
            .SelectMany((valueString, yIdx) => 
                valueString.Select((energyLevel, xIdx) => (location: (xIdx, yIdx), riskLevel: int.Parse(energyLevel.ToString()))))
            .ToDictionary(x => x.location, x => x.riskLevel);  
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var grid = ParseGrid(input.ToList());
        var xLength = grid.Keys.MaxBy(x => x.x).x + 1;
        var yLength = grid.Keys.MaxBy(y => y.y).y + 1;

        for (var ySkip = 0; ySkip < 5; ySkip++)
        for (var xSkip = 0 ; xSkip < 5; xSkip++)
        {
            if (ySkip ==0 && xSkip ==0) continue;
            
            var increment = ySkip  + xSkip;

            for (var y = 0; y < xLength; y++)
            for (var x = 0; x < yLength; x++)
            {
                var newX = (yLength * xSkip) + x ;
                var newY = (yLength * ySkip) + y ;
                var newValue = (grid[(x, y)] + increment) % 9;
                newValue = newValue == 0 ? 9 : newValue;
                grid[(newX, newY)] = newValue;
            }
        }

   
        return ShortestPath(grid);
    }

    private static string ShortestPath(Dictionary<(int x, int y), int> q)
    {
        var start = (0, 0);
        var end = (q.Keys.Select(pos => pos.x).Max(), q.Keys.Select(pos => pos.y).Max());

        
        var dist = q.Select(x => KeyValuePair.Create(x.Key, int.MaxValue)).ToDictionary(k => k.Key, v => v.Value);

        var distQ = new Dictionary<(int x, int y), int> { { (0, 0), 0 } };
    

        dist[start] = 0;

        while (q.Count > 0)
        {
            var key = distQ.MinBy(x => x.Value).Key;

            q.Remove(key);
            distQ.Remove(key);


            var currentDist = dist[key];
            foreach (var neighbour in q.DirectNeighbours(key))
            {
                var neighbourDist = dist[neighbour];
                var neighbourCost = q[neighbour];

                if (currentDist + neighbourCost >= neighbourDist) continue;
                
                dist[neighbour] = currentDist + neighbourCost;
                if (q.ContainsKey(neighbour))
                {
                    distQ[neighbour] = currentDist + neighbourCost;
                }
            }
        }

        return dist[end].ToString();
    }

    public int Day => 15;
}
