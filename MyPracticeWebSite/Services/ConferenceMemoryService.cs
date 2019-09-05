using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using MyPracticeWebSite.Services;
using Shared.Models;

namespace MyPracticeWebSite.Services
{
    public class ConferenceMemoryService: IConferenceService
    {
        private readonly List<ConferenceModel> conferences = new List<ConferenceModel>();
        private readonly IDataProtector _dataProtector;

        public ConferenceMemoryService(IDataProtectionProvider dataProtProvider, PurposeKeys purposeKeys)
        {
            _dataProtector = dataProtProvider.CreateProtector(purposeKeys.ConferenceIdKey);
            conferences.Add(new ConferenceModel { Id = 1, Name = "NDC", EncryptedId= _dataProtector.Protect("1"), Location = "Oslo", Start = new DateTime(2017, 6, 12), AttendeeTotal = 2132 });
            conferences.Add(new ConferenceModel { Id = 2, Name = "IT/DevConnections", EncryptedId = _dataProtector.Protect("2"), Location = "San Francisco", Start = new DateTime(2017, 10, 18), AttendeeTotal = 3210 });
        }
        public Task Add(ConferenceModel model)
        {
            model.Id = conferences.Max(c => c.Id) + 1;
            //model.EncryptedId = _dataProtector.Protect(model.Id.ToString());
            conferences.Add(model);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ConferenceModel>> GetAll()
        {
            return Task.Run(() => conferences.AsEnumerable());
        }

        public Task<ConferenceModel> GetById(int id)
        {
            return Task.Run(() => conferences.First(c => c.Id == id));
        }

        public Task<StatisticsModel> GetStatistics()
        {
            return Task.Run(() =>
            {
                return new StatisticsModel
                {
                    NumberOfAttendees = conferences.Sum(c => c.AttendeeTotal),
                    AverageConferenceAttendees = (int)conferences.Average(c => c.AttendeeTotal)
                };
            });
        }
    }
}
