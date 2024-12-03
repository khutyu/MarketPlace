using MarketPlace.Models;

public interface IAdminUserServices
{
    Task<bool> ChangeAccountStatusAsync(string id);
}