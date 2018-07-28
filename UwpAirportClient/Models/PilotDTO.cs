﻿using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class PilotDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(128, MinimumLength = 1)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(128, MinimumLength = 1)]
        [Required]
        public string LastName { get; set; }

        [Range(typeof(int), "0", "100")]
        [Required]
        public int Experience { get; set; }
    }
}
