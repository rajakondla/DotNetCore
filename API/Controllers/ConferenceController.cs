using System.Collections.Generic;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Linq;

namespace API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ConferenceController : Controller
    {
        private readonly IConferenceRepo repo;

        public ConferenceController(IConferenceRepo repo)
        {
            this.repo = repo;
        }

        public IActionResult GetAll()
        {
            var conferences = repo.GetAll();
            if (!conferences.Any())
                return new NoContentResult();

            return new ObjectResult(conferences);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            ConferenceModel model = repo.GetById(id);
            if (model == null)
                return new NotFoundResult();
            return new ObjectResult(model);
        }

        [HttpPost]
        public ConferenceModel Add([FromBody]ConferenceModel conference)
        {
            return repo.Add(conference);
        }


    }
}
