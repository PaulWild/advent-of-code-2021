namespace AdventOfCode.Days;

public class Day10 : ISolution
{
    private static readonly char[] Open = { '[', '<', '{', '(' };

    public string PartOne(IEnumerable<string> input)
    {
        var invalidSum = 0;
        var bracketStack = new Stack<char>();

        foreach (var row in input)
        {
            var brackets = row.ToCharArray();
            foreach (var bracket in brackets)
            {
                if (Open.Contains(bracket))
                {
                    bracketStack.Push(bracket);
                }
                else
                {
                    var toClose = bracketStack.Pop();
                    if (toClose == '<' && bracket != '>' || toClose == '{' && bracket != '}' || toClose == '[' && bracket != ']' || toClose == '(' && bracket != ')')
                    {
                        invalidSum += bracket switch
                        {
                            ')' => 3,
                            ']' => 57,
                            '}' => 1197,
                            '>' => 25137,
                            _ => throw new ArgumentOutOfRangeException(nameof(bracket))
                        };
                        break;
                    }
                }
            }
        }

        return invalidSum.ToString();
    }

    private static bool IsValid(char[] chunks)
    {
        var bracketStack = new Stack<char>();
        foreach (var bracket in chunks)
        {
            if (Open.Contains(bracket))
            {
                bracketStack.Push(bracket);
            }
            else
            {
                var toClose = bracketStack.Pop();
                if (toClose == '<' && bracket != '>' || toClose == '{' && bracket != '}' ||
                    toClose == '[' && bracket != ']' || toClose == '(' && bracket != ')')
                {
                    return false;
                }
            }
        }
        return true;
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var totalScores = new List<long>();
  
        foreach (var validRow in input.Select(x => x.ToCharArray()).Where(IsValid))
        {
            var bracketStack = new Stack<char>();
            foreach (var bracket in validRow)
            {
                if (Open.Contains(bracket))
                {
                    bracketStack.Push(bracket);
                }
                else
                {
                    bracketStack.Pop();
                }
            }

            var totalScore = 0L;
            while (bracketStack.Count > 0)
            {
                var toClose = bracketStack.Pop();
                totalScore *= 5;
                totalScore += toClose switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                    _ => throw new ArgumentOutOfRangeException(nameof(toClose))
                };
            }
            totalScores.Add(totalScore);
        }
        totalScores.Sort();
        var middle = totalScores.Count / 2;
        return totalScores.Skip(middle).Take(1).First().ToString();
    }

    public int Day => 10;
}
