using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using task18;
using RoundRobinScheduler;
using TestCommand;
using IScheduler;

namespace task19tests;

public class LongCommandsTests
{
    [Fact]
    public void FiveTestCommand_ThreeExecutesEach_ConsoleOutputCorrect()
    {
        var originalOut = Console.Out;
        var sw = new StringWriter();
        Console.SetOut(sw);

        int commandCount = 5;
        int callCommand = 3;
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var server = new CommandScheduler(scheduler);
        var commands = new TestCommand.TestCommand[commandCount];
        for (int i = 0; i < commandCount; i++)
        {
            commands[i] = new TestCommand.TestCommand(scheduler, i + 1, callCommand);
            scheduler.Add(commands[i]);
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        while (commands.Any(c => c.Counter < callCommand) && stopwatch.ElapsedMilliseconds < 5000)
            Thread.Sleep(10);
        Assert.True(stopwatch.ElapsedMilliseconds < 5000, "Test timed out: commands did not finish in 5 seconds");

        server.Stop();
        Thread.Sleep(100);
        Console.SetOut(originalOut);

        string output = sw.ToString();
        for (int i = 1; i <= commandCount; i++)
        {
            for (int j = 1; j <= callCommand; j++)
            {
                Assert.Contains($"Command {i} call {j}", output);
            }
        }
    }
}

