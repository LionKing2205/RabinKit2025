using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class PerformanceTest
{
    public int Id { get; set; }

    public int TaskDescriptionId { get; set; }

    public string PrepareScript { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public string UpdatedAt { get; set; } = null!;

    public virtual ICollection<PerformanceTestAttempt> PerformanceTestAttempts { get; set; } = new List<PerformanceTestAttempt>();

    public virtual TaskComponent TaskDescription { get; set; } = null!;
}
