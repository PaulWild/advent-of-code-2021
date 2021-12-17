namespace AdventOfCode.Days;

public class Day16 : ISolution
{
    public enum Type
    {
        Literal,
        Operator
    };
    
    private Dictionary<char, int[]> hexMap = new()
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

        var packet = Parse(transmission);
        return packet.VersionSum.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        
        throw new NotImplementedException();
    }

    public int  ParseVersion(List<int> transmission)
    {
        var version = Convert.ToInt32(string.Join("", transmission.Take(3)), 2);
        transmission.RemoveRange(0,3);
        return version;
    }

    public Type ParseType(List<int> transmission)
    {
        var typeInt = Convert.ToInt32(string.Join("", transmission.Take(3)), 2);
        var type = typeInt switch
        {
            4 => Type.Literal,
            _ => Type.Operator
        };
        
        transmission.RemoveRange(0, 3);

        return type;
    }

    public (int version, Type type) ParseHeader(List<int> transmission)
    {
        var version  = ParseVersion(transmission);
        var type = ParseType(transmission);

        return (version, type);
    }

    public int ParseLenghtType(List<int> transmission)
    {
        var lengthType = transmission.Take(1).Single();
        transmission.RemoveRange(0, 1);
        return lengthType;
    }

    public int  Parse0LengthType(List<int> transmission)
    {
        var length = Convert.ToInt32(string.Join("", transmission.Take(15).ToList()), 2);
        transmission.RemoveRange(0, 15);

        return length;
    } 
    
    public int Parse1LengthType(List<int> transmission)
    {
        var count = Convert.ToInt32(string.Join("", transmission.Take(11).ToList()), 2);
        transmission.RemoveRange(0, 11);

        return count;
    }
    
    public IPacket Parse(List<int> transmission)
    {
        var (version, type) = ParseHeader(transmission);
        
        if (type == Type.Literal)
        {
            var literalValue = ParseLiteralValue(transmission);
            return new Literal(version, type, literalValue);
        }
        else
        {
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
                 for (int i = 0; i < numberOfSubPackets; i++)
                 {
                     packet.AddNode(Parse(transmission));
                 }
                 
              }

              return packet;
        }
    }
    
    public long ParseLiteralValue(List<int> transmission)
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
        transmission.RemoveRange(0,bitsUsed);

        return decimalNumber;
    }

    public interface IPacket
    {
        public int Version { get; }
        public Type Type { get; }
        
        public IReadOnlyList<IPacket> Nodes { get; }
        
        public int VersionSum { get; }
    }

    public class Literal : IPacket
    {
        public Literal(int version, Type type, long literalValue)
        {
            Type = type;
            Version = version;
            LiteralValue = literalValue;
        }

        public int Version { get; }
        public Type Type { get; }

        public IReadOnlyList<IPacket> Nodes => new List<IPacket>();
        public int VersionSum => Version;

        public long LiteralValue { get; }
    }
    
    public class Operator : IPacket
    {

        private List<IPacket> _nodes = new List<IPacket>();
        public Operator(int version, Type type)
        {
            Type = type;
            Version = version;
        }

        public int Version { get; }
        public Type Type { get; }
        public void AddNode(IPacket node)
        {
            _nodes.Add(node);
        }
        public IReadOnlyList<IPacket> Nodes => _nodes;
        public int VersionSum =>  Version + Nodes.Select(x => x.VersionSum).Sum();
    }

    
    public int Day => 16;
}
