using RabinKit.Core.Exceptions;

namespace RabinKit.Core.Entities;

public class PerformanceTest : EntityBase
{
    public const string TaskComponentField = nameof(_taskComponent);

    private TaskComponent _taskComponent;

    public PerformanceTest()
    {
        Attempts = new List<PerformanceTestAttempt>();
    }

    public long TaskComponentId { get; private set; }

    public TaskComponent TaskComponent
    {
        get => _taskComponent;
        set
        {
            TaskComponentId = value?.Id ?? throw new RequiredFieldNotSpecifiedException("Задача");
            _taskComponent = value;
        }
    }

    public List<PerformanceTestAttempt> Attempts { get; set; }

}