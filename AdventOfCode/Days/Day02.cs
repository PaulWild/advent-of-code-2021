namespace AdventOfCode.Days;

public class Day02 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        long horizontalDistance = 0;
        long verticalDistance = 0;

        foreach (var unformattedCommand in input)
        {
            var split = unformattedCommand.Split(' ');
            var direction = split[0];
            var distance = Convert.ToInt64(split[1]);

            switch (direction)
            {
                case "forward":
                {
                    horizontalDistance += distance;
                    break;
                }
                case "down":
                {
                    verticalDistance += distance;
                    break;
                }
                case "up":
                {
                    verticalDistance -= distance;
                    break;
                }
                default:
                {
                    throw new Exception();
                }
            }
        }
        
        return (verticalDistance * horizontalDistance).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        long horizontalDistance = 0;
        long verticalDistance = 0;
        long aim = 0;

        foreach (var unformattedCommand in input)
        {
            var split = unformattedCommand.Split(' ');
            var direction = split[0];
            var distance = Convert.ToInt64(split[1]);

            switch (direction)
            {
                case "forward":
                {
                    horizontalDistance += distance;
                    verticalDistance += (aim * distance);
                    break;
                }
                case "down":
                {
                    aim += distance;
                    break;
                }
                case "up":
                {
                    aim -= distance;
                    break;
                }
                default:
                {
                    throw new Exception();
                }
            }
        }
        
        return (verticalDistance * horizontalDistance).ToString();
    }

    public int Day => 02;
}
