using MarketPlace.Data;
using Microsoft.AspNetCore.Mvc;

public class SettingsController: Controller
{
    private readonly IRepositoryWrapper _Repository;

    public SettingsController(IRepositoryWrapper Repository)
    {
        _Repository = Repository;
    }
    public IActionResult Index()
    {
        return View(_Repository._Categories.FindAll());
    }
}