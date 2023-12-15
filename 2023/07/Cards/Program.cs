namespace advent;

public class Cards
{
    //const string inputFile = @"C:\dev\src\nash\advent\20231207\input-sample.txt";
    const string inputFile = @"C:\dev\src\nash\advent\20231207\input-actual.txt";

    public static void Main()
    {
        (new Cards()).Problem1();
        (new Cards()).Problem2();
    }

    public void Problem1()
    {
        long result = 0;
        List<Hand> hands = new List<Hand>();
        using (var rdr = File.OpenText(inputFile))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                hands.Add(new Hand(line));
            }
        }
        var orderedHands = hands.OrderBy(h => h.Strength).ToList();
        for (int rank = 1; rank <= orderedHands.Count; rank++)
        {
            result += rank * orderedHands[rank - 1].Bid;
        }
        Console.WriteLine($"{result}");
    }

    public void Problem2()
    {
        long result = 0;
        List<Hand> hands = new List<Hand>();
        using (var rdr = File.OpenText(inputFile))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                hands.Add(new Hand(line, true));
            }
        }
        var orderedHands = hands.OrderBy(h => h.Strength).ToList();
        for (int rank = 1; rank <= orderedHands.Count; rank++)
        {
            result += rank * orderedHands[rank - 1].Bid;
            Console.WriteLine($"{orderedHands[rank - 1]}");
        }
        Console.WriteLine($"{result}");
    }
}

class Hand
{
    public Hand(string inputLine, bool wildJacks = false)
    {
        var substrings = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Cards = substrings[0];
        Bid = int.Parse(substrings[1]);
        _wildJacks = wildJacks;
        if (_wildJacks) _cardValues['J'] = 0;
        _wilds = 0;
        _cardCounts = new Dictionary<char, int>();
        foreach (char c in this.Cards)
        {
            if (_cardCounts.ContainsKey(c))
            {
                _cardCounts[c]++;
            }
            else
            {
                _cardCounts[c] = 1;
            }
            if (_wildJacks && c == 'J')
            {
                _wilds++;
            }
        }
        Strength = getStrength();
    }

    public string Cards { get; set; } = "";
    public int Bid { get; set; }
    public long Strength { get; set; }
    private Dictionary<char, int> _cardCounts;
    private bool _wildJacks;
    private int _wilds;
    private int _rank;

    public override string ToString()
    {
        return $"{Cards} {_rank}";
    }


    Dictionary<char, int> _cardValues = new Dictionary<char, int> {
        {'2',1},
        {'3',2},
        {'4',3},
        {'5',4},
        {'6',5},
        {'7',6},
        {'8',7},
        {'9',8},
        {'T',9},
        {'J',10},
        {'Q',11},
        {'K',12},
        {'A',13}
    };

    private long getStrength()
    {
        _rank = 0;
        if (IsFiveOfAKind())
        {
            _rank = 6;
        }
        else if (IsFourOfAKind())
        {
            _rank = 5;
        }
        else if (IsFullHouse())
        {
            _rank = 4;
        }
        else if (IsThreeOfAKind())
        {
            _rank = 3;
        }
        else if (IsTwoPair())
        {
            _rank = 2;
        }
        else if (IsOnePair())
        {
            _rank = 1;
        }
        long result = _rank;

        foreach (char c in this.Cards)
        {
            result *= 14;
            result += _cardValues[c];
        }
        return result;
    }

    private bool IsFiveOfAKind()
    {
        return _cardCounts.Any(kvp => kvp.Value == 5 || (kvp.Key != 'J' && kvp.Value + _wilds == 5));
    }

    private bool IsFourOfAKind()
    {
        return _cardCounts.Any(kvp => kvp.Value == 4 || (kvp.Key != 'J' && kvp.Value + _wilds == 4));
    }

    private bool IsFullHouse()
    {
        if (_wilds >= 3)
        {
            return true;
        }
        if (_wilds == 2 && _cardCounts.Any(kvp => kvp.Key != 'J' && kvp.Value >= 2))
        {
            return true;
        }
        if (_wilds == 1 && _cardCounts.Count(kvp => kvp.Value >= 2) == 2)
        {
            return true;
        }
        if (_cardCounts.Any(kvp => kvp.Value == 2) && _cardCounts.Any(kvp => kvp.Value == 3))
        {
            return true;
        }
        return false;
    }

    private bool IsThreeOfAKind()
    {
        if (_wilds >= 2)
        {
            return true;
        }
        if (_wilds == 1 && _cardCounts.Any(kvp => kvp.Key != 'J' && kvp.Value >= 2))
        {
            return true;
        }
        return _cardCounts.Any(kvp => kvp.Value >= 3);
    }

    private bool IsTwoPair()
    {
        if (_wilds >= 2)
        {
            return true;
        }
        if (_wilds == 1 && _cardCounts.Any(kvp => kvp.Value == 2))
        {
            return true;
        }
        return _cardCounts.Count(kvp => kvp.Value >= 2) == 2;
    }

    private bool IsOnePair()
    {
        if (_wilds >= 1)
        {
            return true;
        }
        return _cardCounts.Any(kvp => kvp.Value >= 2);
    }

}