using System.IO;
using System.Reflection;

namespace AdventOfCode
{
    public interface ISolution
    {
        string PartOne(string[] input);

        string PartTwo(string[] input);
        
        int Day { get; }

        string[] Input()
        {
            var padding = Day > 9 ? "" : "0";

            var filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? string.Empty, 
                $"Input/day_{padding}{Day}.txt");
            return File.ReadAllLines(filePath);
        }
    }
}