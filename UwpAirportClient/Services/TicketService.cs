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
    public class TicketService
    {
        private readonly string _uriAdd = "Tickets";

        public async Task<List<TicketDTO>> getAllAsync()
        {
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Url.Value + this._uriAdd);
          //  if (response.StatusCode != HttpStatusCode.OK) return null;

            HttpContent content = response.Content;
            string Json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TicketDTO>>(Json);
        }
    }
}
