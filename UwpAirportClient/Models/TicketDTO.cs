using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class TicketDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string FlightNumber { get; set; }
    }
}
