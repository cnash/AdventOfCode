using Microsoft.VisualBasic;
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
    const long CorrectActualResult1 = 194557;
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
        Console.WriteLine($"Actual Result: Expected: {CorrectActualResult1},  {ActualResult}");
        if (CorrectActualResult1 != ActualResult) { return; }

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


    public long Problem2(string input, int blinkCount)
    {
        var pebbles = input
            .Split(' ')
            .Select(x=> BlinkRecursive(long.Parse(x), 1, blinkCount))
            .Sum();

        return pebbles;
    }

    Dictionary<int, Dictionary<long, long>> _pastCalcs = new Dictionary<int, Dictionary<long, long>>();
    long getPastCalc(long value, int blinkNo)
    {
        if (_pastCalcs.TryGetValue(blinkNo, out var pastCalcsThisBlink))
        {
            if (pastCalcsThisBlink.TryGetValue(value, out var result))
            {
                return result;
            }
        }
        return 0;
    }
    void saveCalc(long value, int blinkNo, long result)
    {
        if (!_pastCalcs.ContainsKey(blinkNo))
        {
            _pastCalcs[blinkNo] = new Dictionary<long, long>();
        }
        _pastCalcs[blinkNo][value] = result;
    }

    long BlinkRecursive(long value, int blinkNo, int targetBlinkNo)
    {
        var pastCalc = getPastCalc(value, blinkNo);
        if (pastCalc != 0)
        {
            return pastCalc;
        }

        long result;

        if (blinkNo > targetBlinkNo)
        {
            result = 1;
        }
        else
        {
            var nextBlinkNo = blinkNo + 1;
            if (value == 0)
            {
                result = BlinkRecursive(1, nextBlinkNo, targetBlinkNo);
            }
            else
            {
                var strValue = $"{value}";
                if (strValue.Length % 2 == 1)
                {
                    // return BlinkRecursive((value << 11) + (value << 3), nextBlinkNo, targetBlinkNo);
                    result = BlinkRecursive(value * 2024, nextBlinkNo, targetBlinkNo);
                }
                else
                {
                    var halfStringLength = strValue.Length >> 1;
                    var p1 = long.Parse(strValue.Substring(0, halfStringLength));
                    var p2 = long.Parse(strValue.Substring(halfStringLength));
                    result = BlinkRecursive(p1, nextBlinkNo, targetBlinkNo) + BlinkRecursive(p2, nextBlinkNo, targetBlinkNo);
                }
            }
        }
        saveCalc(value, blinkNo, result);
        return result;
    }
}