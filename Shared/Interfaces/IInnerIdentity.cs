namespace Shared.Interfaces;

public interface IInnerIdentity<T>
{
    Guid Id { get; set; }

    // void SyncFields(ICollection<T>? existing, ICollection<T>? incoming);
}