namespace advent;

public class Wasteland
{
    const string inputFileS1 = @"C:\dev\src\nash\AdventOfCode\2023\08\input-sample-1.txt";
    const string inputFileS2 = @"C:\dev\src\nash\AdventOfCode\2023\08\input-sample-2.txt";
    const string inputFile = @"C:\dev\src\nash\AdventOfCode\2023\08\input-actual.txt";

    public static void Main()
    {
        // (new Wasteland()).Problem1(inputFileS1);
        // (new Wasteland()).Problem1(inputFile);
        // (new Wasteland()).Problem2(inputFileS2);
        (new Wasteland()).Test1();
        (new Wasteland()).Test2();
        (new Wasteland()).Test3();
    }

    public void Problem1(string filename)
    {
        var (instructions, network) = readInputFile(filename);
        string location = "AAA";
        long steps = 0;
        while (location != "ZZZ")
        {
            var entry = network[location];
            int idx = (int)steps % instructions.Length;
            if (instructions[idx] == 'L')
            {
                location = entry.Item1;
            }
            else
            {
                location = entry.Item2;
            }
            steps++;
        }

        Console.WriteLine($"Problem #1: {steps}");
    }

    public void Problem2(string filename)
    {
        var (instructions, network) = readInputFile(filename);
        Queue<string> locations = new Queue<string>(network.Keys.Where(k => k.EndsWith('A')));
        long steps = 0;
        bool goLeft = true;
        int numLocs = locations.Count;
        while (!locations.All(l => l[2] == 'Z')) //l.EndsWith('Z')))
        {
            int idx = (int)(steps % instructions.Length);
            goLeft = instructions[idx] == 'L';
            for (int i = 0; i < numLocs; i++)
            {
                var entry = network[locations.Dequeue()];
                if (goLeft)
                {
                    locations.Enqueue(entry.Item1);
                }
                else
                {
                    locations.Enqueue(entry.Item2);
                }
            }
            steps++;
        }
        Console.WriteLine($"Problem #2: {steps}");
    }

    public void Test1()
    {
        var (instructions, network) = readInputFile(inputFile);
        string location = network.Keys.First(k => k[2] == 'A'); //"AAA";
        long steps = 0;
        while (location[2] != 'Z')
        {
            var entry = network[location];
            int idx = (int)steps % instructions.Length;
            if (instructions[idx] == 'L')
            {
                location = entry.Item1;
            }
            else
            {
                location = entry.Item2;
            }
            steps++;
        }

        Console.WriteLine($"Test #1: {steps}");
    }

    public void Test2()
    {
        var (instructions, network) = readInputFile(inputFile);
        Queue<string> locations = new Queue<string>(network.Keys.Where(k => k.EndsWith('A')));
        int locNum = 0;
        bool goLeft = true;
        while (locations.Any()) 
        {
            long steps = 0;
            locNum++;
            var loc = locations.Dequeue();
            while (loc[2] != 'Z')
            {
                var entry = network[loc];
                int idx = (int)(steps % instructions.Length);
                goLeft = instructions[idx] == 'L';

                if (goLeft)
                {
                    loc = entry.Item1;
                }
                else
                {
                    loc = entry.Item2;
                }
                steps++;
            }
            Console.WriteLine($"Location #{locNum} - Steps to Z: {steps}");
        }
    }

    public void Test3()
    {
        var (instructions, network) = readInputFile(inputFile);
        var instructionsAsBools = instructions.Select(c => c == 'L').ToArray();
        Queue<string> locations = new Queue<string>(network.Keys.Where(k => k.EndsWith('A')));
        List<List<string>> startingLocs = new List<List<string>>();
        int numLocs = locations.Count;
        for (int i=0;i<numLocs;i++)
        {
            startingLocs.Add(new List<string>());
        }
        int locNum = 0;
        long steps = 0;
        while (!locations.All(l => l[2] == 'Z')) //l.EndsWith('Z')))
        {
            int idx = (int)(steps % instructions.Length);

            if (idx == 0) 
            {

                for (int i=0;i<numLocs;i++)
                {
                    var x = locations.Dequeue();
                    startingLocs[i].Add(x);
                    locations.Enqueue(x);
                }

                var allVisited = startingLocs.First();
                for (int i=1;i<startingLocs.Count;i++)
                {
                    allVisited = allVisited.Intersect(startingLocs[i]).ToList();
                }
                if (allVisited.Any())
                {
                    Console.WriteLine($"All paths have started at {string.Join(',', allVisited.ToArray())}");
                }
            }


            for (int i = 0; i < numLocs; i++)
            {
                var entry = network[locations.Dequeue()];
                if (instructionsAsBools[idx])
                {
                    locations.Enqueue(entry.Item1);
                }
                else
                {
                    locations.Enqueue(entry.Item2);
                }
            }
            steps++;
        }
        Console.WriteLine($"Answer: {steps}");
    }

    private (string, Dictionary<string, Tuple<string, string>>) readInputFile(string inputFile)
    {
        Dictionary<string, Tuple<string, string>> network = new Dictionary<string, Tuple<string, string>>();
        string instructions = "";
        using (var rdr = File.OpenText(inputFile))
        {
            instructions = rdr.ReadLine() ?? "";
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                var codes = line
                    .Replace('=', ' ')
                    .Replace('(', ' ')
                    .Replace(',', ' ')
                    .Replace(')', ' ')
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                network.Add(codes[0], new Tuple<string, string>(codes[1], codes[2]));
            }
        }
        return (instructions, network);
    }

}