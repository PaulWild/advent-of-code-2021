#pragma warning disable CS8601
namespace AdventOfCode.Days;

public class Day14 : ISolution
{
    public record Polymer(char Left, char Right);
    
    public class ExpansionTree
    {
        public ExpansionTree(Polymer node)
        {
            Node = node;
        }

        public Polymer Node { get; }
        
        public ExpansionTree Left { get; set; } = null!;

        public ExpansionTree Right { get; set; } = null!;

        public long CountLetter(int currentDepth, int depth, char letter, Dictionary<(Polymer, int), long> seenValues)
        {
            var toReturn = 0L;
            if (currentDepth == 0)
            {
                if (Node.Left == letter) toReturn++;
            }

            if (depth == currentDepth)
            {
                if (Node.Left == letter) toReturn++;
                return toReturn;
            }

            if (seenValues.ContainsKey((Left.Node, currentDepth + 1)))
            {
                toReturn += seenValues[(Left.Node, currentDepth + 1)];
            }
            else
            {
                var hmm = Left.CountLetter(currentDepth + 1, depth, letter, seenValues);
                seenValues[(Left.Node, currentDepth + 1)] = hmm;
                toReturn += hmm;
            }

            if (seenValues.ContainsKey((Right.Node, currentDepth + 1)))
            {
                toReturn += seenValues[(Right.Node, currentDepth + 1)];
            }
            else
            {
                var hmm = Right.CountLetter(currentDepth + 1, depth, letter, seenValues);
                seenValues[(Right.Node, currentDepth + 1)] = hmm;
                toReturn += hmm;
            }


            return toReturn;
        }
    }

    private static void ExpandTree(ExpansionTree tree, IDictionary<Polymer, ExpansionTree?> expandedTrees, IReadOnlyDictionary<Polymer, (Polymer, Polymer)> polymerMap)
    {
        var polymerLeft = polymerMap[tree.Node].Item1;
        var polymerRight = polymerMap[tree.Node].Item2;

        if (expandedTrees.ContainsKey(polymerLeft))
        {
            tree.Left = expandedTrees[polymerLeft];
        }
        else
        {
            var leftTree = new ExpansionTree(polymerLeft);
            expandedTrees[polymerLeft] = leftTree;
            tree.Left = leftTree;
            ExpandTree(leftTree, expandedTrees, polymerMap);
        }
        
        if (expandedTrees.ContainsKey(polymerRight))
        {
            tree.Right = expandedTrees[polymerRight];
        }
        else
        {
            var rightTree = new ExpansionTree(polymerRight);
            expandedTrees[polymerRight] = rightTree;
            tree.Right = rightTree;
            ExpandTree(rightTree, expandedTrees, polymerMap);
        }
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        return SolveForNTimes(input.ToList(), 10);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return SolveForNTimes(input.ToList(), 40);   
    }


    private static string SolveForNTimes(ICollection<string> input, int n)
    {
       
        var polymerTemplate = input.First().ToCharArray().ToList();

        var pairInsertionRules = input.Skip(2)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(pairs =>
            {
                var from = pairs[0].ToCharArray();
                var polymer = new Polymer(@from[0], @from[1]);
                var leftExpansion = new Polymer(@from[0], pairs[1].ToCharArray().First());
                var rightExpansion = new Polymer(pairs[1].ToCharArray().First(), @from[1]);
                return KeyValuePair.Create(polymer, (leftExpansion, rightExpansion));
            }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var expansionTrees = new Dictionary<Polymer, ExpansionTree?>();
        var allLetters = new HashSet<char>(pairInsertionRules.Keys.SelectMany(x => new[] { x.Left, x.Right }));

        var trees = new List<ExpansionTree>();
        for (var i = 0; i < polymerTemplate.Count - 1; i++)
        {
            var tree = new ExpansionTree(new Polymer(polymerTemplate[i], polymerTemplate[i + 1]));
            ExpandTree(tree, expansionTrees, pairInsertionRules);
            trees.Add(tree);
        }


        var counts = new List<long>();
        foreach (var letter in allLetters)
        {
            var dp = new Dictionary<(Polymer, int), long>();
            var count = trees.Select(x => x.CountLetter(0, n, letter, dp)).Sum();
            if (trees.Last().Node.Right == letter) count++;

            if (count > 0) counts.Add(count);
        }

        return (counts.Max() - counts.Min()).ToString();
    }
    
    public int Day => 14;
}
