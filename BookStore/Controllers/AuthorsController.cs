using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext context;

        public AuthorsController(ApplicationDbContext context) { 
            this.context = context;
        }
        public IActionResult Index()
        {
            var authors = context.Authors.ToList();
            var authorsVM = new List<AuthorVM>();

            foreach(var item in authors)
            {
                var authorVM = new AuthorVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn,
                };
                authorsVM.Add(authorVM);
            }

            return View(authorsVM);
        }

        [HttpGet]
        public IActionResult Create() {
            return View("Form");
        }

        [HttpPost]
        public IActionResult Create(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var author = new Author()
            {
                Name = authorFormVM.Name,
            };
            try
            {
                context.Authors.Add(author);
                context.SaveChanges();  
                return RedirectToAction("Index");
            }
            catch
            {
				ModelState.AddModelError("Name", "Author name is already exist");
				return View("Form", authorFormVM);
            }

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            var viewModel = new AuthorFormVM()
            {
                Id = id,
                Name = author.Name,
            };
            return View("Form",viewModel);
        }

        [HttpPost]
        public IActionResult Edit(AuthorFormVM authorFormVM)
        {
            if(!ModelState.IsValid)
            {
                return View("Form",authorFormVM);
            }

            var author = context.Authors.Find(authorFormVM.Id);
            if (author == null)
            {
                return NotFound();
            }
            
            author.Name = authorFormVM.Name;
            author.UpdatedOn = DateTime.Now;

            context.SaveChanges();
            return RedirectToAction("Index");


        }

        public IActionResult Details(int id)
        {
            var author = context.Authors.Find(id);

            if(author == null)
            {
                return NotFound();
            }
            var viewModel = new AuthorFormVM()
            {
                Id =id,
                Name = author.Name,
				CreatedOn = author.CreatedOn,
				UpdatedOn = author.UpdatedOn,
			};
            return View(viewModel);

        }

        public IActionResult Delete(int id)
        {
            var author = context.Authors.Find(id);

            if(author == null)
            {
                return NotFound();
            }
            context.Authors.Remove(author);
            context.SaveChanges();
            return Ok();
        }
    }
}
