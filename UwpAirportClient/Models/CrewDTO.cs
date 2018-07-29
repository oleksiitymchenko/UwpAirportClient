using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class CrewDTO:IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PilotId { get; set; }
        [Required]
        public List<int> StewardressIds { get; set; }

        public override string ToString()
        {
            return $"Pilot id:{PilotId}, Stewardesses:{StewardressIds.Count}";
        }
    }
}
