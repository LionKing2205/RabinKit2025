using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class TaskAttempt
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string Code { get; set; } = null!;

    public string? Inputs { get; set; }

    public string Result { get; set; } = null!;

    public int IsPassed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TaskComponent Task { get; set; } = null!;

    public virtual ICollection<TaskTestAttemptRelation> TaskTestAttemptRelations { get; set; } = new List<TaskTestAttemptRelation>();
}
