using Xunit;
using task17;
using ICommand;
using System;
using System.Threading;

internal class DummyAction : ICommand.ICommand
{
    public static int Counter = 0;
    public void Execute() { Interlocked.Increment(ref Counter); Thread.Sleep(50); }
}

internal class FailingCommand : ICommand.ICommand
{
    public void Execute() => throw new InvalidOperationException("Test exception");
}

public class ServerThreadSpec
{
    [Fact]
    public void ImmediateStop_InterruptsProcessingRegardlessOfQueue()
    {
        var worker = new ServerThread();
        DummyAction.Counter = 0;
        worker.EnqueueCommand(new DummyAction());
        worker.EnqueueCommand(new HardStop(worker));
        worker.EnqueueCommand(new DummyAction());
        Thread.Sleep(200);
        Assert.False(worker.IsAlive);
        Assert.True(DummyAction.Counter <= 2);
    }

    [Fact]
    public void ImmediateStop_ThrowsIfNotFromWorkerThread()
    {
        var worker = new ServerThread();
        var cmd = new HardStop(worker);
        Assert.Throws<InvalidOperationException>(() => cmd.Execute());
    }

    [Fact]
    public void GracefulStop_WaitsForQueueToDrain()
    {
        var worker = new ServerThread();
        DummyAction.Counter = 0;
        worker.EnqueueCommand(new DummyAction());
        worker.EnqueueCommand(new SoftStop(worker));
        worker.EnqueueCommand(new DummyAction());
        Thread.Sleep(400);
        Assert.False(worker.IsAlive);
        Assert.Equal(2, DummyAction.Counter);
    }

    [Fact]
    public void GracefulStop_ThrowsIfNotFromWorkerThread()
    {
        var worker = new ServerThread();
        var cmd = new SoftStop(worker);
        Assert.Throws<InvalidOperationException>(() => cmd.Execute());
    }

    [Fact]
    public void ExceptionInCommand_DoesNotStopThread()
    {
        var worker = new ServerThread();
        DummyAction.Counter = 0;
        worker.EnqueueCommand(new FailingCommand());
        worker.EnqueueCommand(new DummyAction());
        worker.EnqueueCommand(new SoftStop(worker));
        Thread.Sleep(300);
        Assert.False(worker.IsAlive);
        Assert.Equal(1, DummyAction.Counter);
    }
}

