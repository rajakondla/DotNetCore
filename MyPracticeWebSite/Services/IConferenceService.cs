using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace MyPracticeWebSite.Services
{
    public interface IConferenceService
    {
        Task<IEnumerable<ConferenceModel>> GetAll();
        Task<ConferenceModel> GetById(int id);
        Task<StatisticsModel> GetStatistics();
        Task Add(ConferenceModel model); 
    }
}
