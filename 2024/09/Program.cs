namespace advent;
public class DiskFragmenter
{
    const string inputSample = @"C:\dev\src\nash\AdventOfCode\2024\09\input-sample";
    const string inputActual = @"C:\dev\src\nash\AdventOfCode\2024\09\input-actual";
    const int expectedSampleResult1 = 1928;
    const int expectedSampleResult2 = 2858;

    internal int MaxX = 0;
    internal int MaxY = 0;

    public static void Main()
    {
        long answer;
        Console.Write("P1 Sample: ");
        answer = new DiskFragmenter().Problem1(inputSample);
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleResult1)
        {
            throw new Exception($"Sample should have returned {expectedSampleResult1}");
        }
        else
        {
            Console.WriteLine("Sample is correct.");
        }

        Console.Write("P1 Actual: ");
        answer = new DiskFragmenter().Problem1(inputActual);
        Console.WriteLine($"{answer}");

        Console.Write("P2 Sample: ");
        answer = answer = new DiskFragmenter().Problem2(inputSample);
        Console.WriteLine($"{answer}");

        if (answer != expectedSampleResult2)
        {
            throw new Exception($"Sample should have returned {expectedSampleResult2}");
        }
        else
        {
            Console.WriteLine("Sample is correct.");
        }
        Console.Write("P2 Actual: ");
        answer = answer = new DiskFragmenter().Problem2(inputActual);
        Console.WriteLine($"{answer}");
    }

    long Problem1(string inputFilePath)
    { 
        string diskMap = "";

        using (var rdr = File.OpenText(inputFilePath))
        {
            diskMap = rdr.ReadToEnd();
        }

        var initialDiskSize = diskMap
            .ToCharArray()
            .Select(x => int.Parse($"{x}"))
            .Sum();

        var diskActual = new int[initialDiskSize];
        Array.Fill(diskActual, -1);
        int curId = 0;
        bool emptyflag = false;
        int blockPointer = 0;
        foreach (char c in diskMap)
        {
            var numBlocks = int.Parse($"{c}");
            if (!emptyflag)
            {
                Array.Fill(diskActual, curId, blockPointer, numBlocks);
                curId++;
            }
            emptyflag = !emptyflag;
            blockPointer += numBlocks;
        }

        int headPointer = 0, tailPointer = initialDiskSize - 1;
        while (headPointer < tailPointer)
        {
            if (diskActual[headPointer] != -1)
            {
                headPointer++;
                continue;
            }
            if (diskActual[tailPointer] == -1)
            {
                tailPointer--;
                continue;
            }
            diskActual[headPointer] = diskActual[tailPointer];
            diskActual[tailPointer] = -1;
        }

        long checksum = 0;
        for (var i = 0; diskActual[i] != -1; i++)
        {
            checksum += i * diskActual[i];
        }

        return checksum; 
    }

    long Problem2(string inputFilePath)
    {

        string diskMap = "";

        using (var rdr = File.OpenText(inputFilePath))
        {
            diskMap = rdr.ReadToEnd();
        }

        var initialDiskSize = diskMap
            .ToCharArray()
            .Select(x => int.Parse($"{x}"))
            .Sum();

        var diskActual = new int[initialDiskSize];
        Array.Fill(diskActual, -1);
        int curId = 0;
        bool emptyflag = false;
        int blockPointer = 0;
        foreach (char c in diskMap)
        {
            var numBlocks = int.Parse($"{c}");
            if (!emptyflag)
            {
                Array.Fill(diskActual, curId, blockPointer, numBlocks);
                curId++;
            }
            emptyflag = !emptyflag;
            blockPointer += numBlocks;
        }
        curId--;
        while (curId >= 0)
        {
            var blockSize = diskActual.Count(x => x == curId);
            var dest = findDestination(blockSize, diskActual, curId);
            if (dest >= 0)
            {
                for (var i = dest; i < initialDiskSize; i++)
                {
                    if (diskActual[i] == curId) diskActual[i] = -1;
                    if (i >= dest && i < dest + blockSize) diskActual[i] = curId;
                }
            }
            curId--;
        }

        long checksum = 0;
        for (var i = 0; i<diskActual.Length; i++)
        {
            if (diskActual[i] > 0)
            {
                checksum += i * diskActual[i];
            }
        }

        return checksum;
    }

    int findDestination(int size, int[] disk, int id)
    {
        int curEmptySize = 0;
        for (var i = 0; i < disk.Length; i++) {
            if (disk[i] == id)
            {
                return -1;
            }
            else if (disk[i] >= 0)
            {
                curEmptySize = 0;
            }
            else
            {
                curEmptySize++;
                if (curEmptySize == size)
                {
                    return i - curEmptySize + 1;
                }
            }
        }
        return -1;
    }

}
