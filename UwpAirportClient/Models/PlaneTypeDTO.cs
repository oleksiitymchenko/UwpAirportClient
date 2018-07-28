using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class PlaneTypeDTO
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int Places { get; set; }

        public double Carrying { get; set; }
    }
}
