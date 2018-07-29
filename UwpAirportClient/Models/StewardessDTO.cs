using System;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class StewardessDTO : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }
    }
}
