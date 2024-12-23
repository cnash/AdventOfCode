
namespace advent;

public class Solver
{
    const string inputSample = @"C:\Workspaces\AdventOfCode\2024\15\input-sample";
    const string inputSampleSmall = @"C:\Workspaces\AdventOfCode\2024\15\input-sample-small";
    const string inputActual = @"C:\Workspaces\AdventOfCode\2024\15\input-actual";
    const int expectedSampleSmallResult1 = 2028;
    const int expectedSampleResult1 = 10092;
    const int expectedSampleResult2 = 9021;

    public static void Main()
    {
        long answer;

        answer = new Solver().Problem1(inputSampleSmall);
        Console.WriteLine($"Problem 1 Sample: Expected: {expectedSampleSmallResult1}, Observed: {answer}");
        if (answer != expectedSampleSmallResult1) { return; }

        answer = new Solver().Problem1(inputSample);
        Console.WriteLine($"Problem 1 Sample: Expected: {expectedSampleResult1}, Observed: {answer}");
        if (answer != expectedSampleResult1) { return; }

        answer = new Solver().Problem1(inputActual);
        Console.WriteLine($"Problem 1 Actual: Observed: {answer}");

        answer = new Solver().Problem2(inputSample);
        Console.WriteLine($"Problem 2 Sample: Expected: {expectedSampleResult2}, Observed: {answer}");
        if (answer != expectedSampleResult2) { return; }
        answer = new Solver().Problem2(inputActual);
        Console.WriteLine($"Problem 2 Actual: Observed: {answer}");
    }

    long Problem1(string inputFilePath)
    {
        long result = 0;
        ReadData(inputFilePath);
        SetRobotPosition();
        foreach (char c in Instructions)
        {
            ExecuteInstruction(c);
        }

        for (var y = 0; y < Map.Count; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                if (Map[y][x] == 'O')
                {
                    result += (100 * y) + x;
                }
            }
        }

