namespace AdventOfCode.Days;

public class Day16 : ISolution
{
    private enum Type
    {
        Sum=0,
        Product=1,
        Minimum=2,
        Maximum=3,
        Literal=4,
        GreaterThan=5,
        LessThan=6,
        EqualTo=7
    };
    
    private readonly Dictionary<char, int[]> _hexMap = new()
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
        var transmission = input.First().SelectMany(x => _hexMap[x]).ToList();

        var packet = Parse(transmission);
        return packet.VersionSum.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var transmission = input.First().SelectMany(x => _hexMap[x]).ToList();

        var packet = Parse(transmission);
        return packet.Value.ToString();
    }

    private static int ParseVersion(List<int> transmission)
    {
        var version = Convert.ToInt32(string.Join("", transmission.Take(3)), 2);
        transmission.RemoveRange(0,3);
        return version;
    }

    private static Type ParseType(List<int> transmission)
    {
        var typeInt = Convert.ToInt32(string.Join("", transmission.Take(3)), 2);
        var type = (Type)Enum.ToObject(typeof(Type), typeInt);
        
        transmission.RemoveRange(0, 3);

        return type;
    }

    private static (int version, Type type) ParseHeader(List<int> transmission)
    {
        var version  = ParseVersion(transmission);
        var type = ParseType(transmission);

        return (version, type);
    }

    private static int ParseLenghtType(List<int> transmission)
    {
        var lengthType = transmission.Take(1).Single();
        transmission.RemoveRange(0, 1);
        return lengthType;
    }

    private static int Parse0LengthType(List<int> transmission)
    {
        var length = Convert.ToInt32(string.Join("", transmission.Take(15).ToList()), 2);
        transmission.RemoveRange(0, 15);

        return length;
    }

    private static int Parse1LengthType(List<int> transmission)
    {
        var count = Convert.ToInt32(string.Join("", transmission.Take(11).ToList()), 2);
        transmission.RemoveRange(0, 11);

        return count;
    }

    private IPacket Parse(List<int> transmission)
    {
        var (version, type) = ParseHeader(transmission);
        
        if (type == Type.Literal)
        {
            var literalValue = ParseLiteralValue(transmission);
            return new Literal(version, literalValue);
        }

        var lengthType = ParseLenghtType(transmission);
        var packet = new Operator(version, type);
            
        if (lengthType == 0)
        {
            var numberOfBits = Parse0LengthType(transmission);
            var transmissionLeft = transmission.Count;
            do
            {
                packet.AddNode(Parse(transmission));
            } while (transmissionLeft-transmission.Count != numberOfBits);
        }
        else
        {
            var numberOfSubPackets = Parse1LengthType(transmission);
            for (var i = 0; i < numberOfSubPackets; i++)
            {
                packet.AddNode(Parse(transmission));
            }
                 
        }

        return packet;
    }

    private static long ParseLiteralValue(List<int> transmission)
    {
        var lastDigit = false;
        var num = 0;
        var binaryNumber = new List<int>();

        while (!lastDigit)
        {
            var section = transmission.Skip(num * 5).Take(5).ToList();
            lastDigit = section[0] == 0;
            
            binaryNumber.AddRange(section.Skip(1));
            num++;
        }

        var decimalNumber = Convert.ToInt64(string.Join("", binaryNumber), 2);

        var bitsUsed = num * 5;
        transmission.RemoveRange(0,bitsUsed);

        return decimalNumber;
    }

    private interface IPacket
    {
        public int VersionSum { get; }
        
        public long Value { get;  }
    }

    private class Literal : IPacket
    {
        public Literal(int version, long literalValue)
        {
            Version = version;
            LiteralValue = literalValue;
        }

        private int Version { get; }

        public int VersionSum => Version;

        public long Value => LiteralValue;

        private long LiteralValue { get; }
    }

    private class Operator : IPacket
    {

        private readonly List<IPacket> _nodes = new();
        
        public Operator(int version, Type type)
        {
            Type = type;
            Version = version;
        }

        private int Version { get; }
        
        private Type Type { get; }
        
        public void AddNode(IPacket node)
        {
            _nodes.Add(node);
        }

        private IReadOnlyList<IPacket> Nodes => _nodes;
        
        public int VersionSum =>  Version + Nodes.Select(x => x.VersionSum).Sum();

        public long Value
        {
            get
            {
                return Type switch
                {
                    Type.Sum => Nodes.Select(x => x.Value).Sum(),
                    Type.Product => Nodes.Select(x => x.Value).Aggregate((agg,val) => agg * val),
                    Type.Minimum => Nodes.Select(x => x.Value).Min(),
                    Type.Maximum => Nodes.Select(x => x.Value).Max(),
                    Type.Literal => throw new ArgumentOutOfRangeException(),
                    Type.GreaterThan => Nodes[0].Value > Nodes[1].Value ? 1 : 0,
                    Type.LessThan => Nodes[0].Value < Nodes[1].Value ? 1 : 0,
                    Type.EqualTo => Nodes[0].Value == Nodes[1].Value ? 1 : 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    
    public int Day => 16;
}
