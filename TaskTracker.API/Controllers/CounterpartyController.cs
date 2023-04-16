using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CounterpartyController : ControllerBase
    {
        private readonly ApplicationDbContext _contextDb;
        public CounterpartyController(ApplicationDbContext context)
        {
            _contextDb = context;

        }
        // GET: api/<CounterpartyController>
        [HttpGet]
        public async Task<ActionResult<Counterparty>> OnGetAll()
        {
            return Ok(await _contextDb.Counterparty.ToListAsync());
        }

/*        // GET api/<CounterpartyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST api/<CounterpartyController>
        [HttpPost]
        public async Task<ActionResult<Counterparty>> OnPost([FromBody] Counterparty counterparty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };

            _contextDb.Counterparty.Add(counterparty);

            try
            {
                await _contextDb.SaveChangesAsync();

                return Ok(counterparty);
            }
            catch (Exception ex)
            {
                return Problem("Internal Server Error", "", 500);
            }
        }

/*        // PUT api/<CounterpartyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CounterpartyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
