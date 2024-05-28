using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class AuthorFormVM
    {
        public int Id { get; set; }

        [MaxLength(50,ErrorMessage ="The name field can't exceed 50 characters")]
        public string Name { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

	}
}
