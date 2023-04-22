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
public class ProjectController : ControllerBase
{
    private readonly ApplicationDbContext _contextDb;
    public ProjectController(ApplicationDbContext contextDb)
    {
        _contextDb = contextDb;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Project>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Project>>> GetAll()
    {
        return Ok(await _contextDb.Project.Include(_ => _.Counterparty).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await _contextDb.Project.Include(_ => _.Counterparty).SingleOrDefaultAsync(_ => _.Id == id);

        if (project == null) return NotFound(new NotFoundByIdResponse(typeof(Project).Name, id));

        return Ok(project);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Project>> AddProject([FromBody, Required] AddProjectRequest model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var counterparty = await _contextDb.Counterparty.SingleOrDefaultAsync(_ => _.Id == model.CounterpartyId);

        if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, model.CounterpartyId));

        var project = new Project()
        {
            Counterparty = counterparty,
            Name = model.Name
        };

        _contextDb.Project.Add(project);

        await _contextDb.SaveChangesAsync();

        return CreatedAtAction("AddProject", project);
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Project), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Project>> UpdateProject([FromBody, Required] UpdateProjectRequest model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var project = await _contextDb.Project.Include(_ => _.Counterparty).SingleOrDefaultAsync(_ => _.Id == model.Id);

        if (project == null) return NotFound(new NotFoundByIdResponse(typeof(Project).Name, model.Id));

        project.IsMarked = model.IsMarked;
        project.Name = model.Name;

        _contextDb.Project.Update(project);

        await _contextDb.SaveChangesAsync();

        return Ok(project);
    }
}
