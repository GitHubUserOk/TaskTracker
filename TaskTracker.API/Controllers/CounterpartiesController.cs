using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using TaskTracker.API.Controllers.Shared;
using TaskTracker.API.DTOs.Counterparties;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Repositories.Interfaces;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class CounterpartiesController : ControllerBase
    {
        private readonly ICounterpartyRepository _repo;

        public CounterpartiesController(ICounterpartyRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Counterparty>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Counterparty>>> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> GetById(int id)
        {          
            var counterparty = await _repo.GetById(id);

            if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, id));
 
            return Ok(counterparty);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Counterparty>> Add([FromBody, Required] AddCounterpartyRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var counterparty = new Counterparty
            {
                Name = model.Name
            };

            await _repo.Add(counterparty);
            await _repo.SaveChanges();

            return CreatedAtAction("Add", counterparty);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> Update(int id, [FromBody, Required] UpdateCounterpartyRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var counterparty = await _repo.GetById(id);

            if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, id));

            counterparty.IsMarked = model.IsMarked;
            counterparty.Name = model.Name;

            await _repo.SaveChanges();

            return Ok(counterparty);
        }
    }
}
