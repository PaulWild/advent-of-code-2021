using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day11 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => valueString.Select((energyLevel, xIdx) => (location: (xIdx, yIdx), energyLevel: int.Parse(energyLevel.ToString()))))
            .ToDictionary(x => x.location, x => x.energyLevel);

        var totalFlashes = 0;
        for (var step = 0; step < 100; step++)
        {
            Queue<(int x, int y)> toFlash = new();
            Queue<(int x, int y)> flashed = new();

            foreach (var (key, value) in grid)
            {
                grid[key] = value+1;
            }

            foreach (var (key, value) in grid)
            {
                if (value <= 9) continue;
                if (flashed.Contains(key)) continue;
                
                toFlash.Enqueue(key);
                flashed.Enqueue(key);
            }
            
            while (toFlash.Any())
            {
                var flashingOctopus = toFlash.Dequeue();
                totalFlashes++;

                foreach (var neighbour in grid.AllNeighbours(flashingOctopus))
                {
                    grid[neighbour] += 1;
                }

                foreach (var (key, value) in grid)
                {
                    if (value <= 9) continue;
                    if (flashed.Contains(key)) continue;
                    
                    toFlash.Enqueue(key);
                    flashed.Enqueue(key);
                }
            }
            
            foreach (var (key, value) in grid)
            {
                if (value > 9)
                {
                    grid[key] = 0;
                }

            }
        }

        return totalFlashes.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), int> grid = input
            .SelectMany((valueString, yIdx) => 
                valueString.Select((energyLevel, xIdx) => (location: (xIdx, yIdx), energyLevel: int.Parse(energyLevel.ToString()))))
            .ToDictionary(x => x.location, x => x.energyLevel);
        
        var step = 0;
        for (;;)
        {
            Queue<(int x, int y)> toFlash = new();
            Queue<(int x, int y)> flashed = new();

            foreach (var (key, value) in grid)
            {
                grid[key] = value+1;
            }

            foreach (var (key, value) in grid)
            {
                if (value <= 9) continue;
                if (flashed.Contains(key)) continue;
                
                toFlash.Enqueue(key);
                flashed.Enqueue(key);
            }
            
            while (toFlash.Any())
            {
                var flashingOctopus = toFlash.Dequeue();

                foreach (var neighbour in grid.AllNeighbours(flashingOctopus))
                {
                    grid[neighbour] += 1;
                }
                
                foreach (var (key, value) in grid)
                {
                    if (value <= 9) continue;
                    if (flashed.Contains(key)) continue;
                    
                    toFlash.Enqueue(key);
                    flashed.Enqueue(key);
                }
            }
            
            foreach (var (key, value) in grid)
            {
                if (value > 9)
                {
                    grid[key] = 0;
                }

            }

            step++;
            if (grid.Values.All(x => x == 0))
            {
                return step.ToString();
            }
        }
        
    }

    public int Day => 11;
}
