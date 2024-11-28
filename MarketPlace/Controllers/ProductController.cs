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
            return View(_Repository._Products.FindAll());
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
            return View(_Repository._Products.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                        _Repository._Products.Update(product);

                   
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
            return View(_Repository._Products.GetProducttWithCategoryDetails(id));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _Repository._Products.GetById(id);

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
                _Repository._Products.Delete(product);
                _Repository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to Delete student";
                return View(product);
            }
        }

        private void PopulateGenreDLL(object selectedGenre = null)
        {
            ViewBag.Categories = new SelectList(_Repository._Categories.FindAll(),
                "CategoryId", "CategoryName", selectedGenre);
        }
    }
}
