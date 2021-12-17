namespace AdventOfCode.Days;

public class Day17 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var range = input.Select(x => x.Split(',')).First().Select(int.Parse).ToList();

        var xTargetMin = range[0];
        var xTargetMax = range[1];
        var yTargetMin = range[2];
        var yTargetMax = range[3];

        var validYs = new List<int>();
        
        for (var x = 0; x < 200; x++)
        for (var y = 0; y < 200; y++)
        {
            var previousYs = new List<int>();
            var newX = x;
            var newY = y;
            var (xPos, yPos)  = (0, 0);
            for (;;)
            {
                xPos += newX;
                yPos += newY;

                switch (newX)
                {
                    case > 0:
                        newX--;
                        break;
                    case < 0:
                        newX++;
                        break;
                }

                
                
                newY--;
                previousYs.Add(yPos);

                if ((xPos >= xTargetMin && xPos <= xTargetMax) && (yPos >= yTargetMin && yPos <= yTargetMax))
                {
                    validYs.Add(previousYs.Max());
                }
                
                if (xPos > xTargetMax || yPos < yTargetMax)
                {
                    break;
                }
            }
        }

        return validYs.Max().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var range = input.Select(x => x.Split(',')).First().Select(int.Parse).ToList();

        var xTargetMin = range[0];
        var xTargetMax = range[1];
        var yTargetMin = range[2];
        var yTargetMax = range[3];
     
        var validTrajectories = new HashSet<(int, int)>();
        for (var x = -200; x < 200; x++)
        for (var y = -200; y < 200; y++)
        {
            var newX = x;
            var newY = y;
            var (xPos, yPos)  = (0, 0);

            for (;;)
            {
                xPos += newX;
                yPos += newY;

                switch (newX)
                {
                    case > 0:
                        newX--;
                        break;
                    case < 0:
                        newX++;
                        break;
                }

                
                
                newY--;

                if ((xPos >= xTargetMin && xPos <= xTargetMax) && (yPos <= yTargetMax && yPos >= yTargetMin))
                {
                    validTrajectories.Add((x, y));
                }
                
                if (xPos > xTargetMax || yPos < yTargetMin)
                {
                    break;
                }
            }
        }
        
        return validTrajectories.Count.ToString();

    }

    public int Day => 17;
}
