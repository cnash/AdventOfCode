namespace advent;
public class HoofIt
{
    const string inputSample = @"C:\Workspaces\AdventOfCode\2024\10\input-sample";
    const string inputActual = @"C:\Workspaces\AdventOfCode\2024\10\input-actual";
    const int expectedSampleResult1 = 36;
    const int expectedSampleResult2 = 81;

    internal int MaxX = 0;
    internal int MaxY = 0;

    internal List<List<int>> Map;

    public static void Main()
    {
        long answer;
        Console.Write("P1 Sample: ");
        answer = new HoofIt().Problem1(inputSample);
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleResult1)
        {
            throw new Exception($"Sample should have returned {expectedSampleResult1}");
        }
        else
        {
            Console.WriteLine("Sample is correct.");
        }

        Console.Write("P1 Actual: ");
        answer = new HoofIt().Problem1(inputActual);
        Console.WriteLine($"{answer}");

        Console.Write("P2 Sample: ");
        answer = answer = new HoofIt().Problem2(inputSample);
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleResult2)
        {
            throw new Exception($"Sample should have returned {expectedSampleResult2}");
        }
        else
        {
            Console.WriteLine("Sample is correct.");
        }
        Console.Write("P2 Actual: ");
        answer = answer = new HoofIt().Problem2(inputActual);
        Console.WriteLine($"{answer}");
    }

    long Problem1(string inputFilePath)
    {
        var result = 0;
        this.Map = ReadMap(inputFilePath);
        for (var y = 0; y <= MaxY; y++)
        {
            for (var x = 0; x <= MaxX; x++)
            {
                if (Map[y][x] == 0)
                {
                    result += scoreTrailHead(x, y);
                }
            }
        }
        return result;
    }

    long Problem2(string inputFilePath)
    {
        var result = 0;
        this.Map = ReadMap(inputFilePath);
        for (var y = 0; y <= MaxY; y++)
        {
            for (var x = 0; x <= MaxX; x++)
            {
                if (Map[y][x] == 0)
                {
                    result += rateTrailHead(x, y);
                }
            }
        }
        return result;
    }

    List<List<int>> ReadMap(string inputFilePath)
    {
        var result = new List<List<int>>();
        MaxY = -1;
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine() ?? "";
                MaxX = line.Length - 1;
                result.Add(line.ToCharArray().Select(c => int.Parse($"{c}")).ToList());
                MaxY++;
            }
        }
        return result;
    }
    int scoreTrailHead(int x, int y)
    {
        var uniqueSummits = new List<Coords>();
        var allSummits = reachableSummits(x, y);
        foreach (var s in allSummits)
        {
            if (!uniqueSummits.Any(us => us.X == s.X && us.Y == s.Y))
            {
                uniqueSummits.Add(s);
            }
        }
        return uniqueSummits.Count();
    }

    int rateTrailHead(int x, int y)
    {
        return reachableSummits(x, y).Count();
    }
    List<Coords> reachableSummits(int x, int y)
    {
        var result = new List<Coords>();
        var elevation = Map[y][x];
        if (elevation == 9)
        {
            result.Add(new Coords { X = x, Y = y });
        }
        else
        {
            if (x > 0 && Map[y][x - 1] - elevation == 1)
            {
                result.AddRange(reachableSummits(x - 1, y));
            }

            if (y > 0 && Map[y - 1][x] - elevation == 1)
            {
                result.AddRange(reachableSummits(x, y - 1));
            }

            if (x < MaxX && Map[y][x + 1] - elevation == 1)
            {
                result.AddRange(reachableSummits(x + 1, y));
            }

            if (y < MaxY && Map[y + 1][x] - elevation == 1)
            {
                result.AddRange(reachableSummits(x, y + 1));
            }
        }
        return result;
    }
}

public class Coords
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool Equals(Coords other)
    {
        if (Object.ReferenceEquals(other, null)) return false;
        if (Object.ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
}