namespace advent;

public class Boats
{
    const string inputFile = @"C:\dev\src\nash\advent\20231206\input-sample.txt";
    // const string inputFile = @"C:\dev\src\nash\advent\20231206\input-actual.txt";

    public static void Main()
    {
        (new Boats()).Problem1();
        (new Boats()).Problem2();
    }

    public void Problem1()
    {
        List<long> times, distances;

        using (var rdr = File.OpenText(inputFile))
        {
            var line = rdr.ReadLine();
            if (line == null) return;
            times = line[5..]
                .Split(' ', global::System.StringSplitOptions.RemoveEmptyEntries)
                .Select(str => long.Parse(str))
                .ToList();
            line = rdr.ReadLine();
            if (line == null) return;
            distances = line[9..]
                .Split(' ', global::System.StringSplitOptions.RemoveEmptyEntries)
                .Select(str => long.Parse(str))
                .ToList();            
        }
        if (times == null || distances == null) return;

        long result = 1;

        for (var i=0;i<times.Count();i++)
        {
            var time = times[i];
            var distance = distances[i];
            var ways = 0;
            for (var h=1;h<time;h++)
            {
                if (distance < (time - h) * h)
                {
                    ways++;
                }
            }
            result *= ways;
        }
        Console.WriteLine($"{result}");
    }

    public void Problem2()
    {
        List<long> times, distances;

        using (var rdr = File.OpenText(inputFile))
        {
            var line = rdr.ReadLine();
            if (line == null) return;
            times = line[5..]
                .Replace(" ", "")
                .Split(' ', global::System.StringSplitOptions.RemoveEmptyEntries)
                .Select(str => long.Parse(str))
                .ToList();
            line = rdr.ReadLine();
            if (line == null) return;
            distances = line[9..]
                .Replace(" ", "")
                .Split(' ', global::System.StringSplitOptions.RemoveEmptyEntries)
                .Select(str => long.Parse(str))
                .ToList();            
        }
        if (times == null || distances == null) return;

        long result = 1;

        for (var i=0;i<times.Count();i++)
        {
            var time = times[i];
            var distance = distances[i];
            var ways = 0;
            for (var h=1;h<time;h++)
            {
                if (distance < (time - h) * h)
                {
                    ways++;
                }
            }
            result *= ways;
        }
        Console.WriteLine($"{result}");
    }
    
}
    