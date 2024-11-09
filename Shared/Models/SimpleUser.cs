namespace Shared.Models;

public class SimpleUser
{
    private string Id { get; set; }
    private string? UserName { get; set; }
    private string? NormalizedUserName { get; set; }


    public SimpleUser(string id, string? userName, string? normalizedUserName)
    {
        Id = id;
        UserName = userName;
        NormalizedUserName = normalizedUserName;
    }
}