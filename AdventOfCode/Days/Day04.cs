using AdventOfCode.Days.Day04Util;

namespace AdventOfCode.Days;

public class Day04 : ISolution
{
    private static (List<BingoCard> cards, Queue<int> numbersToDraw) ParseInputData(IEnumerable<string> input)
    {
        var bingoCards = new List<BingoCard>();
        var data = input.ToList();
        var numbersToDraw = new Queue<int>(data.First().Split(",").Select(x => Convert.ToInt32(x)));

        var bingoCard = Array.Empty<CardNumber[]>();
        foreach (var row in data.Skip(2))
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                bingoCards.Add(new BingoCard(bingoCard));
                bingoCard = new CardNumber[][] { };
            }
            else
            {
                var bingoRow = row.Split(" ")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => new CardNumber { Number = Convert.ToInt32(x), Marked = false })
                    .ToArray();

                bingoCard = bingoCard.Append(bingoRow).ToArray();
            }
        }

        //don't forget the last bingo card
        bingoCards.Add(new BingoCard(bingoCard));
        return (bingoCards, numbersToDraw);
    }

    
    public string PartOne(IEnumerable<string> input)
    {
        var (bingoCards, numbersToDraw) = ParseInputData(input);

        while (numbersToDraw.Count > 0)
        {
            var drawnNumber = numbersToDraw.Dequeue();
            bingoCards.ForEach(x => x.MarkCard(drawnNumber));

            foreach (var card in bingoCards.Where(card => card.HasCompleteColumn || card.HasCompleteRow))
            {
                return (card.UnMarkedTotal * drawnNumber).ToString();
            }
        }
        throw new NotImplementedException();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (bingoCards, numbersToDraw) = ParseInputData(input);

        while (numbersToDraw.Count > 0)
        {
            var drawnNumber = numbersToDraw.Dequeue();
            bingoCards.ForEach(x => x.MarkCard(drawnNumber));

            var toRemove = new List<BingoCard>();
            foreach (var card in bingoCards.Where(card => card.HasCompleteColumn || card.HasCompleteRow))
            {
                if (bingoCards.Count > 1)
                {
                    toRemove.Add(card);
                }
                else
                {
                    return (card.UnMarkedTotal * drawnNumber).ToString();
                }
            }
            toRemove.ForEach(x => bingoCards.Remove(x));
        }
        throw new NotImplementedException();
    }

    public int Day => 04;
}


