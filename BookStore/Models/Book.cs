using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
	public class Book
	{
		public int Id { get; set; }

		[MaxLength(50)]
		public string Title { get; set; } = null!;

		//to make a foregin key

		public int AuthorId { get; set; }
		public Author Author { get; set; }
		public string Publisher { get; set; } = null!;
		public DateTime PublishDate { get; set; }
		public string? ImageURL { get; set; }
		public string Description { get; set; } = null!;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		//many to many relation
		public List<BookCategory> Categories { get; set; } = new List<BookCategory>();	
	}
}
