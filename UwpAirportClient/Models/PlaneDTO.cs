using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class PlaneDTO : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TypePlaneId { get; set; }

        public string Created { get; set; }

        public string LifeTime { get; set; }
    }
}
