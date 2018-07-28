using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class FlightDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string FinishPoint { get; set; }

        [Required]
        public string FinishTime { get; set; }

        [Required]
        public List<int> TicketIds { get; set; }
    }
}
