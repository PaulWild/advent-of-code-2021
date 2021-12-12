namespace AdventOfCode.Days;

public class Day12 : ISolution
{
    private static bool CanExplore(List<string> explored, string cave)
    {
        return cave.ToUpper() == cave || !explored.Contains(cave);
    }
    
    private static bool CanExplorePart2(List<string> explored, string cave)
    {
        if (cave == "start") return false;
        if (cave == "end") return true;
        if (cave.ToUpper() == cave) return true;

        var caveCounts = explored
            .Where(x => x.ToUpper() != x)
            .GroupBy(x => x)
            .ToDictionary(key => key.Key, value => value.Count());

        return caveCounts.Values.All(x => x == 1) || !caveCounts.ContainsKey(cave);

    }
    public string PartOne(IEnumerable<string> input)
    {
        return CountPaths(input, CanExplore);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return CountPaths(input, CanExplorePart2);
    }


    private static string CountPaths(IEnumerable<string> input,Func<List<string>, string, bool> canExplore)
    {
        var caves = input
            .Select(path => path.Split("-"))
            .SelectMany(path => new[] { (start: path[1], end: path[0]), (start: path[0], end: path[1]) })
            .GroupBy(x => x.start)
            .ToDictionary(key => key.Key, value => value.Select(x => x.end).ToList());


        var incompletePaths = caves["start"].Select(x => new List<string> {"start", x}).ToList();
        var incompleteQueue = new Queue<List<string>>(incompletePaths);

        var completedPaths = new List<List<string>>();
        while (incompleteQueue.Count > 0)
        {
            var pathToExpand = incompleteQueue.Dequeue();
            var lastNode = pathToExpand.Last();

            var potentialNext = caves[lastNode].Where(x => canExplore(pathToExpand, x));

            foreach (var toAdd in potentialNext)
            {
                var newPath = pathToExpand.Select(x => x).ToList();
                newPath.Add(toAdd);
                if (toAdd == "end")
                {
                    completedPaths.Add(newPath);
                }
                else
                {
                    incompleteQueue.Enqueue(newPath);
                }
            }
        }

        return completedPaths.Count.ToString();
    }
    
    public int Day => 12;
}
