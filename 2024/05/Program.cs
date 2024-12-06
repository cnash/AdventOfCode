namespace advent;

public class PrintQueue
{
    const string inputSample = @"C:\dev\src\nash\AdventOfCode\2024\05\input-sample";
    const string inputActual = @"C:\dev\src\nash\AdventOfCode\2024\05\input-actual";
    const int expectedSampleResult1 = 143;
    const int expectedSampleResult2 = 123;

    public static void Main()
    {
        Console.Write("P1 Sample: ");
        int answer = new PrintQueue().Problem1(inputSample);
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
        answer = new PrintQueue().Problem1(inputActual);
        Console.WriteLine($"{answer}");

        Console.Write("P2 Sample: ");
        answer = answer = new PrintQueue().Problem2(inputSample);
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
        answer = answer = new PrintQueue().Problem2(inputActual);
        Console.WriteLine($"{answer}");
    }

    int Problem1(string inputFilePath)
    {
        bool finishedReadingRules = false;
        var rules = new List<Tuple<int, int>>();
        var updates = new List<List<int>>();
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                if (string.IsNullOrEmpty(line)) 
                { 
                    finishedReadingRules = true; 
                    continue; 
                }
                if (!finishedReadingRules)
                {
                    var nums = line.Split("|").Select(x=>int.Parse(x)).ToList();
                    rules.Add(new Tuple<int, int>(nums[0], nums[1]));
                }
                else
                {
                    updates.Add(line.Split(",").Select(x => int.Parse(x)).ToList());
                }
            }
        }

        int total = updates
            .Where(update => isCorrect(update, rules))
            .Select(middleValue)
            .Sum();

        return total;
    }

    bool isCorrect(List<int> update, List<Tuple<int, int>> rules)
    {
        foreach (var rule in rules)
        {
            var a = update.IndexOf(rule.Item1);
            var b = update.IndexOf(rule.Item2);
            if (a == -1 || b == -1) continue;
            if (b < a) return false;
        }
        return true;
    }

    int middleValue(List<int> update)
    {
        return update[update.Count() / 2]; 
    }

    int Problem2(string inputFilePath)
    {
        bool finishedReadingRules = false;
        var rules = new List<Tuple<int, int>>();
        var updates = new List<List<int>>();
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    finishedReadingRules = true;
                    continue;
                }
                if (!finishedReadingRules)
                {
                    var nums = line.Split("|").Select(x => int.Parse(x)).ToList();
                    rules.Add(new Tuple<int, int>(nums[0], nums[1]));
                }
                else
                {
                    updates.Add(line.Split(",").Select(x => int.Parse(x)).ToList());
                }
            }
        }

        int total = updates
            .Where(update => !isCorrect(update, rules))
            .Select(x => fixUpdate(x, rules))
            .Select(middleValue)
            .Sum();

        return total;
    }

    List<int> fixUpdate(List<int> update, List<Tuple<int, int>> rules)
    {
        foreach (var rule in rules)
        {
            var a = update.IndexOf(rule.Item1);
            var b = update.IndexOf(rule.Item2);
            if (a == -1 || b == -1) continue;
            if (b < a)
            {
                var buffer = update[a];
                update[a] = update[b];
                update[b] = buffer;
            }
        }
        if (isCorrect(update, rules)) { return update; }
        else return (fixUpdate(update, rules));
    }

}

