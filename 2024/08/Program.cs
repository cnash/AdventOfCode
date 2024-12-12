using System;
using System.Diagnostics;

namespace advent;

// An antinode occurs at any point that is perfectly in line with two antennas of
// the same frequency - but only when one of the antennas is twice as far away as
// the other. This means that for any pair of antennas with the same frequency,
// there are two antinodes, one on either side of them.

public class AntinodeCounter
{
    const string inputSample = @"C:\dev\src\nash\AdventOfCode\2024\08\input-sample";
    const string inputActual = @"C:\dev\src\nash\AdventOfCode\2024\08\input-actual";
    const int expectedSampleResult1 = 14;
    const int expectedSampleResult2 = 34;

    internal int MaxX = 0;
    internal int MaxY = 0;

    public static void Main()
    {
        int answer;
        Console.Write("P1 Sample: ");
        answer = new AntinodeCounter().Problem1(inputSample);
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
        answer = new AntinodeCounter().Problem1(inputActual);
        Console.WriteLine($"{answer}");

        Console.Write("P2 Sample: ");
        answer = answer = new AntinodeCounter().Problem2(inputSample);
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
        answer = answer = new AntinodeCounter().Problem2(inputActual);
        Console.WriteLine($"{answer}");
    }

    int Problem1(string inputFilePath)
    {
        var antennae = new List<Antenna>();
        var lineNum = -1;
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                MaxX = line.Length-1;
                lineNum++;
                for (var i = 0; i <= MaxX; i++)
                {
                    char marker = line[i];
                    if (marker != '.')
                    {
                        antennae.Add(new Antenna { Symbol = line[i], Coords = new Coords { X = i, Y = lineNum } });
                    }
                }
            }
        }
        MaxY = lineNum;

        List<Coords> antinodes = new List<Coords>();
        var antennaeGroups = antennae.GroupBy(a => a.Symbol);
        foreach (var ag in antennaeGroups)
        {
            var aList = ag.ToList();
            ;
            for (var i = 0; i < aList.Count() - 1; i++)
            {
                var ant1 = aList[i];
                for (var j = i + 1; j < aList.Count(); j++)
                {
                    var ant2 = aList[j];
                    var deltaX = ant1.Coords.X - ant2.Coords.X;
                    var deltaY = ant1.Coords.Y - ant2.Coords.Y;
                    var aNode1 = new Coords { X = ant1.Coords.X + deltaX, Y = ant1.Coords.Y + deltaY };
                    List<Coords> aNodes = new List<Coords>();
                    if (isOnMap(aNode1))
                    {
                        aNodes.Add(aNode1);
                    }

                    var aNode2 = new Coords { X = ant2.Coords.X - deltaX, Y = ant2.Coords.Y - deltaY };
                    if (isOnMap(aNode2))
                    {
                        aNodes.Add(aNode2);
                    }

                    //ShowMap(aNodes, new List<Antenna> { ant1, ant2 });
                    antinodes.AddRange(aNodes.Where(aNode => !antinodes.Any(a => a.X == aNode.X && a.Y == aNode.Y)));
                }
            }
        }
        return antinodes.Count();
    }

    int Problem2(string inputFilePath)
    {

        var antennae = new List<Antenna>();
        var lineNum = -1;
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                MaxX = line.Length - 1;
                lineNum++;
                for (var i = 0; i <= MaxX; i++)
                {
                    char marker = line[i];
                    if (marker != '.')
                    {
                        antennae.Add(new Antenna { Symbol = line[i], Coords = new Coords { X = i, Y = lineNum } });
                    }
                }
            }
        }
        MaxY = lineNum;

        List<Coords> antinodes = new List<Coords>();
        var antennaeGroups = antennae.GroupBy(a => a.Symbol);
        foreach (var ag in antennaeGroups)
        {
            var aList = ag.ToList();
            ;
            for (var i = 0; i < aList.Count() - 1; i++)
            {
                var ant1 = aList[i];
                for (var j = i + 1; j < aList.Count(); j++)
                {
                    var ant2 = aList[j];
                    var deltaX = ant1.Coords.X - ant2.Coords.X;
                    var deltaY = ant1.Coords.Y - ant2.Coords.Y;
                    List<Coords> aNodes = new List<Coords>() { ant1.Coords, ant2.Coords };
                    var aNode = ant1.Coords;
                    while (isOnMap(aNode))
                    {
                        aNodes.Add(aNode);
                        aNode = new Coords { X = aNode.X + deltaX, Y = aNode.Y + deltaY };
                    }
                    aNode = ant2.Coords;
                    while (isOnMap(aNode))
                    {
                        aNodes.Add(aNode);
                        aNode = new Coords { X = aNode.X - deltaX, Y = aNode.Y - deltaY };
                    }

                    //ShowMap(aNodes, new List<Antenna> { ant1, ant2 });
                    antinodes.AddRange(aNodes.Where(aNode => !antinodes.Any(a => a.X == aNode.X && a.Y == aNode.Y)));
                }
            }
        }
        return antinodes.Count();
    }

    private bool isOnMap(Coords c)
    {
        return c.X >= 0 && c.X <= MaxX && c.Y >= 0 && c.Y <= MaxY;
    }

    private void ShowMap(IEnumerable<Coords> antinodes, IEnumerable<Antenna> antennae)
    {
        Console.WriteLine();
        Console.WriteLine();
        for (var y = 0; y <= MaxY; y++)
        {
            for (var x = 0; x <= MaxX; x++)
            {
                if (antinodes.Any(antinode => antinode.X == x && antinode.Y == y))
                {
                    Console.Write("#");
                }
                else
                {
                    var ant = antennae.FirstOrDefault(ant1 => ant1.Coords.X == x && ant1.Coords.Y == y);
                    if (ant != null)
                    {
                        Console.Write(ant.Symbol);
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine();
        }
        //Console.WriteLine("Press any key to continue.");
        //Console.ReadKey();
    }

}



public class Coords
{
    public int X { get; set; }
    public int Y { get; set; }

    public double DistanceFrom(int x, int y) {
        var deltaX = Math.Abs(X-x);
        var deltaY = Math.Abs(Y-y);
        var distance = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        return distance;
    }

    public Coords Clone() => new Coords { X = this.X, Y = this.Y };
}

public class Antenna
{
    public char Symbol { get; set; }
    public Coords Coords { get; set; }
}


public static class Extensions
{
    public static bool EqualFP(this double v1, double v2) => Math.Abs(v1 - v2) < 1e-6;
}