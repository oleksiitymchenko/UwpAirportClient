﻿using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class DepartureDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(8, MinimumLength = 3)]
        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public string DepartureTime { get; set; }

        [Required]
        public int CrewId { get; set; }

        [Required]
        public int PlaneId { get; set; }
    }
}
