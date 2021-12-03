namespace AdventOfCode.Days;

public class Day03 : ISolution
{
    private static (char[] mostCommon, char[] leastCommon) CalculateRates(ICollection<string> input)
    {
        var numberLength = input.First().Length;
        var numberCount = input.Count;


        var oneCountArray = new int[numberLength];

        foreach (var number in input)
        {
            var bits = number.ToCharArray();
            for(var i = 0; i<numberLength; i++)
            {
                if (bits[i] == '1')
                {
                    oneCountArray[i] += 1;
                } 
            }
        }
        
        var mostCommon = oneCountArray.Select(x => x >= (numberCount / 2.0) ? '1' : '0').ToArray();
        var leastCommon = oneCountArray.Select(x => x < (numberCount / 2.0) ? '1' : '0').ToArray();
        return (mostCommon, leastCommon);
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        var (mostCommon, leastCommon) = CalculateRates(input.ToArray());
        var gammaRate = string.Join("", mostCommon);
        var epsilonRate = string.Join("", leastCommon);

        return (Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2)).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputArray = input.ToArray();
        var numberLength = inputArray.First().Length;

        var oxygenPotentials = inputArray.Select(x => x).ToArray();
        for (var bit = 0; bit <numberLength; bit ++)
        {
            var (mostCommon, _) = CalculateRates(oxygenPotentials);
            oxygenPotentials = oxygenPotentials.Where(x => x[bit] == mostCommon[bit]).ToArray();
            if (oxygenPotentials.Length == 1)
            {
                break;
            }
        }

        var co2Potentials = inputArray.Select(x => x).ToArray();
        for (var bit = 0; bit <numberLength; bit ++)
        {
            var (_, leastCommon) = CalculateRates(co2Potentials);
            co2Potentials = co2Potentials.Where(x => x[bit] == leastCommon[bit]).ToArray();
            if (co2Potentials.Length == 1)
            {
                break;
            }
        }

        var oxygenRate = Convert.ToInt32(string.Join("", oxygenPotentials.Single()), 2);
        var co2Rate = Convert.ToInt32(string.Join("", co2Potentials.Single()), 2);

        return (oxygenRate * co2Rate).ToString();

    }

    public int Day => 03;
}
