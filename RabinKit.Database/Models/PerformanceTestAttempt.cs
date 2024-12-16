using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class PerformanceTestAttempt
{
    public int Id { get; set; }

    public string Runs { get; set; } = null!;

    public int PerformanceTestId { get; set; }

    public string CreatedAt { get; set; } = null!;

    public string UpdatedAt { get; set; } = null!;

    public virtual PerformanceTest PerformanceTest { get; set; } = null!;
}
