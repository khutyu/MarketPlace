using MarketPlace.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class UserProfileViewComponent : ViewComponent
{
    private readonly IRepositoryWrapper _repo;

    public UserProfileViewComponent(IRepositoryWrapper repositoryWrapper)
    {
        _repo = repositoryWrapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userId)
    {
        var user = _repo._Users.GetById(userId);
        return View(user);
    }
}