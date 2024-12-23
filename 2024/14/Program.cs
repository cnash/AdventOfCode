using System.Text;
using System.Text.RegularExpressions;

namespace advent;

public class Solver
{
    const string inputSample = @"C:\Workspaces\AdventOfCode\2024\14\input-sample";
    const string inputActual = @"C:\Workspaces\AdventOfCode\2024\14\input-actual";
    const int expectedSampleResult1 = 12;
    const int expectedSampleResult2 = 0;

    public static void Main()
    {
        long answer;

        answer = new Solver().Problem1(inputSample);
        Console.WriteLine($"Problem 1 Sample: Expected: {expectedSampleResult1}, Observed: {answer}");
        if (answer != expectedSampleResult1) { return; }
        answer = new Solver().Problem1(inputActual);
        Console.WriteLine($"Problem 1 Actual: Observed: {answer}");

        // answer = new Solver().Problem2(inputSample);
        // Console.WriteLine($"Problem 2 Sample: Expected: {expectedSampleResult2}, Observed: {answer}");
        // if (answer != expectedSampleResult2) { return; }
        answer = new Solver().Problem2(inputActual);
        Console.WriteLine($"Problem 2 Actual: Observed: {answer}");
    }
    long Problem1(string inputFilePath)
    {
        List<Robot> robots;
        int maxX, maxY;
        (robots, maxX, maxY) = ReadData(inputFilePath);

        for (var i = 0; i < 100; i++)
        {
            MoveRobots(robots, maxX, maxY);
        }

        var q1 = robots.Count(r => r.Position.X < maxX / 2 && r.Position.Y < maxY / 2);
        var q2 = robots.Count(r => r.Position.X > (maxX / 2) && r.Position.Y < maxY / 2);
        var q3 = robots.Count(r => r.Position.X > (maxX / 2) && r.Position.Y > (maxY / 2));
        var q4 = robots.Count(r => r.Position.X < maxX / 2 && r.Position.Y > (maxY / 2));
        return q1 * q2 * q3 * q4;
    }

    long Problem2(string inputFilePath)
    {
        List<Robot> robots;
        int maxX, maxY;
        (robots, maxX, maxY) = ReadData(inputFilePath);

        long result = 0;
        // for (;result<8000;result++)
        // {
        //     MoveRobots(robots, maxX, maxY);
        // }

        do
        {
            if (result % 1000 == 0)
            {
                Console.Clear();
                Console.WriteLine($"Seconds: {result}");
            }
            result++;
            MoveRobots(robots, maxX, maxY);
            var map = GenerateMap(robots, maxX, maxY);
            var mapStr = MapToString(map);
            if (mapStr.Contains("###############"))
            {
                Console.Clear();
                Console.WriteLine($"Seconds: {result}");
                Console.WriteLine(mapStr);
                var k = Console.ReadKey();
                if (k.Key != ConsoleKey.Spacebar)
                {
                    break;
                }
            }
        } while (true);
        return result;
    }


