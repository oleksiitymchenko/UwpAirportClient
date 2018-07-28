using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class CrewDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PilotId { get; set; }
        [Required]
        public List<int> StewardressIds { get; set; }
    }
}
