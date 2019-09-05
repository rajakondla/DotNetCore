using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MyPracticeWebSite.Services;
using Shared.Models;

namespace MyPracticeWebSite.Services
{
    public class ConferenceAPIService : IConferenceService
    {
        private readonly HttpClient _httpClient;

        public ConferenceAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        //public ConferenceAPIService(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClient = httpClientFactory.CreateClient("MyAPIService");
        //}

        public async Task Add(ConferenceModel model)
        {
            await _httpClient.PostAsJsonAsync("v1/conference", model);
        }

        public async Task<IEnumerable<ConferenceModel>> GetAll()
        {
            IEnumerable<ConferenceModel> result;
            var response = await _httpClient.GetAsync("v1/conference");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<IEnumerable<ConferenceModel>>();
            }
            else
                throw new HttpRequestException(response.ReasonPhrase);
            return result;
        }

        public async Task<ConferenceModel> GetById(int id)
        {
            ConferenceModel result = null;
            var response = await _httpClient.GetAsync($"v1/conference/{id}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<ConferenceModel>();
            else
                new HttpRequestException(response.ReasonPhrase);
            return result;
        }

        public async Task<ConferenceModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

            public async Task<StatisticsModel> GetStatistics()
        {
            var result = new StatisticsModel();
            var response = await _httpClient.GetAsync($"/v1/Statistics");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<StatisticsModel>();

            return result;
        }
    }
}
