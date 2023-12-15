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
        while (locations.Any()) //l.EndsWith('Z')))
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
            Console.WriteLine($"Location #{locNum} - {steps}");
        }
    }

    public void Test3()
    {
        var (instructions, network) = readInputFile(inputFile);
        Queue<string> locations = new Queue<string>(network.Keys.Where(k => k.EndsWith('A')));
        int locNum = 0;
        bool goLeft = true;
        while (locations.Any())
        {
            long steps = 0;
            locNum++;
            var firstLoc = locations.Dequeue();
            var loc = firstLoc;
            int idx = -1;
            while (loc[2] != 'Z')
            {
                var entry = network[loc];
                idx = (int)(steps % instructions.Length);
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
            var stepsToZ = steps;
            while (!(idx == 0 && loc == firstLoc))
            {
                var entry = network[loc];
                idx = (int)(steps % instructions.Length);
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
            var stepsToLoop = steps;

            Console.WriteLine($"Location #{locNum} - {stepsToZ} - {stepsToLoop}");
        }
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