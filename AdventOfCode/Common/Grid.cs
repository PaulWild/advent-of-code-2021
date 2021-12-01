
namespace AdventOfCode.Common;

public class Grid
{
    public static IEnumerable<(int x, int y, int z)> Neighbours3(int x, int y, int z)
    {
        for (var zNeighbour = z - 1; zNeighbour <= z + 1; zNeighbour++)
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        for (var xNeighbour = x - 1; xNeighbour <= x + 1; xNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y && zNeighbour == z))
            {
                yield return (xNeighbour, yNeighbour, zNeighbour);
            }
        }
    }

    public static IEnumerable<(int x, int y, int z, int w)> Neighbours4(int x, int y, int z, int w)
    {
        for (var wNeighbour = w - 1; wNeighbour <= w + 1; wNeighbour++)
        for (var zNeighbour = z - 1; zNeighbour <= z + 1; zNeighbour++)
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        for (var xNeighbour = x - 1; xNeighbour <= x + 1; xNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y && zNeighbour == z && wNeighbour == w))
            {
                yield return (xNeighbour, yNeighbour, zNeighbour, wNeighbour);
            }
        }
    }
}