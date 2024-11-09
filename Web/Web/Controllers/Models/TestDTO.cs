using Web.Data.Test;

namespace Web.Controllers.Models;

public class TestDTO : UniqueTest
{
    public string? FantomName { get; set; }
    public Guid? StudentId { get; set; }
}