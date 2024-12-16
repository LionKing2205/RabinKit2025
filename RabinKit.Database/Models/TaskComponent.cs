using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class TaskComponent
{
    public int Id { get; set; }

    public string Toolbox { get; set; } = null!;

    public string Input { get; set; } = null!;

    public string Output { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Playground { get; set; }

    public string IsTest { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<PerformanceTest> PerformanceTests { get; set; } = new List<PerformanceTest>();

    public virtual ICollection<TaskAttempt> TaskAttempts { get; set; } = new List<TaskAttempt>();

    public virtual ICollection<TestValue> TestValues { get; set; } = new List<TestValue>();
}
