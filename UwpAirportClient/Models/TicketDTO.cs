﻿
namespace UwpAirportClient.Models
{
    public class TicketDTO : IEntity
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public string FlightNumber { get; set; }
    }
}
