namespace AdventOfCode.Days.Day04Util;

public record struct CardNumber(int Number, bool Marked);
public class BingoCard
{
    private readonly CardNumber[][] _card;

    public BingoCard(CardNumber[][] card)
    {
        _card = card;
    }

    public void MarkCard(int number)
    {
        foreach (var row in _card)
            for (var column = 0; column < _card.Length; column++)
            {
                if (row[column].Number == number)
                {
                    row[column] = row[column] with { Marked = true };
                }
            }
    }

    public bool HasCompleteRow => _card
        .Select(row => row.All(x => x.Marked))
        .Any(rowComplete => rowComplete);


    public bool HasCompleteColumn
    {
        get
        {
            var rowLength = _card[0].Length;
            var columnLenght = _card.Length;

            for (var row = 0; row < rowLength; row++)
            {
                var columnComplete = true;
                for (var column = 0; column < columnLenght; column++)
                {
                    if (_card[column][row].Marked == false)
                    {
                        columnComplete = false;
                    }
                }

                if (columnComplete)
                {
                    return true;
                }
            }

            return false;
        }
    }
        
    public int UnMarkedTotal =>  (from row in _card 
        from column in row 
        where !column.Marked
        select column.Number).Sum();

}