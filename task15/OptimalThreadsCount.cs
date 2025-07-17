using System.Text.Json;
using System.Diagnostics;
using task14;
using System.Linq;


public class OptimalThreadsCount
{
    public static void Main()
    {
        var function = Math.Sin;
        double[] stepOptions = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        double chosenStep = 0;

        foreach (var step in stepOptions)
        {
            double result = DefiniteIntegral.SolveForOneThread(-100, 100, function, step);
            if (Math.Abs(result) <= 1e-4)
            {
                chosenStep = step;
                break;
            }
        }

        double[] timings = new double[10]; 

        double singleTiming = 0.0;
        for (int i = 0; i < 100; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            _ = DefiniteIntegral.SolveForOneThread(-100, 100, function, chosenStep);
            stopwatch.Stop();
            singleTiming += stopwatch.Elapsed.TotalMilliseconds;
        }
        singleTiming = singleTiming / 100;
        timings[0] = singleTiming;

        for (int threadCount = 2; threadCount <= 10; threadCount++)
        {
            double totalTime = 0.0;
            for (int j = 0; j < 100; j++)
            {
                var stopwatch = Stopwatch.StartNew();
                _ = DefiniteIntegral.Solve(-100, 100, function, chosenStep, threadCount);
                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;
            }
            timings[threadCount - 1] = totalTime / 100;
        }

        var multiTiming = timings.Skip(1).Min();
        var bestIndex = timings.Skip(1).ToList().IndexOf(multiTiming);
        var bestThreads = bestIndex + 2;
        var diffTime = (singleTiming - multiTiming) / singleTiming * 100;

        File.WriteAllText("./result.txt",
            $"Step size: {chosenStep}\n" +
            $"Optimal thread count: {bestThreads} (more data in outputData.json) \n" +
            $"Multithreaded minimum time: {multiTiming:F4} ms (more data in outputData.json) \n" +
            $"Single-threaded minimum time: {singleTiming:F4} ms (more data in outputData.json) \n" +
            $"Time diff: {diffTime:F2}%\n");
        var outputData = new
        {
            ThreadsCount = (new int[] { 1 }).Concat(Enumerable.Range(2, 9)).ToArray(),
            Timings = timings
        };

        var settings = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(outputData, settings);
        File.WriteAllText("outputData.json", json);
    }
}

