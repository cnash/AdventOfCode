namespace advent;

public class Solver
{
    const string inputSample = @"..\..\..\input-sample";
    const string inputActual = @"..\..\..\input-actual";
    const int expectedSampleResult1 = 0;
    const int expectedSampleResult2 = 0;

    public static void Main()
    {
        long answer;

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
        return 0;
    }

    long Problem2(string inputFilePath)
    {
        return 0;
    }


}