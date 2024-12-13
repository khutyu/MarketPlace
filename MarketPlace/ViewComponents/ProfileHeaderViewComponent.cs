using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;

public class ProfileHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(User model)
    {
        return View(model);
    }
}