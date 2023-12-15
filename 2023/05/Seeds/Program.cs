



public class Seeds
{
    //const string inputFileName = @"C:\\dev\\src\\nash\\advent\\20231205\\Seeds\\input-sample.txt";
    const string inputFileName = @"C:\dev\src\nash\advent\20231205\input-actual.txt";

    void Problem1()
    {
        solveIt(line =>  line.Substring(7).Split().Select(str => long.Parse(str)));
    }

    void Problem2()
    {
        solveIt(p2seedLineParser);
    }

    IEnumerable<long> p2seedLineParser(string line)
    {
        var seedTokens = line.Substring(7).Split().Select(str => long.Parse(str)).ToList();
        var result = new List<long>();
        for (var i = 0; i < seedTokens.Count(); i+=2) {
            for (long j = seedTokens[i]; j <= seedTokens[i] + seedTokens[i+1]; j++)
            {
                yield return j;
            }
        }
    }

    void solveIt(Func<string, IEnumerable<long>> seedLineParser)
    {
        var maps = new List<List<mapEntry>>();
        IEnumerable<long> seeds = new List<long>();

        using (var rdr = File.OpenText(inputFileName))
        {
            var line = rdr.ReadLine();

            seeds = seedLineParser(line ?? "");

            var curmap = new List<mapEntry>();

            while (!rdr.EndOfStream)
            {
                line = rdr.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                if (line.EndsWith(":"))
                {
                    curmap = new List<mapEntry>();
                    maps.Add(curmap);
                    continue;
                }

                var lineVals = line.Split().Select(str => long.Parse(str)).ToList();
                curmap.Add(new mapEntry
                {
                    d = lineVals[0],
                    s = lineVals[1],
                    l = lineVals[2]
                });
            }
        }

        var answer = long.MaxValue;

        foreach (var seed in seeds)
        {
            var curVal = seed;
            foreach (var map in maps)
            {
                curVal = doLookup(map, curVal);
            }
            answer = Math.Min(curVal, answer);
        }

        Console.WriteLine(answer);
    }


    long doLookup(List<mapEntry> map, long key)
    {
        var x = map.FirstOrDefault(me => me.s <= key && key <= me.s + me.l);
        if (x == null)
        {
            return key;
        }
        else
        {
            return (key - x.s) + x.d;
        }
    }
    public static void Main()
    {
        var seeds = new Seeds();
        seeds.Problem1();
        seeds.Problem2();
    }
}

class mapEntry
{
    public long d, s, l;
}

