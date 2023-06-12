using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using TaskTracker.API.Controllers.Shared;
using TaskTracker.API.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Repositories.Interfaces;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _repo.GetById(id);

            if (user == null) return NotFound(new NotFoundByIdResponse(typeof(User).Name, id));

            return Ok(user);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<User>> Add([FromBody, Required] AddUserRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                Name = model.Name
            };

            await _repo.Add(user);
            await _repo.SaveChanges();

            return CreatedAtAction("Add", user);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> Update(int id, [FromBody, Required] UpdateUserRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _repo.GetById(id);

            if (user == null) return NotFound(new NotFoundByIdResponse(typeof(User).Name, id));

            user.IsMarked = model.IsMarked;
            user.Name = model.Name;

            await _repo.SaveChanges();

            return Ok(user);
        }
    }
}
