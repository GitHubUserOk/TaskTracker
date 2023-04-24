using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using TaskTracker.API.Controllers.Shared;
using TaskTracker.API.DTOs.Counterparties;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Repositories;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class CounterpartyController : ControllerBase
    {
         private readonly ICounterpartyRepo _repo;

        public CounterpartyController(ICounterpartyRepo repo)
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

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> Update([FromBody, Required] UpdateCounterpartyRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var counterparty = await _repo.GetById(model.Id);

            if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, model.Id));

            counterparty.IsMarked = model.IsMarked;
            counterparty.Name = model.Name;

            await _repo.SaveChanges();

            return Ok(counterparty);
        }
    }
}
