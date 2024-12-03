using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepositoryWrapper _Repository;

        public ProductsController(IRepositoryWrapper Repository)
        {
            _Repository = Repository;
        }

        public IActionResult Index()
        {
            return View(_Repository._Products.FindAllAsync());
        }

        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            PopulateGenreDLL();
            return View("Edit", new Product());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            PopulateGenreDLL();
            return View(_Repository._Products.GetByIdAsync(id));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                
                        _Repository._Products.UpdateAsync(product);

            
                    _Repository.Save();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                    PopulateGenreDLL();
                }
            }
            ViewBag.Action = (product.ProductId == 0) ? "Add" : "Edit";
            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View(_Repository._Products.GetWithCategoryDetailsAsync(id));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _Repository._Products.GetByIdAsync(id);

            if (student == null)
                return NotFound();
            else
            {
                return View(student);
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(Product product)
        {
            if (product != null)
            {
                _Repository._Products.DeleteAsync(product);
                _Repository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to Delete student";
                return View(product);
            }
        }

        private async void PopulateGenreDLL(object selectedGenre = null)
        {
            ViewBag.Categories =   new SelectList( await _Repository._Categories.FindAllAsync(),
                "CategoryId", "CategoryName", selectedGenre);
        }
    }
}
