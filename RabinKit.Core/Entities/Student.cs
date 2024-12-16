using RabinKit.Core.Exceptions;

namespace RabinKit.Core.Entities;

public class Student : EntityBase
{
    public string? Name { get; set; }
    public string? Group { get; set; }
    public string? Year { get; set; }

}