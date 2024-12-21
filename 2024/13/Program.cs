using System.Text.RegularExpressions;

namespace advent;

public class Solver
{
    const string inputSample = @"C:\Workspaces\AdventOfCode\2024\13\input-sample";
    const string inputActual = @"C:\Workspaces\AdventOfCode\2024\13\input-actual";
    const int expectedSampleResult1 = 480;
    const int expectedSampleResult2 = 0;
    const int MAX_PRESSES = 100;

    public static void Main()
    {
        long answer;

        answer = new Solver().Problem1(inputSample);
        Console.WriteLine($"Problem 1 Sample: Expected: {expectedSampleResult1}, Observed: {answer}");
        if (answer != expectedSampleResult1) { return; }
        answer = new Solver().Problem1(inputActual);
        Console.WriteLine($"Problem 1 Actual: Observed: {answer}");

        answer = new Solver().Problem2(inputSample);
        Console.WriteLine($"Problem 2 Sample: Expected: {expectedSampleResult2}, Observed: {answer}");
        // if (answer != expectedSampleResult2) { return; }
        answer = new Solver().Problem2(inputActual);
        Console.WriteLine($"Problem 2 Actual: Observed: {answer}");
    }
    long Problem1(string inputFilePath)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        int result = 0;
        List<Specs> allSpecs = ReadSpecs(inputFilePath);
        foreach (var specs in allSpecs)
        {
            int best = int.MaxValue;
            for (var a = 0; a <= MAX_PRESSES; a++)
            {
                long x = a * specs.ButtonA.X;
                long y = a * specs.ButtonA.Y;
                for (var b = 0; b <= MAX_PRESSES; b++)
                {
                    // if (a + b > MAX_PRESSES) break;
                    long x2 = x + (b * specs.ButtonB.X);
                    long y2 = y + (b * specs.ButtonB.Y);
                    // Console.WriteLine($"A:{a} B:{b} ({x2} {y2}) ({specs.Prize.X} {specs.Prize.Y})");
                    if (x2 == specs.Prize.X && y2 == specs.Prize.Y)
                    {
                        var cost = (a * 3) + b;
                        best = int.Min(best, cost);
                        break;
                    }
                    if (x2 > specs.Prize.X || y2 > specs.Prize.Y) break;
                }
            }

            if (best < int.MaxValue)
            {
                result += best;
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return result;
    }

    long Problem2(string inputFilePath)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        long result = 0;
        List<Specs> allSpecs = ReadSpecs(inputFilePath);
        foreach (var specs in allSpecs)
        {
            specs.Prize.X += 10000000000000;
            specs.Prize.Y +=10000000000000;
            
            var a = ((specs.Prize.X*specs.ButtonB.Y)-(specs.Prize.Y*specs.ButtonB.X))/((specs.ButtonA.X*specs.ButtonB.Y)-(specs.ButtonA.Y*specs.ButtonB.X));

            var b = (specs.Prize.X - (a*specs.ButtonA.X)) / specs.ButtonB.X;

            bool works = (a*specs.ButtonA.X) + (b*specs.ButtonB.X) == specs.Prize.X
                && (a*specs.ButtonA.Y) + (b*specs.ButtonB.Y) == specs.Prize.Y;

            if (works)
            {
                result += (3*a) + b;
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return result;
    }

    List<Specs> ReadSpecs(string inputFilePath)
    {
        List<Specs> allSpecs = new List<Specs>();
        Specs currentSpecs = new Specs();
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(line))
                {
                    //there are blank lines between each spec
                    allSpecs.Add(currentSpecs);
                    currentSpecs = new Specs();
                }
                else
                {
                    string pattern = @"X.(\d+), Y.(\d+)";
                    Match match = Regex.Match(line, pattern);
                    if (line.StartsWith("Button A"))
                    {
                        currentSpecs.ButtonA = new Coords(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    }
                    if (line.StartsWith("Button B"))
                    {
                        currentSpecs.ButtonB = new Coords(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    }
                    if (line.StartsWith("Prize"))
                    {
                        currentSpecs.Prize = new Coords(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    }
                }
            }
        }
        allSpecs.Add(currentSpecs);
        return allSpecs;
    }
}

public class Specs
{
    public Coords? ButtonA { get; set; }
    public Coords? ButtonB { get; set; }
    public Coords? Prize { get; set; }
}


