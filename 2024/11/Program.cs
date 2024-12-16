using System.Formats.Asn1;

namespace advent;
public class Pebbles
{
    const string SampleInput = "125 17";
    const int SampleBlinkCount1 = 6;
    const int SampleExpectedResult1 = 22;
    const int SampleBlinkCount2 = 25;
    const int SampleExpectedResult2 = 55312;

    const string ActualInput = "8793800 1629 65 5 960 0 138983 85629";
    const int ActualBlinkCount = 25;
    const int ActualBlinkCount2 = 75;

    public static void Main()
    {
        var Sample1Result = new Pebbles().Problem2(SampleInput, SampleBlinkCount1);
        Console.WriteLine($"Sample 1: Expected: {SampleExpectedResult1}, Observed: {Sample1Result}");
        if (Sample1Result != SampleExpectedResult1 ) { return; }

        var Sample2Result = new Pebbles().Problem2(SampleInput, SampleBlinkCount2);
        Console.WriteLine($"Sample 1: Expected: {SampleExpectedResult2}, Observed: {Sample2Result}");
        if (Sample2Result != SampleExpectedResult2) { return; }

        var ActualResult = new Pebbles().Problem2(ActualInput, ActualBlinkCount);
        Console.WriteLine($"Actual Result: {ActualResult}");

        ActualResult = new Pebbles().Problem2(ActualInput, ActualBlinkCount2);
        Console.WriteLine($"Actual Result #2: {ActualResult}");
    }

    public int Problem1(string input, int blinkCount)
    {
        var pebbles = input.Split(' ').Select(x=>long.Parse(x)).ToArray();
        for (var i=0;i<blinkCount;i++)
        {
            pebbles = Blink(pebbles);
        }
        return pebbles.Count();
    }

    long[] Blink(long[] pebbles)
    {
        var result = new List<long>();
        foreach (var p in pebbles)
        {
            if (p==0)
            {
                result.Add(1);
                continue;
            }
            var pStr = $"{p}";
            if (pStr.Length % 2 == 0)
            {
                result.Add(long.Parse(pStr.Substring(0,pStr.Length/2)));
                result.Add(long.Parse(pStr.Substring(pStr.Length/2)));
                continue;
            }
            result.Add(p*2024);
        }
        return result.ToArray();
    }

    const string cache1 = "./cache1";
    const string cache2 = "./cache2";
    const int pageSize = 1000;

    public long Problem2(string input, int blinkCount)
    {
        var pebbles = input
            .Split(' ')
            .Select(x=> BlinkRecursive(uint.Parse(x), 1, blinkCount))
            .Sum();

        return pebbles;
    }

    long BlinkRecursive(uint value, int blinkNo, int targetBlinkNo)
    {
        if (blinkNo > targetBlinkNo)
        {
            return 1;
        }

        var nextBlinkNo = blinkNo+1;
        if (value == 0)
        {
            return BlinkRecursive(1, nextBlinkNo, targetBlinkNo);
        }

        var strValue = $"{value}";
        if (strValue.Length % 2 == 1)
        {
            // return BlinkRecursive((value << 11) + (value << 3), nextBlinkNo, targetBlinkNo);
            return BlinkRecursive(value * 2024, nextBlinkNo, targetBlinkNo);
        }
        var halfStringLength = strValue.Length>>1;
        var p1 = uint.Parse(strValue.Substring(0,halfStringLength));
        var p2 = uint.Parse(strValue.Substring(halfStringLength));

        return BlinkRecursive(p1, nextBlinkNo, targetBlinkNo) + BlinkRecursive(p2, nextBlinkNo, targetBlinkNo);
    }
}