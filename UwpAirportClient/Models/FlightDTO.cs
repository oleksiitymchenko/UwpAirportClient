using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UwpAirportClient.Models
{
    public class FlightDTO : IEntity
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string StartPoint { get; set; }

        public string StartTime { get; set; }

        public string FinishPoint { get; set; }

        public string FinishTime { get; set; }

        public List<int> TicketIds { get; set; }
    }
}
