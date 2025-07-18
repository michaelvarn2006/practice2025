using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using task18;
using RoundRobinScheduler;
using TestCommand;
using IScheduler;

namespace task19;

public class CommandGraph
{
    static void Main()
    {
        int commandCount = 5;
        int callCommand = 3;

        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var server = new task18.CommandScheduler(scheduler);

        var commands = new List<TestCommand.TestCommand>();

        for (int i = 1; i <= commandCount; i++)
        {
            var command = new TestCommand.TestCommand(scheduler, i, callCommand);
            scheduler.Add(command);
            commands.Add(command);
        }

        while (commands.Any(c => c.Counter < callCommand))
        {
            Thread.Sleep(10);
        }

        server.Stop();
        Thread.Sleep(100);

        double[] times = commands.Select(cmd => (double)cmd.ElapsedMs).ToArray();
        int[] commandNumbers = Enumerable.Range(1, commandCount).ToArray();

        var plot = new ScottPlot.Plot();
        plot.Add.Scatter(times, commandNumbers);
        plot.Title("TestCommand Execution");
        plot.XLabel("Time (ms)");
        plot.YLabel("Number of commands");
        plot.SavePng("./result.png", 600, 600);

        double totalTime = times.Sum();
        double averageTime = times.Average();

        string report =
            $"Number of commands: {commandCount}\n" +
            $"Calls per command: {callCommand}\n" +
            $"Minimum execution time: {times.Min()} ms\n" +
            $"Maximum execution time: {times.Max()} ms\n" +
            $"Average time per command: {averageTime:F2} ms\n" +
            $"Total execution time: {totalTime} ms\n";

        File.WriteAllText("./result.txt", report);
    }
}

