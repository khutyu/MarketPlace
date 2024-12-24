using MarketPlace.Shared.Models;
using Microsoft.AspNetCore.Identity;

public class AdminUserServices : IAdminUserServices
{
    protected UserManager<User> _userManager;
    public AdminUserServices(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ChangeAccountStatusAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;

        user.IsSuspended = !user.IsSuspended;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<User> GetUserWithAddressAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}