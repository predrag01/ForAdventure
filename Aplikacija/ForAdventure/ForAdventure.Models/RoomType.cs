﻿using System.ComponentModel.DataAnnotations;

namespace ForAdventure.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Type { get; set; }
    }
}