        return result;
    }

    long Problem2(string inputFilePath)
    {
        long result = 0;
        ReadData(inputFilePath);
        WidenMap();
        SetRobotPosition();
        foreach (char c in Instructions)
        {
            ExecuteInstruction2(c);
        }

        for (var y = 0; y < Map.Count; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                if (Map[y][x] == '[')
                {
                    result += (100 * y) + x;
                }
            }
        }

        return result;
    }

    List<string> Map = [];

    string Instructions = "";

    Coords RobotPosition = new Coords(0, 0);

    void ReadData(string inputFilePath)
    {
        bool readingMap = true;
        using (var rdr = File.OpenText(inputFilePath))
        {
            while (!rdr.EndOfStream)
            {
                var line = rdr.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingMap = false;
                    continue;
                }
                if (readingMap)
                {
                    Map.Add(line);
                }
                else
                {
                    Instructions += line;
                }
            }
        }

    }

    void SetRobotPosition()
    {
        for (var y = 0; y < Map.Count; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                if (Map[y][x] == '@')
                {
                    RobotPosition.X = x;
                    RobotPosition.Y = y;
                    return;
                }
            }
        }
    }

    void ExecuteInstruction(char direction)
    {
        Coords intendedDestination = NextSpace(direction, RobotPosition);

        if (intendedDestination.X < 0
            || intendedDestination.Y < 0
            || intendedDestination.Y >= Map.Count
            || intendedDestination.X >= Map[intendedDestination.Y].Length)
        {
            //Edge of the Map. Do nothing.
            return;
        }

        var charAtIntendedDestination = GetCharAt(intendedDestination);

        if (charAtIntendedDestination == '#')
        {
            //Wall. Do nothing.
            return;
        }

        if (charAtIntendedDestination == '.')
        {
            //Empty space.  Move into it.
            MoveRobotTo(intendedDestination);
            return;
        }

        var considering = intendedDestination;
        while (GetCharAt(considering) == 'O')
        {
            considering = NextSpace(direction, considering);
        }
        if (GetCharAt(considering) == '#')
        {
            //Can't move boxes because they are against a wall;
            return;
        }
        else
        {
            SetCharAt('O', considering);
            MoveRobotTo(intendedDestination);
        }
    }

    void MoveRobotTo(Coords dest)
    {
        SetCharAt('.', RobotPosition);
        SetCharAt('@', dest);
        RobotPosition = dest;
    }

    void SetCharAt(char c, Coords dest)
    {
        var charArray = Map[dest.Y].ToCharArray();
        charArray[dest.X] = c;
        Map[dest.Y] = new string(charArray);
    }

    char GetCharAt(Coords dest)
    {
        return Map[dest.Y][dest.X];
    }

    Coords NextSpace(char direction, Coords currentSpace)
    {
        switch (direction)
        {
            case '^':
                return new Coords(currentSpace.X, currentSpace.Y - 1);
            case '>':
                return new Coords(currentSpace.X + 1, currentSpace.Y);
            case 'v':
                return new Coords(currentSpace.X, currentSpace.Y + 1);
            case '<':
                return new Coords(currentSpace.X - 1, currentSpace.Y);
            default:
                throw new Exception("Invalid character on map");
        }
    }

    void WidenMap()
    {
        var newMap = new List<string>();
        foreach (var line in Map)
        {
            newMap.Add(
                line
                    .Replace("#", "##")
                    .Replace("O", "[]")
                    .Replace(".", "..")
                    .Replace("@", "@."));
        }
        Map = newMap;
    }

    void ExecuteInstruction2(char direction)
    {
        Coords intendedDestination = NextSpace(direction, RobotPosition);

        if (intendedDestination.X < 0
            || intendedDestination.Y < 0
            || intendedDestination.Y >= Map.Count
            || intendedDestination.X >= Map[intendedDestination.Y].Length)
        {
            //Edge of the Map. Do nothing.
            return;
        }

        var charAtIntendedDestination = GetCharAt(intendedDestination);

        if (charAtIntendedDestination == '#')
        {
            //Wall. Do nothing.
            return;
        }

        if (charAtIntendedDestination == '.')
        {
            //Empty space.  Move into it.
            MoveRobotTo(intendedDestination);
            return;
        }

        //boxes?!?!
        if (CanBigBoxMove(intendedDestination, direction))
        {
            MoveBigBoxes(intendedDestination, direction);
            MoveRobotTo(intendedDestination);
            return;
        }
    }

    bool CanBigBoxMove(Coords boxPos1, char direction)
    {
        if (direction == '<' || direction == '>')
        {
            var considering = boxPos1;
            while (GetCharAt(considering) == ']' || GetCharAt(considering) == '[')
            {
                considering = NextSpace(direction, considering);
            }
            return GetCharAt(considering) != '#';
        }

        var charAtPos = GetCharAt(boxPos1);
        var boxPos2 = new Coords((charAtPos == '[') ? boxPos1.X + 1 : boxPos1.X - 1, boxPos1.Y);

        var nextChar1 = GetCharAt(NextSpace(direction, boxPos1));
        if (nextChar1 == '#') return false;

        var nextChar2 = GetCharAt(NextSpace(direction, boxPos2));
        if (nextChar2 == '#') return false;

        return (nextChar1 == '.' || CanBigBoxMove(NextSpace(direction, boxPos1), direction))
            && (nextChar2 == '.' || CanBigBoxMove(NextSpace(direction, boxPos2), direction));
    }

    void MoveBigBoxes(Coords boxPos1, char direction)
    {
        if (direction == '<' || direction == '>')
        {
            char c;
            var considering = boxPos1;
            var cStack = new Stack<char>();
            var posStack = new Stack<Coords>();
            while ((c = GetCharAt(considering)) != '.')
            {
                cStack.Push(c);
                posStack.Push(considering);
                considering = NextSpace(direction, considering);
            }
            while (cStack.TryPop(out var mapChar))
            {
                SetCharAt(mapChar, considering);
                considering = posStack.Pop();
            }
            SetCharAt('.', boxPos1);
            return;
        }

        var charAtPos = GetCharAt(boxPos1);
        var boxPos2 = new Coords((charAtPos == '[') ? boxPos1.X + 1 : boxPos1.X - 1, boxPos1.Y);
        
        var nextChar1 = GetCharAt(NextSpace(direction, boxPos1));
        if (nextChar1 == '[' || nextChar1 == ']')
        {
            MoveBigBoxes(NextSpace(direction, boxPos1), direction);
        }

        var nextChar2 = GetCharAt(NextSpace(direction, boxPos2));
        if (nextChar2 == '[' || nextChar2 == ']')
        {
            MoveBigBoxes(NextSpace(direction, boxPos2), direction);
        }

        SetCharAt(GetCharAt(boxPos1), NextSpace(direction, boxPos1));
        SetCharAt(GetCharAt(boxPos2), NextSpace(direction, boxPos2));
        SetCharAt('.', boxPos1);
        SetCharAt('.', boxPos2);
    }
}