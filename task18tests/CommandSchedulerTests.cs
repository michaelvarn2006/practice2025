using Xunit;
using task18;
using IScheduler;
using ICommand;
using RoundRobinScheduler;
using System.Threading;
using System.Diagnostics;

namespace task18tests;

public class CommandSchedulerTests
{
    [Fact]
    public void TwoLongCommands_ExecutedInTurns()
    {
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var cs = new CommandScheduler(scheduler);
        var cmd1 = new StepCommand(5);
        var cmd2 = new StepCommand(5);
        cs.EnqueueCommand(cmd1);
        cs.EnqueueCommand(cmd2);
        while (!cmd1.IsDone || !cmd2.IsDone) Thread.Sleep(10);
        cs.Stop();
        Assert.True(cmd1.Executed == 5);
        Assert.True(cmd2.Executed == 5);
    }

    [Fact]
    public void OneShortOneLongCommands_ShortNotBlockedByLong()
    {
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var cs = new CommandScheduler(scheduler);
        var longCmd = new StepCommand(100);
        var shortCmd = new StepCommand(1);
        cs.EnqueueCommand(longCmd);
        cs.EnqueueCommand(shortCmd);
        while (!shortCmd.IsDone) Thread.Sleep(5);
        cs.Stop();
        Assert.True(shortCmd.IsDone);
        Assert.True(shortCmd.Executed == 1);
        Assert.True(longCmd.Executed == 100);
        Assert.True(longCmd.IsDone);
    }

    [Fact]
    public void NoCommands_CommandSchedulerCorrectlyWaitsForCommands()
    {
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var cs = new CommandScheduler(scheduler);
        var sw = Stopwatch.StartNew();
        Thread.Sleep(100);
        cs.Stop();
        Assert.True(sw.ElapsedMilliseconds >= 100);
    } 
}

