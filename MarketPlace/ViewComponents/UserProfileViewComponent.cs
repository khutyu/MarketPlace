using MarketPlace.Data;
using Microsoft.AspNetCore.Mvc;

public class UserProfileViewComponent : ViewComponent
{
    private readonly  IRepositoryWrapper _repositoryWrapper;

    public UserProfileViewComponent(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _repositoryWrapper._UserServices.GetByUsernameAsync(User.Identity.Name);
        return View(user);
    }
}