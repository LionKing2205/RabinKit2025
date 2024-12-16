using System;
using System.Collections.Generic;

namespace RabinKit.Database.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Group { get; set; }

    public string? Year { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
