using RabinKit.Core.Exceptions;

namespace RabinKit.Core.Entities;

public class TestValue : EntityBase
{
    public const string TaskField = nameof(_task);

    public long TaskId { get; set; }

    public Dictionary<string, string> InputVars { get; set; }

    public Dictionary<string, string> OutputVars { get; set; }

    private TaskComponent _task;

    public TaskComponent Task
    {
        get => _task;
        set
        {
            TaskId = value?.Id ?? throw new RequiredFieldNotSpecifiedException("Задача");
            _task = value;
        }
    }

    // public List<TaskTestAttemptRelation> Attempts { get; set; }
}