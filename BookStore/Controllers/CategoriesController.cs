using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var category = context.Categories.ToList();
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM) { 
            //model state used for validation(makes validation without request to the data base)
            if(!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
            var category = new Category()
            {
                Name = categoryVM.Name,
            };

            try
            {

                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Category name is already exist");
                return View(categoryVM);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //to save the data into the input element when i click on edit
            var category = context.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryVM
            {
                Id = id,
                Name = category.Name,
            };
            return View("Create",viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if(!ModelState.IsValid)
            {
                return View("Create",categoryVM);
            }
            //take the data from the view and update the data base
            var category = context.Categories.Find(categoryVM.Id);

            if(category is null)
            {
                //notfound is build in 
                return NotFound();
            }
            category.Name = categoryVM.Name;
            //when i make any updates the updated on will take the update time
            category.UpdatedOn = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var categories = context.Categories.Find(id);
            if (categories is null)
            {
                return NotFound();
            }
            var categoryVM = new CategoryVM
            {
                Id = id,
                Name = categories.Name,
                CreatedOn = categories.CreatedOn,
                UpdatedOn = categories.UpdatedOn,
            };
            return View(categoryVM);
        }

        public IActionResult Delete(int id)
        {
            var categories = context.Categories.Find(id);
            if(categories is null)
            {
                return NotFound();
            }
            context.Categories.Remove(categories);
            context.SaveChanges();
            //build in ok return status200 which means that the requset is done
            return Ok();
        }
    }
}
