using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class TaskStatus
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int IsPassed { get; set; }

    public TimeSpan? SolutionTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
