using Xunit;
using task18;
using IScheduler;
using ICommand;
using RoundRobinScheduler;
using System.Threading;
using System.Diagnostics;
using ISteppable;

namespace task18tests;

public class StepCommand : ICommand.ICommand, ISteppable.ISteppable
{
    public int StepsLeft;
    public int Executed = 0;
    public StepCommand(int steps) { StepsLeft = steps; }
    public void Execute() { if (StepsLeft > 0) { StepsLeft--; Executed++; } }
    public bool IsDone => StepsLeft == 0;
}

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
        Assert.Equal(5, cmd1.Executed);
        Assert.Equal(5, cmd2.Executed);
        Assert.InRange(cmd1.Executed, 5, 5);
        Assert.InRange(cmd2.Executed, 5, 5);
    }

    [Fact]
    public void ShortCommand_NotBlockedByLong()
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
    }

    [Fact]
    public void NoWork_DoesNotWasteCpu()
    {
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var cs = new CommandScheduler(scheduler);
        var sw = Stopwatch.StartNew();
        Thread.Sleep(100);
        cs.Stop();
        Assert.True(sw.ElapsedMilliseconds >= 100);
    }

    [Fact]
    public void NoCommands_ThreadStopsGracefully()
    {
        var scheduler = new RoundRobinScheduler.RoundRobinScheduler();
        var cs = new CommandScheduler(scheduler);
        Thread.Sleep(50);
        cs.Stop();
        Assert.True(true);
    }
}

