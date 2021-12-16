using System.Globalization;

namespace AdventOfCode.Days;

public class Day16 : ISolution
{
    public enum Type
    {
        Literal,
        Operator
    };
    
    private Dictionary<char, int[]> hexMap = new Dictionary<char, int[]>
    {
        { '0', new[] { 0, 0, 0, 0 } },
        { '1', new[] { 0, 0, 0, 1 } },
        { '2', new[] { 0, 0, 1, 0 } },
        { '3', new[] { 0, 0, 1, 1 } },
        { '4', new[] { 0, 1, 0, 0 } },
        { '5', new[] { 0, 1, 0, 1 } },
        { '6', new[] { 0, 1, 1, 0 } },
        { '7', new[] { 0, 1, 1, 1 } },
        { '8', new[] { 1, 0, 0, 0 } },
        { '9', new[] { 1, 0, 0, 1 } },
        { 'A', new[] { 1, 0, 1, 0 } },
        { 'B', new[] { 1, 0, 1, 1 } },
        { 'C', new[] { 1, 1, 0, 0 } },
        { 'D', new[] { 1, 1, 0, 1 } },
        { 'E', new[] { 1, 1, 1, 0 } },
        { 'F', new[] { 1, 1, 1, 1 } }
    };
    
    public string PartOne(IEnumerable<string> input)
    {
        var transmission = input.First().SelectMany(x => hexMap[x]).ToList();

        return VersionSum(0, transmission).Item1.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        
        throw new NotImplementedException();
    }

    public (int version, ICollection<int> rest) ParseVersion(ICollection<int> transmission)
    {
        return (Convert.ToInt32(string.Join("", transmission.Take(3)), 2), transmission.Skip(3).ToList());
    }

    public (Type type, ICollection<int> rest) ParseType(ICollection<int> transmission)
    {
        var typeInt = Convert.ToInt32(string.Join("", transmission.Take(3)), 2);
        var type = typeInt switch
        {
            4 => Type.Literal,
            _ => Type.Operator
        };
        
        return (type, transmission.Skip(3).ToList());
    }

    public (int version, Type type, ICollection<int> rest) ParseHeader(ICollection<int> transmission)
    {
        var (version, r) = ParseVersion(transmission);
        var (type, rest) = ParseType(r);

        return (version, type, rest);
    }

    public (int lengthType, ICollection<int> rest) ParseLenghtType(ICollection<int> transmission)
    {
        return (transmission.Take(1).Single(), transmission.Skip(1).ToList());
    }

    public (int bitsInSubPacket, ICollection<int> rest) Parse0LengthType(ICollection<int> transmission)
    {
        return (Convert.ToInt32(string.Join("", transmission.Take(15).ToList()),2), transmission.Skip(15).ToList());
    } 
    
    public (int bitsInSubPacket, ICollection<int> rest) Parse1LengthType(ICollection<int> transmission)
    {
        return (Convert.ToInt32(string.Join("", transmission.Take(11).ToList()),2), transmission.Skip(11).ToList());
    }

    public (int, ICollection<int>) VersionSum(int versionSum, ICollection<int> transmission)
    {
        if (transmission.All(x => x == 0))
        {
            return (versionSum, new List<int>());
        }
        
        (var version, var type, transmission) = ParseHeader(transmission);

        if (type == Type.Literal)
        {
             (var literalValue, transmission) = ParseLiteralValue(transmission);
             return VersionSum(versionSum + version, transmission);
        }
        else
        {
             (var lengthType, transmission) = ParseLenghtType(transmission);
             if (lengthType == 0)
             {
                 (var numberOfBits, transmission) = Parse0LengthType(transmission);
             }
             else
             {
                 (var numberOfSubPackets, transmission) = Parse1LengthType(transmission);
             }

             return VersionSum(versionSum + version, transmission);
        }


    }


    public (long number, ICollection<int> rest) ParseLiteralValue(ICollection<int> transmission)
    {
        bool lastDigit = false;
        int num = 0;
        var binaryNumber = new List<int>();

        while (!lastDigit)
        {
            var section = transmission.Skip(num * 5).Take(5).ToList();
            lastDigit = section[0] == 0 ? true : false;
            
            binaryNumber.AddRange(section.Skip(1));
            num++;
        }

        var decimalNumber = Convert.ToInt64(string.Join("", binaryNumber), 2);

        var bitsUsed = num * 5;

        return (decimalNumber, transmission.Skip(bitsUsed).ToList());
    }
    
    
    public int Day => 16;
}
