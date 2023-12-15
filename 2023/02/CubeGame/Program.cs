using System.IO;
using System.Text.RegularExpressions;

// See https://aka.ms/new-console-template for more information
const string inputFileName = @"C:\dev\src\nash\advent\20231202.1\CubeGame\input-actual.txt";
// const string inputFileName = @"C:\dev\src\nash\advent\20231202.1\CubeGame\input-sample";

// bool possibleGame(string line, string color, int maxCubes)
// {
//     var rx = new Regex(@"(?<qty>\d+) " + color);
//     var matches = rx.Matches(line);
//     var qtys = matches.SelectMany(m=> m.Groups.Values).Where(g=>g.Name == "qty").Select(g=>g.Value);
//     return !qtys.Any(qty => {
//         if (int.TryParse(qty, out int q))
//         {
//             return q > maxCubes;
//         }
//         return true;
//     });
// }

int minQty(string line, string color)
{
    var rx = new Regex(@"(?<qty>\d+) " + color);
    var matches = rx.Matches(line);
    var qtys = matches.SelectMany(m=> m.Groups.Values).Where(g=>g.Name == "qty").Select(g => {
            if (int.TryParse(g.Value, out var num))
            {
                return num;
            };
            return 0;
        });
    return qtys.Max();
}

int total = 0;
using (var rdr = File.OpenText(inputFileName))
{
    while(!rdr.EndOfStream)
    {
        var line = rdr.ReadLine();
        if (line == null) continue;

        var minRed = minQty(line, "red");
        var minGreen = minQty(line, "green");
        var minBlue = minQty(line, "blue");
        Console.WriteLine($"{minRed} red, {minGreen} green, {minBlue} blue = {minRed * minGreen * minBlue}");
        total += minRed * minGreen * minBlue;
    }
    Console.WriteLine($"{total}");
}