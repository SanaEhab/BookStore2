using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            var authors = context.Authors.OrderBy(authors => authors.Name).ToList();
            var categories = context.Categories.OrderBy(categories => categories.Name).ToList();
            //convert from authors to  selectlist authorList
            var authorList = new List<SelectListItem>();

            foreach (var author in authors)
            {
                authorList.Add(new SelectListItem
                {
                    Value = author.Id.ToString(),
                    Text = author.Name,
                });
            }

            var categoryList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                });
            }

            //from selectlistitem to viewModel
            var viewModel = new BookFormVM
            {
                Authors = authorList,
                Categories = categoryList,  
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BookFormVM viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var book = new Book
            {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                Publisher = viewModel.Publisher,
                PublishDate = viewModel.PublishDate,
                Description = viewModel.Description,
                //جبت السيليكتد وحولته ل بوك كاتيجوري
                Categories = viewModel.SelectedCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),
            };
            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
