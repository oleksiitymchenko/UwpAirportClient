using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class DepartureDTO
    {
        public int Id { get; set; }

        public string FlightNumber { get; set; }

        public string DepartureTime { get; set; }

        public int CrewId { get; set; }

        public int PlaneId { get; set; }
    }
}
