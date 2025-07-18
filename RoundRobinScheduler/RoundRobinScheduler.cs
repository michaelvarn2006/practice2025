namespace RoundRobinScheduler;

using ICommand;
using ISteppable;

public class RoundRobinScheduler : IScheduler.IScheduler
{
    private readonly Queue<ICommand> _commands = new();
    public bool HasCommand() => _commands.Count > 0;
    public ICommand? Select()
    {
        int count = _commands.Count;
        while (count-- > 0)
        {
            var cmd = _commands.Dequeue();
            if (cmd is ISteppable steppable && steppable.IsDone)
                continue;
            _commands.Enqueue(cmd);
            return cmd;
        }
        return null;
    }
    public void Add(ICommand cmd) => _commands.Enqueue(cmd);
}

