namespace advent;

public class Pipes
{
    const string inputFileS1 = @"C:\dev\src\nash\AdventOfCode\2023\10\input-sample-1.txt";
    const string inputFileS2 = @"C:\dev\src\nash\AdventOfCode\2023\10\input-sample-2.txt";
    const string inputFile = @"C:\dev\src\nash\AdventOfCode\2023\10\input-actual.txt";

    public static void Main()
    {
        Console.WriteLine("Sample Data 1:");
        (new Pipes()).Problem1(inputFileS1);
        // (new Pipes()).Problem2(inputFileS1);

        Console.WriteLine("Sample Data 2:");
        (new Pipes()).Problem1(inputFileS2);

        // Console.WriteLine("Actual Data");
        // (new Pipes()).Problem1(inputFile);
    }


    List<char[]> pipes = new List<char[]>();
    int maxX, maxY;
    public void Problem1(string filename)
    {
        long answer = 0;

        int lineNum = 0;
        Coord start = new Coord(-1,-1);

        using (var rdr = File.OpenText(filename))
        {
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine();
                pipes.Add(line.ToCharArray());

                int sPos = line.IndexOf('S');
                if (sPos > -1)
                {
                    start.X = sPos;
                    start.Y = lineNum;
                    Console.WriteLine($"S is at {start.X}, {start.Y}");
                }
                lineNum++;
                maxX = Math.Max(maxX, line.Count()-1);
            }
            maxY = lineNum-1;
        }

        List<Coord> firstSteps = new List<Coord>();

        if (start.X > 0)
        {
            char c = pipes.charAt(start.X-1, start.Y);
            if (c == 'L' || c == 'F')
            {
                firstSteps.Add(new Coord(start.X-1, start.Y));
            }
        }
        if (start.Y > 0)
        {
            char c = pipes.charAt(start.X, start.Y-1);
            if (c == '7' || c == 'F')
            {
                firstSteps.Add(new Coord(start.X, start.Y-1));
            }
        }
        if (start.X < maxX)
        {
            char c = pipes.charAt(start.X+1, start.Y);
            if (c == '7' || c == 'J')
            {
                firstSteps.Add(new Coord(start.X+1, start.Y));
            }
        }
        if (start.Y < maxY)
        {
            char c = pipes.charAt(start.X, start.Y+1);
            if (c == 'J' || c == 'L')
            {
                firstSteps.Add(new Coord(start.X, start.Y+1));
            }
        }

        List<Coord> path1 = new List<Coord>() { start, firstSteps.First() };
        List<Coord> path2 = new List<Coord>() { start, firstSteps.Last() };

        while (!path1.Last().Equal(path2.Last()))
        {
            takeAStep(path1);
            takeAStep(path2);
        }

        answer = path1.Count - 1;

        Console.WriteLine($"Problem 1: {answer}");
    }

    private void takeAStep(List<Coord> path)
    {
    }

    private bool areConnected(Coord c1, Coord c2)
    {
        if (c1.X < 0 || c2.X < 0 || c1.X > maxX || c2.X > maxX 
            || c1.Y < 0 || c2.Y < 0 || c1.Y > maYY || c2.Y > maYY) return false;


        // if (start.X > 0)
        // {
        //     char c = pipes.charAt(start.X-1, start.Y);
        //     if (c == 'L' || c == 'F')
        //     {
        //         firstSteps.Add(new Coord(start.X-1, start.Y));
        //     }
        // }
        // if (start.Y > 0)
        // {
        //     char c = pipes.charAt(start.X, start.Y-1);
        //     if (c == '7' || c == 'F')
        //     {
        //         firstSteps.Add(new Coord(start.X, start.Y-1));
        //     }
        // }
        // if (start.X < maxX)
        // {
        //     char c = pipes.charAt(start.X+1, start.Y);
        //     if (c == '7' || c == 'J')
        //     {
        //         firstSteps.Add(new Coord(start.X+1, start.Y));
        //     }
        // }
        // if (start.Y < maxY)
        // {
        //     char c = pipes.charAt(start.X, start.Y+1);
        //     if (c == 'J' || c == 'L')
        //     {
        //         firstSteps.Add(new Coord(start.X, start.Y+1));
        //     }
        // }

    }
}

public static class Extensions
{
    public static char charAt(this List<char[]> pipes, int x, int y) 
    {
        return pipes[y][x];
    }

    public static T nextToLast<T>(List<T> list)
    {
        return list[list.Count-2];
    }

}

public class Coord
{
    public int X {get;set;}
    public int Y {get;set;}

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equal(Coord other)
    {
        return this.X == other.X && this.Y == other.Y;
    }
}