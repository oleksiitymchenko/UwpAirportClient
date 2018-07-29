using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class PilotDTO : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Experience { get; set; }
    }
}
