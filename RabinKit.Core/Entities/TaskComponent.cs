using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace RabinKit.Core.Entities;

public class TaskComponent : EntityBase
{
    public JsonObject Toolbox { get; set; }

    public string[] Input { get; set; }

    public string[] Output { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string? Playground { get; set; }

    public bool IsTest { get; set; }

    public List<TaskAttempt> Attempts { get; set; }

    public List<TestValue> Values { get; set; }

    public List<TaskStatusR> StatusR { get; set; }

    //public List<PerformanceTest> PerformanceTests { get; set; }
}