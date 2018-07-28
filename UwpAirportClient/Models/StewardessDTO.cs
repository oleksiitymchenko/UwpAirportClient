using System;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class StewardessDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
