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
    public class AbstractService<TEntity> where TEntity : IEntity
    {
        private HttpClient _client;
        private string BasicUrl;
        /// <summary>
        /// basic url like : http://localhost:3445/api/Tickets
        /// </summary>
        /// <param name="basicUrl"></param>
        public AbstractService(HttpClient client, string basicUrl)
        {
            this._client = client;
            this.BasicUrl = basicUrl;
        }

        public virtual async Task<List<TEntity>> getAllAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(BasicUrl);

            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();

            HttpContent content = response.Content;
            string Json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TEntity>>(Json);
        }


        public virtual async Task DeleteAsync(TEntity ticket)
        {
            HttpResponseMessage response = await _client.DeleteAsync(BasicUrl + "/" + ticket.Id);
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }

        public virtual async Task UpdateAsync(TEntity ticket)
        {
            var Json = JsonConvert.SerializeObject(ticket);
            var response = await _client.PutAsync(BasicUrl + "/" + ticket.Id, new StringContent(Json, Encoding.UTF8, "application/json"));
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }

        public virtual async Task CreateAsync(TEntity ticket)
        {
            var Json = JsonConvert.SerializeObject(ticket);
            var response = await _client.PostAsync(BasicUrl, new StringContent(Json, Encoding.UTF8, "application/json"));
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException();
        }
    }
}
