// See https://aka.ms/new-console-template for more information
namespace advent;

public class Oasis
{
    const string inputFileSample = @"C:\dev\src\nash\AdventOfCode\2023\09\input-sample.txt";
    const string inputFile = @"C:\dev\src\nash\AdventOfCode\2023\09\input-actual.txt";

    public static void Main()
    {
        Console.WriteLine("Sample Data:");
        (new Oasis()).Problem1(inputFileSample);
        (new Oasis()).Problem2(inputFileSample);

        Console.WriteLine("Actual Data");
        (new Oasis()).Problem1(inputFile);
        (new Oasis()).Problem2(inputFile);
    }

    public void Problem1(string filename)
    {
        try {
        long answer = 0;
        using (var rdr = File.OpenText(filename))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                var sequence = line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => long.Parse(str))
                    .ToList();
                long next = nextNumberInSequence(sequence);
                // Console.WriteLine($"Line: {line} - Next: {next}");
                answer += next;
            }
        }
        Console.WriteLine($"Problem 1: {answer}");
        } catch (Exception exc) {
            Console.WriteLine(exc.Message);
        }
    }

    private long nextNumberInSequence(List<long> sequence)
    {
        List<long> newSequence = new List<long>();
        for (var i=1; i<sequence.Count; i++)
        {
            newSequence.Add(sequence[i] - sequence[i-1]);
        }
        if (newSequence.All(x=>x==0))
        {
            return sequence.Last();
        } else {
            return sequence.Last() + nextNumberInSequence(newSequence);
        }
    }

    public void Problem2(string filename)
    {
        try {
        long answer = 0;
        using (var rdr = File.OpenText(filename))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                var sequence = line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => long.Parse(str))
                    .ToList();
                long next = previousNumberInSequence(sequence);
                // Console.WriteLine($"Line: {line} - Next: {next}");
                answer += next;
            }
        }
        Console.WriteLine($"Problem 1: {answer}");
        } catch (Exception exc) {
            Console.WriteLine(exc.Message);
        }
    }

    private long previousNumberInSequence(List<long> sequence)
    {
        List<long> newSequence = new List<long>();
        for (var i=1; i<sequence.Count; i++)
        {
            newSequence.Add(sequence[i] - sequence[i-1]);
        }
        if (newSequence.All(x=>x==0))
        {
            return sequence.First();
        } else {
            return sequence.First() - previousNumberInSequence(newSequence);
        }
    }
}
