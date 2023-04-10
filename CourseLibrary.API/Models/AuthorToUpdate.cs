﻿using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class AuthorToUpdate
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        [StringLength(50)]
        public string MainCategory { get; set; }
    }
}