    public (List<Robot>, int, int) ReadData(string inputFilePath)
    {
        List<Robot> robots = new List<Robot>();
        int maxX = 0;
        int maxY = 0;
        using (var rdr = File.OpenText(inputFilePath))
        {
            string line = rdr.ReadLine() ?? "";
            var dimensions = line.Split(' ').Select(int.Parse).ToArray();
            maxX = dimensions[0];
            maxY = dimensions[1];
            while (!rdr.EndOfStream)
            {
                line = rdr.ReadLine() ?? "";
                string pattern = @"p=(-?\d+),(-?\d+)\sv=(-?\d+),(-?\d+)";
                Match match = Regex.Match(line, pattern);
                robots.Add(new Robot
                {
                    Position = new Coords(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    Velocity = new Coords(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });
            }
        }
        return (robots, maxX, maxY);
    }

    public void MoveRobots(List<Robot> robots, int maxX, int maxY)
    {
        foreach (var robot in robots)
        {
            robot.Position.X = (robot.Position.X + robot.Velocity.X) % maxX;
            if (robot.Position.X < 0) robot.Position.X += maxX;

            robot.Position.Y = (robot.Position.Y + robot.Velocity.Y) % maxY;
            if (robot.Position.Y < 0) robot.Position.Y += maxY;
        }
    }

    public char[,] GenerateMap(List<Robot> robots, int maxX, int maxY)
    {
        var map = new char[maxX, maxY];
        for (var i = 0; i < maxX; i++)
        {
            for (var j = 0; j < maxY; j++)
            {
                if (robots.Any(r => r.Position.X == i && r.Position.Y == j))
                {
                    map[i, j] = '#';
                }
                else
                {
                    map[i, j] = ' ';
                }
            }
        }
        return map;
    }

    public string MapToString(char[,] map)
    {
        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                sb.Append($"{map[i, j]}"); //Console.Write($"{map[i, j]}");
            }
            sb.Append("\n"); //Console.WriteLine();
        }
        return sb.ToString();
    }

    public void ShowBots(List<Robot> robots, int maxX, int maxY)
    {
        var map = new int[maxX, maxY];
        for (var i = 0; i < maxX; i++)
        {
            for (var j = 0; j < maxY; j++)
            {
                map[i, j] = 0;
            }
        }

        foreach (var robot in robots)
        {
            map[robot.Position.X, robot.Position.Y]++;
        }

        for (var i = 0; i < maxX; i++)
        {
            for (var j = 0; j < maxY; j++)
            {
                if (map[i, j] == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write($"{map[i, j]}");
                }
            }
            Console.WriteLine();
        }
    }

    // List<Region> FindAllRegions(char [,] garden)
    // {
    //     List<Region> allRegions = new List<Region>();

    //     var startPoint = new Coords(0,0);
    //     while (startPoint != null)
    //     {
    //         allRegions.Add(new Region(GetRegionCoords(startPoint, ref garden)));
    //         startPoint = FindNextRegion(garden);
    //     }
    //     return allRegions;
    // }

    // Coords FindNextRegion(char[,] garden)
    // {
    //     for (var x=0;x<garden.GetLength(1);x++)
    //     {
    //         for (var y=0;y<garden.GetLength(0);y++)
    //         {
    //             if (garden[y,x] != ' ')
    //             {
    //                 return new Coords(x, y);
    //             }
    //         }
    //     }
    //     return null;
    // }

    // List<Coords> GetRegionCoords(Coords plot, ref char[,] garden)
    // {
    //     char plotType = garden[plot.Y,plot.X];
    //     var result = new List<Coords> {plot};
    //     garden[plot.Y,plot.X] = ' ';
    //     if (plot.X> 0 && garden[plot.Y,plot.X-1]==plotType)
    //     {
    //         result.AddRange(GetRegionCoords(new Coords(plot.X-1, plot.Y), ref garden));
    //     }
    //     if (plot.Y> 0 && garden[plot.Y-1,plot.X]==plotType)
    //     {
    //         result.AddRange(GetRegionCoords(new Coords(plot.X, plot.Y-1), ref garden));
    //     }
    //     if (plot.X<garden.GetLength(1)-1 && garden[plot.Y,plot.X+1]==plotType)
    //     {
    //         result.AddRange(GetRegionCoords(new Coords(plot.X+1, plot.Y), ref garden));
    //     }
    //     if (plot.Y<garden.Length-1 && garden[plot.Y+1,plot.X]==plotType)
    //     {
    //         result.AddRange(GetRegionCoords(new Coords(plot.X, plot.Y+1), ref garden));
    //     }
    //     return result;
    // }

}
public class Robot
{
    public required Coords Position { get; set; }
    public required Coords Velocity { get; set; }
}


// public class Region
// {
//     public Region(List<Coords> plots)
//     {
//         Plots = plots;
//     }

//     public List<Coords> Plots {get;}

//     public int Area => Plots.Count;
// }