using ICommand;
using System;
using System.Collections.Generic;
using System.Threading;

namespace task17;

public class ServerThread
{
    private readonly Queue<ICommand.ICommand> _tasks = new();
    private readonly object _lock = new();
    private Thread _workerThread;
    private bool _active = true;
    private bool _pendingShutdown = false;
    public bool IsAlive => _active;

    public ServerThread()
    {
        _workerThread = new Thread(ProcessLoop) { IsBackground = true };
        _workerThread.Start();
    }

    private void ProcessLoop()
    {
        while (true)
        {
            ICommand.ICommand? task = null;
            lock (_lock)
            {
                while (_tasks.Count == 0 && _active && !_pendingShutdown)
                {
                    Monitor.Wait(_lock);
                }
                if (!_active)
                    break;
                if (_tasks.Count > 0)
                {
                    task = _tasks.Dequeue();
                }
                else if (_pendingShutdown)
                {
                    _active = false;
                    break;
                }
            }
            if (task == null && _active)
            {
                throw new InvalidOperationException("Unexpected state: task == null while worker is active");
            }
            if (task != null)
            {
                try
                {
                    task.Execute();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception in command: {exception}");
                }
            }
        }
    }

    public void EnqueueCommand(ICommand.ICommand cmd)
    {
        lock (_lock)
        {
            if (!_active) throw new InvalidOperationException("Worker is not running");
            _tasks.Enqueue(cmd);
            Monitor.PulseAll(_lock);
        }
    }

    public void HardStop()
    {
        if (Thread.CurrentThread != _workerThread)
            throw new InvalidOperationException("Immediate stop must be called from worker thread");
        lock (_lock)
        {
            _active = false;
            Monitor.PulseAll(_lock);
        }
    }

    public void SoftStop()
    {
        if (Thread.CurrentThread != _workerThread)
            throw new InvalidOperationException("Graceful stop must be called from worker thread");
        lock (_lock)
        {
            _pendingShutdown = true;
            Monitor.PulseAll(_lock);
        }
    }
}

public class HardStop : ICommand.ICommand
{
    private readonly ServerThread _worker;
    public HardStop(ServerThread worker) { _worker = worker; }
    public void Execute() => _worker.HardStop();
}

public class SoftStop : ICommand.ICommand
{
    private readonly ServerThread _worker;
    public SoftStop(ServerThread worker) { _worker = worker; }
    public void Execute() => _worker.SoftStop();
}

