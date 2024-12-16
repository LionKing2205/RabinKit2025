using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class TestValue
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string InputVars { get; set; } = null!;

    public string OutputVars { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TaskComponent Task { get; set; } = null!;

    public virtual ICollection<TaskTestAttemptRelation> TaskTestAttemptRelations { get; set; } = new List<TaskTestAttemptRelation>();
}
