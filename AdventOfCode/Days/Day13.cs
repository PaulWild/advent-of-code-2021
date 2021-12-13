using System.Text;

namespace AdventOfCode.Days;

public class Day13 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (commands, grid) = ParseInput(input);
        grid = Fold(grid, commands[0].axis, commands[0].index);
        return grid.Count.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var (commands, grid) = ParseInput(input);
        grid = commands.Aggregate(grid, (g, command) => Fold(g, command.axis, command.index));
        return GridToString(grid);
    }
    
    private static HashSet<(int x, int y)> Fold(HashSet<(int x, int y)> grid, string axis, int index)
    {
        var newGrid = new HashSet<(int x, int y)>();
        foreach (var point in grid)
        {
            newGrid.Add(axis switch
            {
                "y" when point.y > index => (point.x, index - (point.y - index)),
                "x" when point.x > index => (index - (point.x - index), point.y),
                _ => point
            });
        }
        return newGrid;
    }

    private static (List<(string axis, int index)> commands, HashSet<(int x, int y)> grid) ParseInput(IEnumerable<string> input)
    {
        var finishedGrid = false;
        var commands = new List<(string axis, int index)>();
        var grid = new HashSet<(int x, int y)>();

        foreach (var row in input)
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                finishedGrid = true;
            }
            else if (finishedGrid)
            {
                var parsed = row.Split(" ")[2].Split("=");
                commands.Add((axis: parsed[0], index: int.Parse(parsed[1])));
            }
            else
            {
                var parsed = row.Split(",");
                grid.Add((x: int.Parse(parsed[0]), y: int.Parse(parsed[1])));
            }
        }

        return (commands, grid);
    }
    
    private static string GridToString(HashSet<(int x, int y)> grid)
    {
        var toReturn = new StringBuilder();
        toReturn.AppendLine("");
        for (var y = 0; y <= grid.Select(point => point.y).Max(); y++)
        {
            for (var x = 0; x <= grid.Select(point => point.x).Max(); x++)
            {
                toReturn.Append(grid.Contains((x, y)) ? '▓' : ' ');
            }

            toReturn.AppendLine();
        }

        return toReturn.ToString();
    }


    public int Day => 13;
}
