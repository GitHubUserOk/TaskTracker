using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using TaskTracker.API.DTOs.Projects;
using TaskTracker.Infrastructure;
using TaskTracker.Domain.Entities;
using TaskTracker.API.Controllers.Shared;

namespace TaskTracker.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectsController(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Project>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Project>>> GetAll()
    {
        return Ok(await _dbContext.Projects.AsNoTracking().Include(_ => _.Counterparty).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await _dbContext.Projects.AsNoTracking().Include(_ => _.Counterparty).SingleOrDefaultAsync(_ => _.Id == id);

        if (project == null) return NotFound(new NotFoundByIdResponse(typeof(Project).Name, id));

        return Ok(project);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Project>> Add([FromBody, Required] AddProjectRequest model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var counterparty = await _dbContext.Counterparties.SingleOrDefaultAsync(_ => _.Id == model.CounterpartyId);

        if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, model.CounterpartyId));

        var project = new Project()
        {
            Counterparty = counterparty,
            Name = model.Name
        };

        _dbContext.Projects.Add(project);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction("Add", project);
    }

    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Project>> Update(int id, [FromBody, Required] UpdateProjectRequest model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var project = await _dbContext.Projects.SingleOrDefaultAsync(_ => _.Id == id);

        if (project == null) return NotFound(new NotFoundByIdResponse(typeof(Project).Name, id));

        project.IsMarked = model.IsMarked;
        project.Name = model.Name;

        _dbContext.Projects.Update(project);

        await _dbContext.SaveChangesAsync();

        return Ok(project);
    }
}
