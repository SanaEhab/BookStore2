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

            if(!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
            var category = new Category()
            {
                Name = categoryVM.Name,
            };
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            var category = context.Categories.Find(categoryVM.Id);
            category.Name = categoryVM.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
