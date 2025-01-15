using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Data.Test;

public class TestLink
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MinLength(5), MaxLength(100)] public string Name { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset Expiration { get; set; } = DateTimeOffset.Now.AddDays(7);

    public UniqueTest Test { get; set; }
    public Guid TestId { get; set; }

    public TestSettings TestSettings { get; set; }
    public Guid TestSettingsId { get; set; }

    public TestLink()
    {
        Name = Id.ToString();
    }
}