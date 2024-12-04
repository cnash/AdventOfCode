using System.Collections.Generic;
using System.Linq;

namespace advent;

public class WordSearch
{
    const string inputSample = @"C:\dev\src\nash\AdventOfCode\2024\4\input\sample";
    const int expectedSampleCount = 18;
    const int expectedSampleCount2 = 9;

    const string inputActual = @"C:\dev\src\nash\AdventOfCode\2024\4\input\actual";
    public static void Main()
    {
        Console.Write("P1 Sample: ");
        int answer = new WordSearch().ReadPuzzle(inputSample).HowManyXmas();
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleCount)
        {
            throw new Exception($"Sample should have returned {expectedSampleCount}");
        }
        Console.Write("P1 Actual: ");
        answer = (new WordSearch()).ReadPuzzle(inputActual).HowManyXmas();
        Console.WriteLine($"{answer}");

        Console.Write("P2 Sample: ");
        answer = new WordSearch().ReadPuzzle(inputSample).HowManyXmas2();
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleCount2)
        {
            throw new Exception($"Sample should have returned {expectedSampleCount2}");
        }
        Console.Write("P2 Actual: ");
        answer = (new WordSearch()).ReadPuzzle(inputActual).HowManyXmas2();
        Console.WriteLine($"{answer}");

    }

    List<List<char>> PuzzleMap;
    int Height => PuzzleMap.Count;
    int Width => PuzzleMap.FirstOrDefault()?.Count ?? 0;

    WordSearch ReadPuzzle(string inputFilePath)
    {
        PuzzleMap = new List<List<char>>();
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine() ?? "";
                PuzzleMap.Add(line.ToCharArray().ToList());
            }
        }
        return this;
    }

    int HowManyXmas()
    {
        var possibilities = new List<Possibility>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
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
            .Where(p => p.Letter(PuzzleMap) == 'X')
            .Where(p => !p.IsDeadEnd(Height, Width))
            .Select(p => p.Next())
            .Where(p => p.Letter(PuzzleMap) == 'M')
            .Where(p => !p.IsDeadEnd(Height, Width))
            .Select(p => p.Next())
            .Where(p => p.Letter(PuzzleMap) == 'A')
            .Where(p => !p.IsDeadEnd(Height, Width))
            .Select(p => p.Next())
            .Where(p => p.Letter(PuzzleMap) == 'S')
            .Count();

        return matchCount;
    }

    int HowManyXmas2()
    {
        var possibilities = new List<Possibility>();
        for (var y = 1; y < Height - 1; y++)
        {
            for (var x = 1; x < Width - 1; x++)
            {
                possibilities.Add(new Possibility
                {
                    Coords = new Point(x, y)
                });
            }
        }

        // M.M S.M S.S M.S
        // .A. .A. .A. .A.
        // S.S S.M M.M M.S

        possibilities = possibilities.Where(p => p.Letter(PuzzleMap) == 'A').ToList();
        var matches = possibilities.Where(p =>
        {
            string letters = "";
            p.Direction = Direction.UpLeft;
            letters += p.Next().Letter(PuzzleMap);
            p.Direction = Direction.UpRight;
            letters += p.Next().Letter(PuzzleMap);
            p.Direction = Direction.DownRight;
            letters += p.Next().Letter(PuzzleMap);
            p.Direction = Direction.DownLeft;
            letters += p.Next().Letter(PuzzleMap);

            List<string> acceptable = ["SSMM", "MSSM", "MMSS", "SMMS"];
            return acceptable.Contains(letters);
        }
        ).ToList();

        return matches.Count;
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


