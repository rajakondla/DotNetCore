﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shared.Models;

namespace MyPracticeWebSite.Services
{
    public class ProposalMemoryService: IProposalService
    {
        private readonly List<ProposalModel> proposals = new List<ProposalModel>();
        private readonly IDataProtector _dataProtector;

        public ProposalMemoryService(IDataProtectionProvider dataProtProvider,PurposeKeys purposeKeys)
        {
            _dataProtector = dataProtProvider.CreateProtector(purposeKeys.ProposalIdKey);
            proposals.Add(new ProposalModel
            {
                Id = 1,
               EncryptedId=_dataProtector.Protect("1"),
                ConferenceId = 1,
                Speaker = "Roland Guijt",
                Title = "Understanding ASP.NET Core Security"
            });
            proposals.Add(new ProposalModel
            {
                Id = 2,
                EncryptedId = _dataProtector.Protect("2"),
                ConferenceId = 2,
                Speaker = "John Reynolds",
                Title = "Starting Your Developer Career"
            });
            proposals.Add(new ProposalModel
            {
                Id = 3,
                EncryptedId = _dataProtector.Protect("3"),
                ConferenceId = 2,
                Speaker = "Stan Lipens",
                Title = "ASP.NET Core TagHelpers"
            });
        }
        public Task<IEnumerable<ProposalModel>> GetAll(int conferenceId)
        {
            return Task.Run(() => proposals.Where(p=>p.ConferenceId==conferenceId));
        }

        public Task Add(ProposalModel model)
        {
            model.Id = proposals.Max(p => p.Id) + 1;
            proposals.Add(model);
            return Task.CompletedTask;
        }

        public Task<ProposalModel> Approve(int proposalId)
        {
            return Task.Run(() =>
            {
                var proposal = proposals.First(p => p.Id == proposalId);
                proposal.Approved = true;
                return proposal;
            });
        }
    }
}
