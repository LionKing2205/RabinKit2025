using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class TaskTestAttemptRelation
{
    public int TestId { get; set; }

    public int AttemptId { get; set; }

    public int Result { get; set; }

    public virtual TaskAttempt Attempt { get; set; } = null!;

    public virtual TestValue Test { get; set; } = null!;
}
