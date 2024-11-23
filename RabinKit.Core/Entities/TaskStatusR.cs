using RabinKit.Core.Exceptions;

namespace RabinKit.Core.Entities;

public class TaskStatusR : EntityBase
{
    public const string TaskComponentField = nameof(_taskComponent);

    private TaskComponent _taskComponent;

    public long TaskId { get;  set; }
    public bool IsPassed { get; set; }
    public TimeSpan SolutionTime { get; set; }
    public TaskComponent TaskComponents
    {
        get => _taskComponent;
        set
        {
            TaskId = value?.Id ?? throw new RequiredFieldNotSpecifiedException("Задача");
            _taskComponent = value;
        }
    }
}