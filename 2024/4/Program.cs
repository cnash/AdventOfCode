namespace advent;

public class WordSearch
{
    const string inputSample = @"C:\dev\src\nash\AdventOfCode\2024\4\input\sample";
    const int expectedSampleCount = 18;

    const string inputActual = @"C:\dev\src\nash\AdventOfCode\2024\4\input\actual";
    public static void Main()
    {
        Console.Write("Sample: ");
        int answer = (new WordSearch()).HowManyXmas(inputSample);
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleCount)
        {
            throw new Exception($"Sample should have returned {expectedSampleCount}");
        }
        Console.Write("Actual: ");
        answer = (new WordSearch()).HowManyXmas(inputActual);
        Console.WriteLine($"{answer}");
    }

    int HowManyXmas(string inputFilePath)
    {
        int xmasCount = 0;
        List<List<char>> puzzleMap = new List<List<char>>();
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine() ?? "";
                puzzleMap.Add(line.ToCharArray().ToList());
            }
        }
        int height = puzzleMap.Count();
        int width = puzzleMap.First().Count();

        var possibilities = new List<Possibility>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                possibilities.AddRange(
                    Enum.GetValues<Direction>()
                        .Select(d =>
                            new Possibility
                            {
                                Direction = d,
                                Coords = new Point(x, y)
                            }
                        )
                );
            }
        }

        var matchCount = possibilities
            .Where(p => p.Letter(puzzleMap) == 'X')
            .Where(p => !p.IsDeadEnd(height, width))
            .Select(p => p.Next())
            .Where(p => p.Letter(puzzleMap) == 'M')
            .Where(p => !p.IsDeadEnd(height, width))
            .Select(p => p.Next())
            .Where(p => p.Letter(puzzleMap) == 'A')
            .Where(p => !p.IsDeadEnd(height, width))
            .Select(p => p.Next())
            .Where(p => p.Letter(puzzleMap) == 'S')
            .Count();

        return matchCount;
    }
}

public enum Direction
{
    Up = 1,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x, int y) { X = x; Y = y; }
}

public class Possibility
{
    public Direction Direction { get; set; }
    public Point Coords { get; set; }
    public char Letter(List<List<char>> puzzleMap) {
        return puzzleMap[Coords.X][Coords.Y];
    }

    public bool IsDeadEnd(int height, int width)
    {
        var badDirs = new List<Direction>();
        if (Coords.X == 0)
        {
            badDirs.Add(Direction.DownLeft);
            badDirs.Add(Direction.Left);
            badDirs.Add(Direction.UpLeft);
        }
        if (Coords.X == width-1)
        {
            badDirs.Add(Direction.DownRight);
            badDirs.Add(Direction.Right);
            badDirs.Add(Direction.UpRight);
        }
        if (Coords.Y == 0)
        {
            badDirs.Add(Direction.UpLeft);
            badDirs.Add(Direction.Up);
            badDirs.Add(Direction.UpRight);
        }
        if (Coords.Y == height-1)
        {
            badDirs.Add(Direction.DownLeft);
            badDirs.Add(Direction.Down);
            badDirs.Add(Direction.DownRight);
        }
        badDirs = badDirs.Distinct().ToList();
        return badDirs.Contains(Direction);
    }

    public Possibility Next()
    {
        var next = new Possibility { Direction = this.Direction, Coords = new Point(Coords.X, Coords.Y) };
        switch (this.Direction)
        {
            case Direction.UpLeft:
                next.Coords.Y--;
                next.Coords.X--;
                break;
            case Direction.Up:
                next.Coords.Y--;
                break;
            case Direction.UpRight:
                next.Coords.Y--;
                next.Coords.X++;
                break;
            case Direction.Right:
                next.Coords.X++;
                break;
            case Direction.DownRight:
                next.Coords.Y++;
                next.Coords.X++;
                break;
            case Direction.Down:
                next.Coords.Y++;
                break;
            case Direction.DownLeft:
                next.Coords.Y++;
                next.Coords.X--;
                break;
            case Direction.Left:
                next.Coords.X--;
                break;
        }
        return next;
    }
}


