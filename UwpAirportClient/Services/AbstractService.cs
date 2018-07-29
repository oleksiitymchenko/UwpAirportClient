using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UwpAirportClient.Services
{
    public class AbstractService
    {
        private readonly string _uriAdd = "Tickets";
        private HttpClient client = new HttpClient();

        public async Task<List<TicketDTO>> getAllAsync()
        {
            HttpResponseMessage response = await client.GetAsync(Url.Value + this._uriAdd);

            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();

            HttpContent content = response.Content;
            string Json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TicketDTO>>(Json);
        }


        public async Task DeleteAsync(TicketDTO ticket)
        {
            HttpResponseMessage response = await client.DeleteAsync(Url.Value + this._uriAdd + "/" + ticket.Id);
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }

        public async Task UpdateAsync(TicketDTO ticket)
        {
            var Json = JsonConvert.SerializeObject(ticket);
            var response = await client.PutAsync(Url.Value + this._uriAdd + "/" + ticket.Id, new StringContent(Json, Encoding.UTF8, "application/json"));
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }

        public async Task CreateAsync(TicketDTO ticket)
        {
            var Json = JsonConvert.SerializeObject(ticket);
            var response = await client.PostAsync(Url.Value + this._uriAdd, new StringContent(Json, Encoding.UTF8, "application/json"));
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }
    }
}
}
