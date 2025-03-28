﻿using System.ComponentModel.DataAnnotations;

namespace AppComponents.Repository.Tests
{
    public class MockItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        public string? Value { get; set; }
    }
}
