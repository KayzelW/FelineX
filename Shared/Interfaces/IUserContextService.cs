using System.Security.Claims;
using Shared.Types;

namespace Shared.Interfaces;

public interface IUserContextService
{
    public string? UserToken { get; }
    public string UserName { get; }
    public uint? Access { get; }
    public bool IsAuthorized { get; }
    public Task AuthorizeAsync(string login, string password);
    public bool HasAccess(AccessLevel accessLevel);
    public bool HasAccess(uint accessLevel);
    public ClaimsPrincipal ClaimsPrincipal { get; }
    public void Logout();
}