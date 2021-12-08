namespace AdventOfCode.Days;

public class Day08 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        return input
            .SelectMany(x => x.Split("|")[1].Split(" ", StringSplitOptions.TrimEntries))
            .Count(x => new[] { 2, 4, 3, 7 }.Contains(x.Length))
            .ToString();
    }

    private static bool ContainsArray(char[] input, char[] comparator)
    {
        return comparator.All(input.Contains);
    }
    
    private static bool ArrayEqual(char[] input, char[] comparator)
    {
        return comparator.All(input.Contains) && input.All(comparator.Contains);
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var outputNumbers = new List<int>();
        foreach (var numbers in input)
        {
            var tmp = numbers
                .Split("|", StringSplitOptions.TrimEntries)
                .Select(x => x.Split(" ", StringSplitOptions.TrimEntries))
                .ToArray();
            var garbled = tmp[0].Select(x => x.ToCharArray()).ToList();
            var output = tmp[1].Select(x => x.ToCharArray()).ToArray();

            
            Dictionary<int, Char[]> numberMap = new()
            {
                { 1, garbled.Single(x => x.Length == 2) },
                { 8, garbled.Single(x => x.Length == 7) },
                { 7, garbled.Single(x => x.Length == 3) },
                { 4, garbled.Single(x => x.Length == 4) }
            };
            garbled = garbled
                .Where(x => !ArrayEqual(x, numberMap[1]))
                .Where(x => !ArrayEqual(x, numberMap[4]))
                .Where(x => !ArrayEqual(x, numberMap[7]))
                .Where(x => !ArrayEqual(x, numberMap[8])).ToList();

            var six = garbled.Single(x => x.Length == 6 && !ContainsArray(x, numberMap[1]));
            numberMap.Add(6,six);
            garbled = garbled.Where(x => !ArrayEqual(x, numberMap[6])).ToList();

            var nine = garbled.Single(x =>
                x.Length == 6 && ContainsArray(x, numberMap[1]) && ContainsArray(x, numberMap[4]));
            numberMap.Add(9, nine);     
            garbled = garbled.Where(x => !ArrayEqual(x, numberMap[9])).ToList();

            //Zero is the last 6 segment number
            var zero = garbled.Single(x => x.Length == 6 ); 
            numberMap.Add(0, zero);    
            garbled = garbled.Where(x => !ArrayEqual(x, numberMap[0])).ToList();

            var three = garbled.Single(x => x.Length == 5 && ContainsArray(x, numberMap[1]));
            numberMap.Add(3, three);        
            garbled = garbled.Where(x => !ArrayEqual(x, numberMap[3])).ToList();       

            var five = garbled.Single(x => x.Length == 5 && x.Count(y => numberMap[6].Contains(y)) == 5);
            numberMap.Add(5, five);    
            garbled = garbled.Where(x => !ArrayEqual(x, numberMap[5])).ToList();
            
            numberMap.Add(2, garbled.Single());       


            outputNumbers.Add(int.Parse(string.Join("", output.Select(x => numberMap.First(kvp => ArrayEqual(x, kvp.Value)).Key))));
        }

        return outputNumbers.Sum().ToString();
    }

    public int Day => 08;
}
