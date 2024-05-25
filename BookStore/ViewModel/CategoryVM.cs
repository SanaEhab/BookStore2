﻿using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage="please enter name")]
        [MaxLength(30, ErrorMessage = "the max char equal 30")]
        public string Name { get; set; } = null!;
    }
}
