namespace advent;
public class Garden
{
    const string inputSample = @"..\..\..\input-sample";
    const string inputActual = @"..\..\..\input-actual";
    const int expectedSampleResult1 = 1930;
    const int expectedSampleResult2 = 1206;

    public static void Main()
    {
        long answer;

        answer = new Garden().Problem1(inputSample);
        Console.WriteLine($"Problem 1 Sample: Expected: {expectedSampleResult1}, Observed: {answer}");
        if (answer != expectedSampleResult1) { return; }
        answer = new Garden().Problem1(inputActual);
        Console.WriteLine($"Problem 1 Actual: Observed: {answer}");

        answer = new Garden().Problem2(inputSample);
        Console.WriteLine($"Problem 2 Sample: Expected: {expectedSampleResult2}, Observed: {answer}");
        if (answer != expectedSampleResult2) { return; }

        answer = new Garden().Problem2("../../../sample1");
        Console.WriteLine($"Problem 2 Sample: Expected: 80, Observed: {answer}");
        answer = new Garden().Problem2("../../../sample2");
        Console.WriteLine($"Problem 2 Sample: Expected: 236, Observed: {answer}");
        answer = new Garden().Problem2("../../../sample3");
        Console.WriteLine($"Problem 2 Sample: Expected: 368, Observed: {answer}");


        answer = new Garden().Problem2(inputActual);
        Console.WriteLine($"Problem 2 Actual: Observed: {answer}");
    }

    long Problem1(string inputFilePath)
    {
        var allRegions = FindAllRegions(inputFilePath);
        return allRegions.Select(r=>r.Cost).Sum();
    }

    long Problem2(string inputFilePath)
    {
        var allRegions = FindAllRegions(inputFilePath);
        return allRegions.Select(r=>r.BulkCost).Sum();
    }

    List<Region> FindAllRegions(string inputFilePath)
    {
        var allLines = File.ReadAllLines(inputFilePath);
        var garden = allLines.Select(line => line.ToCharArray()).ToArray();
        List<Region> allRegions = new List<Region>();

        var startPoint = new Coords(0,0);
        while (startPoint != null)
        {
            allRegions.Add(new Region(GetRegionCoords(startPoint, ref garden)));
            startPoint = FindNextRegion(garden);
        }
        return allRegions;
    }

    Coords FindNextRegion(char[][] garden)
    {
        for (var x=0;x<garden[0].Length;x++)
        {
            for (var y=0;y<garden.Length;y++)
            {
                if (garden[y][x] != ' ')
                {
                    return new Coords(x, y);
                }
            }
        }
        return null;
    }

    List<Coords> GetRegionCoords(Coords plot, ref char[][] garden)
    {
        char plotType = garden[plot.Y][plot.X];
        var result = new List<Coords> {plot};
        garden[plot.Y][plot.X] = ' ';
        if (plot.X> 0 && garden[plot.Y][plot.X-1]==plotType)
        {
            result.AddRange(GetRegionCoords(new Coords(plot.X-1, plot.Y), ref garden));
        }
        if (plot.Y> 0 && garden[plot.Y-1][plot.X]==plotType)
        {
            result.AddRange(GetRegionCoords(new Coords(plot.X, plot.Y-1), ref garden));
        }
        if (plot.X<garden[0].Length-1 && garden[plot.Y][plot.X+1]==plotType)
        {
            result.AddRange(GetRegionCoords(new Coords(plot.X+1, plot.Y), ref garden));
        }
        if (plot.Y<garden.Length-1 && garden[plot.Y+1][plot.X]==plotType)
        {
            result.AddRange(GetRegionCoords(new Coords(plot.X, plot.Y+1), ref garden));
        }
        return result;
    }
}

public class Coords
{
    public Coords(int x, int y)
    {
        X = x;
        Y = y;
    }

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

public class Region
{
    public Region(List<Coords> plots)
    {
        Plots = plots;
    }

    public List<Coords> Plots {get;}

    public int Area => Plots.Count;

    public int Perimeter
    {
        get
        {
            return
                Plots.Select(p=> 4 - (Plots.Count(p2=>((p2.X == p.X-1) || (p2.X==p.X+1)) && (p2.Y == p.Y))
                                    + Plots.Count(p2=>((p2.Y == p.Y-1) || (p2.Y==p.Y+1)) && (p2.X == p.X))))
                    .Sum();
        }
    }

    public int CornerCount(Coords p)
    {
        bool NW = Plots.Any(p2=>p2.X==p.X-1&&p2.Y==p.Y-1);
        bool N = Plots.Any(p2=>p2.X==p.X&&p2.Y==p.Y-1);
        bool NE = Plots.Any(p2=>p2.X==p.X+1&&p2.Y==p.Y-1);
        bool E = Plots.Any(p2=>p2.X==p.X+1&&p2.Y==p.Y);
        bool SE = Plots.Any(p2=>p2.X==p.X+1&&p2.Y==p.Y+1);
        bool S = Plots.Any(p2=>p2.X==p.X&&p2.Y==p.Y+1);
        bool SW = Plots.Any(p2=>p2.X==p.X-1&&p2.Y==p.Y+1);
        bool W = Plots.Any(p2=>p2.X==p.X-1&&p2.Y==p.Y);
        
        var count = 0;
        if (!N && !E) count++;
        if (!E && !S) count++;
        if (!S && !W) count++;
        if (!W && !N) count++;

        if (W && SW && !S) count++;
        if (S && SW && !W) count++;
        if (E && SE && !S) count++;
        if (S && SE && !E) count++;

        return count;
    }

    public int Cost => Area * Perimeter;

    public int BulkPerimeter
    {
        get {
            return Plots.Select(p=>CornerCount(p)).Sum();
        }
    }

    public int BulkCost => Area * BulkPerimeter;
}


