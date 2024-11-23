using RabinKit.Core.Entities;
using RabinKit.Core.Exceptions;
//using System.Text.Json.Nodes;

namespace RabinKit.Core.Entities;

public class TaskAttempt : EntityBase
{
    public const string TaskComponentField = nameof(_taskComponent);

    private TaskComponent _taskComponent;

    public TaskAttempt(
        string code,
        Dictionary<string, string> parameters,
        TaskComponent taskComponents)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Inputs = parameters;
        TaskComponents = taskComponents;

        //Tests = new List<TaskTestAttemptRelation>();
    }

    private TaskAttempt()
    {
    }

    public long TaskId { get; private set; }

    public string Code { get; set; }

    public Dictionary<string, string> Inputs { get; set; }

    public string Result { get; set; }

    public bool IsPassed { get; set; }

    public TaskComponent TaskComponents
    {
        get => _taskComponent;
        set
        {
            TaskId = value?.Id ?? throw new RequiredFieldNotSpecifiedException("Задача");
            _taskComponent = value;
        }
    }

    //public List<TaskTestAttemptRelation> Tests { get; set; }

//    public bool IsPassedTests
//        => Type == AttemptTypes.Test
//           && Tests.Any()
//           && Tests.TrueForAll(x => x.Result == TestResult.Success);
}