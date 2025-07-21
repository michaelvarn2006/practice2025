using Xunit;
using task17;
using ICommand;
using System;
using System.Threading;

public class ServerBaseTests
{
    [Fact]
    public void HardStop_InterruptsProcessingRegardlessOfQueue()
    {
        var worker = new ServerThread();
        task17.DummyAction.Counter = 0;
        worker.EnqueueCommand(new task17.DummyAction());
        worker.EnqueueCommand(new HardStop(worker));
        worker.EnqueueCommand(new task17.DummyAction());
        Thread.Sleep(300);
        Assert.False(worker.IsAlive);
        Assert.True(task17.DummyAction.Counter == 1);
    }

    [Fact]
    public void HardStop_ThrowsIfNotFromWorkerThread()
    {
        var worker = new ServerThread();
        var cmd = new HardStop(worker);
        Assert.Throws<InvalidOperationException>(() => cmd.Execute());
    }

    [Fact]
    public void SoftStop_WaitsForQueueToDrain()
    {
        var worker = new ServerThread();
        task17.DummyAction.Counter = 0;
        worker.EnqueueCommand(new task17.DummyAction());
        worker.EnqueueCommand(new SoftStop(worker));
        worker.EnqueueCommand(new task17.DummyAction());
        Thread.Sleep(300);
        Assert.False(worker.IsAlive);
        Assert.True(task17.DummyAction.Counter == 2);
    }

    [Fact]
    public void SoftStop_ThrowsIfNotFromWorkerThread()
    {
        var worker = new ServerThread();
        var cmd = new SoftStop(worker);
        Assert.Throws<InvalidOperationException>(() => cmd.Execute());
    }

    [Fact]
    public void ExceptionInCommand_DoesNotStopThread()
    {
        var worker = new ServerThread();
        task17.DummyAction.Counter = 0;
        worker.EnqueueCommand(new task17.FailingCommand());
        worker.EnqueueCommand(new task17.DummyAction());
        worker.EnqueueCommand(new SoftStop(worker));
        Thread.Sleep(300);
        Assert.False(worker.IsAlive);
        Assert.True(task17.DummyAction.Counter == 1);
    }
}

