namespace Shared.Interfaces;

public interface IInnerIdentity
{
    Guid Id { get; set; }

    // void SyncFields(ICollection<T>? existing, ICollection<T>? incoming); //TODO: using AutoMapper Lib
}