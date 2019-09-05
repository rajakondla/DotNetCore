using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using MyPracticeWebSite.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;

namespace MyPracticeWebSite.Controllers
{
    [Authorize]
    public class ProposalController: Controller
    {
        private readonly IConferenceService conferenceService;
        private readonly IProposalService proposalService;
        private readonly IDataProtector _dataProtector;

        public ProposalController(IConferenceService conferenceService, IProposalService proposalService,IDataProtectionProvider dataProtProvider, PurposeKeys purposeKeys)
        {
            _dataProtector = dataProtProvider.CreateProtector(purposeKeys.ConferenceIdKey);
            this.conferenceService = conferenceService;
            this.proposalService = proposalService;
        }

        public async Task<IActionResult> Index(string id)
        {
            int conferenceId = int.Parse(_dataProtector.Unprotect(id));
            var conference = await conferenceService.GetById(conferenceId);      
            ViewBag.Title = $"Proposals For Conference {conference.Name} {conference.Location}";
            ViewBag.ConferenceId = id;

            return View(await proposalService.GetAll(conferenceId));
        }

        public IActionResult Add(string id)
        {
            int conferenceId = int.Parse(_dataProtector.Unprotect(id));
            ViewBag.Title = "Add Proposal";
            return View(new ProposalModel {ConferenceId = conferenceId});
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProposalModel proposal)
        {
            if (ModelState.IsValid)
                await proposalService.Add(proposal);
            return RedirectToAction("Index", new {conferenceId = proposal.ConferenceId});
        }

        public async Task<IActionResult> Approve(int proposalId)
        {
            var proposal = await proposalService.Approve(proposalId);
            return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
        }
    }
}
