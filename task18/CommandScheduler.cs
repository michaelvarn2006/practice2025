using System;
using System.Collections.Generic;
using System.Threading;
using IScheduler;
using ICommand;
using ISteppable;

namespace task18;

public class CommandScheduler
{
    private readonly IScheduler.IScheduler _scheduler;
    private readonly Queue<ICommand.ICommand> _newCommands = new();
    private readonly object _lock = new();
    private Thread _thread;
    private bool _running = true;

    public CommandScheduler(IScheduler.IScheduler scheduler)
    {
        _scheduler = scheduler;
        _thread = new Thread(Run) { IsBackground = true };
        _thread.Start();
    }

    public void EnqueueCommand(ICommand.ICommand cmd)
    {
        lock (_lock)
        {
            _newCommands.Enqueue(cmd);
            Monitor.PulseAll(_lock);
        }
    }

    public void Stop()
    {
        lock (_lock)
        {
            _running = false;
            Monitor.PulseAll(_lock);
        }
    }

    private void Run()
    {
        while (true)
        {
            ICommand.ICommand? cmd = null;
            lock (_lock)
            {
                if (_scheduler.HasCommand())
                {
                    cmd = _scheduler.Select();
                }
                else if (_newCommands.Count > 0)
                {
                    var newCmd = _newCommands.Dequeue();
                    _scheduler.Add(newCmd);
                    cmd = _scheduler.Select();
                }
                else if (_running)
                {
                    Monitor.Wait(_lock);
                }
                else
                {
                    break;
                }
            }
            if (cmd != null)
            {
                cmd.Execute();
            }
        }
    }
}

public class StepCommand : ICommand.ICommand, ISteppable.ISteppable
{
    public int StepsLeft;
    public int Executed = 0;
    public StepCommand(int steps) { StepsLeft = steps; }
    public void Execute() { if (StepsLeft > 0) { StepsLeft--; Executed++; } }
    public bool IsDone => StepsLeft == 0;
}

