using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day14 : ISolution
{
    public record polymer(char Left, char Right);
    
    public class ExpansionTree
    {
        public ExpansionTree(polymer node)
        {
            Node = node;
        }

        public polymer Node { get; set; }
        
        public ExpansionTree Left { get; set; }
        
        public ExpansionTree Right { get; set; }

        public void ExpandedPolymer(int currentDepth, int depth, List<char> expansion)
        {
            if (currentDepth == 0)
            {
                expansion.Add(this.Node.Left);
            }
            if (depth == currentDepth)
            {
                expansion.Add(this.Node.Right);
            }
            else
            {
                this.Left.ExpandedPolymer(currentDepth + 1, depth, expansion);
                this.Right.ExpandedPolymer(currentDepth + 1, depth, expansion);
            }
        }

        public long CountLetter(int currentDepth, int depth, char letter, Dictionary<(polymer, int), long> seenValues)
        {
            var toReturn = 0L;
            if (currentDepth == 0)
            {
                if (this.Node.Left == letter) toReturn++;
            }

            if (depth == currentDepth)
            {
                if (this.Node.Left == letter) toReturn++;
                return toReturn;
            }

            if (seenValues.ContainsKey((this.Left.Node, currentDepth + 1)))
            {
                toReturn += seenValues[(this.Left.Node, currentDepth + 1)];
            }
            else
            {
                var hmm = this.Left.CountLetter(currentDepth + 1, depth, letter, seenValues);
                seenValues[(this.Left.Node, currentDepth + 1)] = hmm;
                toReturn += hmm;
            }

            if (seenValues.ContainsKey((this.Right.Node, currentDepth + 1)))
            {
                toReturn += seenValues[(this.Right.Node, currentDepth + 1)];
            }
            else
            {
                var hmm = this.Right.CountLetter(currentDepth + 1, depth, letter, seenValues);
                seenValues[(this.Right.Node, currentDepth + 1)] = hmm;
                toReturn += hmm;
            }


            return toReturn;
        }
    }

    public void ExpandTree(ExpansionTree tree, Dictionary<polymer,ExpansionTree> expandedTrees, Dictionary<polymer, (polymer, polymer)> polymerMap)
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
        var polymerTemplate = input.First().ToCharArray().ToList() ;
        
        var pairInsertionRules = input.Skip(2)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(pairs =>
            {
                var from = pairs[0].ToCharArray();
                var polymer = new polymer(from[0], from[1]);
                var leftExpansion = new polymer(from[0], pairs[1].ToCharArray().First());
                var rightExpansion = new polymer(pairs[1].ToCharArray().First(), from[1]);
                return KeyValuePair.Create(polymer, (leftExpansion, rightExpansion));
            }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var expansionTrees = new Dictionary<polymer,ExpansionTree>();
        var allLetters = new HashSet<char>(pairInsertionRules.Keys.SelectMany(x => new[] { x.Left, x.Right }));
        
        var trees = new List<ExpansionTree>();
        for (int i = 0; i < polymerTemplate.Count - 1; i++)
        {
            var tree = new ExpansionTree(new polymer(polymerTemplate[i], polymerTemplate[i + 1]));
            ExpandTree(tree, expansionTrees, pairInsertionRules);
            trees.Add(tree);
        }


        var counts = new List<long>();
        foreach (var letter in allLetters)
        {
            var dp = new Dictionary<(polymer, int), long>();
            var count = trees.Select(x => x.CountLetter(0, 10, letter,dp)).Sum();
            if (trees.Last().Node.Right == letter) count++;
            
            if (count > 0) counts.Add(count);
        }

        return (counts.Max() - counts.Min()).ToString();
    }

    private static string PartOneSolution(IEnumerable<string> input)
    {
        var polymerTemplate = input.First().ToCharArray().ToList();
        var pairInsertionRules = input.Skip(2)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(pairs =>
            {
                var from = pairs[0].ToCharArray();
                return KeyValuePair.Create((first: @from[0], second: @from[1]), pairs[1].ToCharArray()[0]);
            }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var polymer = polymerTemplate;

        for (var step = 1; step <= 10; step++)
        {
            var newPolymer = new List<char>();
            for (var i = 0; i < polymer.Count; i++)
            {
                if (i == (polymer.Count - 1))
                {
                    newPolymer.Add(polymer[i]);
                }
                else
                {
                    var rule = pairInsertionRules[(polymer[i], polymer[i + 1])];
                    newPolymer.Add(polymer[i]);
                    newPolymer.Add(rule);
                }
            }

            polymer = newPolymer;
            var countsFoo = polymer.GroupBy(x => x).Select(x => (x.Key, x.Count())).ToList();
            //Console.WriteLine($"H: {countsFoo.First(x => x.Key == 'H').Item2}, Total: {countsFoo.Select(x => x.Item2).Sum()}, Percentage: {((double)countsFoo.First(x => x.Key == 'H').Item2)/((double)countsFoo.Select(x => x.Item2).Sum())}");
            Console.WriteLine(string.Join("", polymer));
        }

        var counts = polymer.GroupBy(x => x).Select(x => x.Count()).ToList();
        return (counts.Max() - counts.Min()).ToString();
    }

    // I think this might be some sort of DP problem. Keep track of all the changes from earlier in the chain but that doesn't seem right
    public string PartTwo(IEnumerable<string> input)
    {
        var polymerTemplate = input.First().ToCharArray().ToList() ;
        
        var pairInsertionRules = input.Skip(2)
            .Select(x => x.Split("->", StringSplitOptions.TrimEntries))
            .Select(pairs =>
            {
                var from = pairs[0].ToCharArray();
                var polymer = new polymer(from[0], from[1]);
                var leftExpansion = new polymer(from[0], pairs[1].ToCharArray().First());
                var rightExpansion = new polymer(pairs[1].ToCharArray().First(), from[1]);
                return KeyValuePair.Create(polymer, (leftExpansion, rightExpansion));
            }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var expansionTrees = new Dictionary<polymer,ExpansionTree>();
        var allLetters = new HashSet<char>(pairInsertionRules.Keys.SelectMany(x => new[] { x.Left, x.Right }));
        
        var trees = new List<ExpansionTree>();
        for (int i = 0; i < polymerTemplate.Count - 1; i++)
        {
            var tree = new ExpansionTree(new polymer(polymerTemplate[i], polymerTemplate[i + 1]));
            ExpandTree(tree, expansionTrees, pairInsertionRules);
            trees.Add(tree);
        }


        var counts = new List<long>();
        foreach (var letter in allLetters)
        {
            var dp = new Dictionary<(polymer, int), long>();
            var count = trees.Select(x => x.CountLetter(0, 40, letter,dp)).Sum();
            if (trees.Last().Node.Right == letter) count++;
            
            if (count > 0) counts.Add(count);
        }

        return (counts.Max() - counts.Min()).ToString();
    }

    public int Day => 14;
}
