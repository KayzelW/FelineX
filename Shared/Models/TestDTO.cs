using Shared.Data.Test;

namespace Shared.Models;

public class TestDTO : UniqueTest
{
    public string? FantomName { get; set; }
    public Guid? StudentId { get; set; }
}