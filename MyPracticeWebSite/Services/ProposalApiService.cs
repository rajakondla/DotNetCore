using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.Models;
using System;
using MyPracticeWebSite.Services;

namespace MyPracticeWebSite
{  
    public class ProposalApiService : IProposalService
    {
        private readonly HttpClient _httpClient;

        public ProposalApiService(HttpClient httpClient)
        {
            
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        public async Task<IEnumerable<ProposalModel>> GetAll(int conferenceId)
        {
            var result = new List<ProposalModel>();
            var response = await _httpClient.GetAsync($"/v1/Proposal/{conferenceId}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<List<ProposalModel>>();
            else
                throw new HttpRequestException(response.ReasonPhrase);
            return result;
        }

        public async Task Add(ProposalModel model)
        {
            await _httpClient.PostAsJsonAsync("/v1/Proposal", model);
        }

        public async Task<ProposalModel> Approve(int proposalId)
        {
            var response = await _httpClient.PutAsync($"/v1/Proposal/{proposalId}", 
                null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ProposalModel>();
            }
            throw new ArgumentException($"Error retrieving proposal {proposalId}"+
                $" Response: {response.ReasonPhrase}");
        }
    }
}
