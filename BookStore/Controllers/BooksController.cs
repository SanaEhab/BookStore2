using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment webHostEnvironment;

		public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
            /*to make a join between two tables book and authors
            var books = context.Books.Include(book => book.Author).ToList();*/

            /*to make a join between book and authors tables
             * var books = context.Books
                .Include(book => book.Categories)
                .ThenInclude(book => book.category).ToList();
             */

             var books = context.Books
                .Include(book => book.Author)
                .Include(book => book.Categories)
                .ThenInclude(book => book.category)
                .ToList();
            //second way of mapping (select)
            var bookVM = books.Select(book => new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author.Name,
                Publisher = book.Publisher,
                PublishDate = book.PublishDate,
                ImageURL = book.ImageURL,
                Categories = book.Categories.Select(book => book.category.Name).ToList(),

            }).ToList(); ;

			//first way of mapping (foreach)

			/*var bookVM = new List<BookVM>();

            foreach (var book in books)
            {
                var bookvm = new BookVM
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author.Name,
                    Publisher = book.Publisher,
                    PublishDate = book.PublishDate,
                    ImageURL = book.ImageURL,
                    Categories = new List<string>(),
                };
                //add each category in the categories list 
                foreach (var c in book.Categories)
                {
                    bookvm.Categories.Add(c.category.Name);
				}

                bookVM.Add(bookvm);
            }*/

			return View(bookVM);
		}


		[HttpGet]
        public IActionResult Create()
        {
            //بدي ابعت داتا لصفحة الكرييت (الداتا يلي بدها تبين جوا الليست) عشان اعرضها
            var authors = context.Authors.OrderBy(authors => authors.Name).ToList();
            var categories = context.Categories.OrderBy(categories => categories.Name).ToList();

            //convert from authors that come from the database to  selectlist authorList
            var authorList = new List<SelectListItem>();

            foreach (var author in authors)
            {
                authorList.Add(new SelectListItem
                {
                    //value.. the thing that will be send to the database
                    Value = author.Id.ToString(),
                    //text.. the thing that will appear to the user
                    Text = author.Name,
                });
            }

			//convert from category that come from the database to  selectlist categoryList
			var categoryList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                });
            }

            //convert from selectlistitem to viewModel
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
            
            string imageName = null; //عرفتها برا عشان اقدر استخدمها جوا اللوب تاعت البوك
			
            //how to deal with files(upload file)
			if (viewModel.ImageURL != null)
            {
                //to get the image name
                imageName = Path.GetFileName(viewModel.ImageURL.FileName);

                //to store the image name in the wwwroot in images in books folder(specific path) 
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/images/books",imageName);
                //to create file with specific path
                var stream = System.IO.File.Create(path);
                //copy the image to stream 
                viewModel.ImageURL.CopyTo(stream);
            }



            //convert the viewModel data to model before we send it to the database
            var book = new Book
            {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                Publisher = viewModel.Publisher,
                PublishDate = viewModel.PublishDate,
                Description = viewModel.Description,
                ImageURL = imageName,
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

        public ActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book is null)
            {
                return NotFound();
            }

            //to delete the image

			var path = Path.Combine(webHostEnvironment.WebRootPath,"images/books", book.ImageURL);
            if(System.IO.File.Exists(path))
            {
				System.IO.File.Delete(path);
			}
            

			context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
