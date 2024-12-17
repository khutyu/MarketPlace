using MarketPlace.Data;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

[Route("Account/Settings")]
public class SettingsController: Controller
{
    private readonly IRepositoryWrapper _Repository;

    public SettingsController(IRepositoryWrapper Repository)
    {
        _Repository = Repository;
    }
    public IActionResult Index()
    {
        return View("~/Views/Account/Settings/Index.cshtml",new AccountSettingsViewModel{});
    }
}