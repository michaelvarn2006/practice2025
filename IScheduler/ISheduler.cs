using ICommand;
namespace IScheduler;

public interface IScheduler
{
    bool HasCommand();
    ICommand.ICommand? Select();
    void Add(ICommand.ICommand cmd);
}

