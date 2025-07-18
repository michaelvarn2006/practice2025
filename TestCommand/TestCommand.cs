using task18;
using IScheduler;
using ICommand;
using ISteppable;
using System.Diagnostics;

namespace TestCommand;

public class TestCommand : ICommand.ICommand, ISteppable.ISteppable
{
    public int Counter = 0;
    private int Id;
    private int MaxCount;
    private IScheduler.IScheduler Scheduler;
    private Stopwatch stopwatch = new Stopwatch();
    public long ElapsedMs => stopwatch.ElapsedMilliseconds;
    public bool IsDone => Counter >= MaxCount;
    public TestCommand(IScheduler.IScheduler scheduler, int id, int maxCount)
    {
        Scheduler = scheduler;
        Id = id;
        MaxCount = maxCount;
    }
    public void Execute()
    {
        if (Counter == 0)
            stopwatch.Start();

        if (Counter < MaxCount)
        {
            Console.WriteLine($"Command {Id} call {++Counter}");
            if (Counter < MaxCount)
                Scheduler.Add(this);
            else
                stopwatch.Stop();
        }
    }
}

