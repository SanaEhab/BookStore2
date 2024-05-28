using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStore.Models
{
	//this model has been created to make a many to many relation
	public class BookCategory
	{
		public int BookId { get; set; }
		public Book book { get; set; }
		public int CategoryId { get; set; }
		public Category category { get; set; }
	}
}
