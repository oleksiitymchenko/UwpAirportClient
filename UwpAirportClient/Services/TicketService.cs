using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UwpAirportClient.Models;

namespace UwpAirportClient.Services
{
    public class TicketService:AbstractService<TicketDTO>
    {
        public TicketService():base(new HttpClient(), Url.Value + "Tickets")
        {
        }
    }
}
