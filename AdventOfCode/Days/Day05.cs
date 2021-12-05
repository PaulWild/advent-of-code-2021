using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day05 : ISolution
{
    private record struct Coordinate((int x, int y) From, (int x, int y) To);
    
    public string PartOne(IEnumerable<string> input)
    {
        var vents = new Dictionary<(int x, int y), int>();
        var coordinates = ParseInput(input);

        MapHorizontalVerticalVents(coordinates, vents);

        return vents.Values.Count(x => x > 1).ToString();

    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var vents = new Dictionary<(int x, int y), int>();
        var coordinates = ParseInput(input);
     
        MapHorizontalVerticalVents(coordinates, vents);
        MapDiagonalVents(coordinates, vents);
        
        return vents.Values.Count(x => x > 1).ToString();
    }
    
    private static List<Coordinate> ParseInput(IEnumerable<string> input)
    {
       return  (
            from row in input
            select row.Split(" -> ")
            into numbers
            let fromString = numbers[0].Split(",")
            let toString = numbers[1].Split(",")
            select new Coordinate(
                (Convert.ToInt32(fromString[0]), Convert.ToInt32(fromString[1])),
                (Convert.ToInt32(toString[0]), Convert.ToInt32(toString[1])))
        ).ToList();
    }
    
    private static void MapHorizontalVerticalVents(IEnumerable<Coordinate> coordinates, Dictionary<(int x, int y), int> vents)
    {
        foreach (var ((fromX, fromY), (toX, toY)) in coordinates.Where(coord =>
                     coord.From.x == coord.To.x || coord.From.y == coord.To.y))
        {
            var minX = Math.Min(fromX, toX);
            var maxX = Math.Max(fromX, toX);
            var minY = Math.Min(fromY, toY);
            var maxY = Math.Max(fromY, toY);

            for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
            {
                vents.AddOrUpdate((x, y), 1, value => value + 1);
            }
        }
    }
    
    private static void MapDiagonalVents(IEnumerable<Coordinate> coordinates, Dictionary<(int x, int y), int> vents)
    {
        foreach (var ((fromX, fromY), (toX, toY)) in coordinates.Where(coord =>
                     !(coord.From.x == coord.To.x || coord.From.y == coord.To.y)))
        {
            var minX = Math.Min(fromX, toX);
            var maxX = Math.Max(fromX, toX);
            var xDelta = fromX < toX ? 1 : -1;
            var yDelta = fromY < toY ? 1 : -1;

            for (var increment = 0; increment <= maxX - minX; increment++)
            {
                var xUpdate = fromX + (increment * xDelta);
                var yUpdate = fromY + (increment * yDelta);

                vents.AddOrUpdate((xUpdate, yUpdate), 1, value => value + 1);
            }
        }
    }

    public int Day => 05;
}
