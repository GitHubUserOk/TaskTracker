using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mime;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class CounterpartyController : ControllerBase
    {
        private readonly ApplicationDbContext _contextDb;
        public CounterpartyController(ApplicationDbContext contextDb)
        {
            _contextDb = contextDb;

        }
        // GET: api/v1
        [HttpGet]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> GetAll()
        {
            return Ok(await _contextDb.Counterparty.ToListAsync());
        }

        // GET api/v1/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> GetById(int id)
        {
            if(id <= 0) return BadRequest();
            
            var counterparty = await _contextDb.Counterparty.SingleOrDefaultAsync(x => x.Id == id);

            if(counterparty == null) return NotFound($"Counterparty with {id} not found");

            return Ok(counterparty);
        }

        // POST api/v1
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Counterparty>> Create([FromBody] Counterparty counterparty)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _contextDb.Counterparty.Add(counterparty);

            await _contextDb.SaveChangesAsync();

            return CreatedAtAction("CounterpartyCreated", counterparty);
        }

        // PUT api/v1
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Counterparty), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Counterparty>> Put([FromBody] Counterparty incomingCounterparty)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (incomingCounterparty.Id <= 0) return BadRequest();

            var counterparty = await _contextDb.Counterparty.SingleOrDefaultAsync(x => x.Id == incomingCounterparty.Id);

            if (counterparty == null) return NotFound($"Counterparty with {incomingCounterparty.Id} not found");

            counterparty.FillPropertyValues(incomingCounterparty);

            _contextDb.Counterparty.Update(counterparty);

            await _contextDb.SaveChangesAsync();

            return Ok(counterparty);
        }
    }
}
