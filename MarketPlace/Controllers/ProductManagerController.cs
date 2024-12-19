using MailKit.Search;
using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ContosoUniversity.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly IRepositoryWrapper _Repository;

        public ProductManagerController(IRepositoryWrapper Repository)
        {
            _Repository = Repository;
        }

        public IActionResult Listing(string searchstring="all")
        {
            IEnumerable<Product> Products = _Repository._Products.GetProductstWithCategoryDetails();
            IEnumerable<Category> categories=_Repository._Categories.FindAll();
			if (searchstring == "all")
            {
               Products=_Repository._Products.FindAll();
            }
            else if(categories.Where(c=> c.CategoryName.ToLower() == searchstring.ToLower()).Count() > 0)
            {
                Products = Products.Where(p => p.Category.CategoryName.ToLower() == searchstring.ToLower());
            }
			else
			{
                Products = Products.Where(p => p.ProductName.Contains(searchstring, StringComparison.OrdinalIgnoreCase));
            }

            return View(Products);
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
            return View(_Repository._Products.GetById(id));
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
                ViewBag.ErrorMessage = "Unable to Delete Product";
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
