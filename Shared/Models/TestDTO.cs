using Shared.DB.Classes.Test;

namespace Shared.Models;

public class TestDTO : Test
{
    public string? FantomName { get; set; } = "";
    public Guid? StudentId { get; set; }

    public TestDTO()
    {
        
    }
}
