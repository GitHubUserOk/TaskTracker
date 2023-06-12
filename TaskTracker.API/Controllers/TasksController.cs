using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using TaskTracker.API.Controllers.Shared;
using TaskTracker.API.DTOs.Projects;
using TaskTracker.API.DTOs.Tasks;
using TaskTracker.API.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;

namespace TaskTracker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TasksController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetTaskResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<GetTaskResponse>>> GetAll()
        {
            return Ok(await GetTask(0, true));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GetTaskResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetTaskResponse>> GetById(int id)
        {
            var tasks = await GetTask(id);

            if(tasks.Count() == 0)
            {
                return NotFound(new NotFoundByIdResponse(typeof(GetTaskResponse).Name, id));
            };

            return Ok(tasks[0]);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(TaskTracker.Domain.Entities.Task), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<GetTaskResponse>> Add([FromBody, Required] AddTaskRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var counterparty = await _dbContext.Counterparties.SingleOrDefaultAsync(_ => _.Id == model.CounterpartyId);

            if (counterparty == null) return NotFound(new NotFoundByIdResponse(typeof(Counterparty).Name, model.CounterpartyId));

            var project = await _dbContext.Projects.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == model.ProjectId);

            if (project == null) return NotFound(new NotFoundByIdResponse(typeof(Project).Name, model.ProjectId));

            if (counterparty.Id != project.Counterparty?.Id) return BadRequest(new BadRequestResponse("Сounterparty and counterparty of project do not match"));

            //TODO var author = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == model.AuthorId);

            //TODO if (author == null) return NotFound(new NotFoundByIdResponse(typeof(User).Name, model.AuthorId));

            var task = new TaskTracker.Domain.Entities.Task()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow,
                ProjectId = project.Id,
                CounterpartyId = counterparty.Id
                //TODO AuthorId = author.Id
            };

            TaskTracker.Domain.Entities.Task? basisTask = null;

            if (model.BasisTaskId != null)
            {
                basisTask = await _dbContext.Tasks.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == model.BasisTaskId);

                if (basisTask == null) return NotFound(new NotFoundByIdResponse(typeof(TaskTracker.Domain.Entities.Task).Name, (int)model.BasisTaskId));

                task.BasisTaskId = (int)model.BasisTaskId;
            }

            _dbContext.Tasks.Add(task);

            await _dbContext.SaveChangesAsync();

            var taskDTO = new GetTaskResponse()
            {
                Id = task.Id,
                IsMarked = task.IsMarked,
                Title = task.Title,
                Description = task.Description,
                CreatedDate = task.CreatedDate,
                //TODO AuthorId = author.Id,
                //TODO AuthorName = author.Name,
                CounterpartyId = counterparty.Id,
                CounterpartyName = counterparty.Name,
                ProjectId = project.Id,
                ProjectName = project.Name,
                BasisTaskId = basisTask?.Id,
                BasisTaskTitle = basisTask?.Title
            };

            return CreatedAtAction("Add", taskDTO);
        }

        [HttpPost("{id}/AddPerformers")]
        public async Task<ActionResult> AddPerformers(int id, [FromBody] int[] arr)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var task = await _dbContext.Tasks.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == id);

            if (task == null) return NotFound(new NotFoundByIdResponse(typeof(TaskTracker.Domain.Entities.Task).Name, id));

            foreach(var item in arr)
            {
                var user = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == item);

                if (user == null) return NotFound(new NotFoundByIdResponse(typeof(User).Name, id));
            };

            bool perfofmerAdded = false;

            foreach (var userId in arr)
            {
                var performer = await _dbContext.Performers.AsNoTracking().SingleOrDefaultAsync(_ => _.TaskId == id & _.UserId == userId);

                if(performer == null)
                {
                    var newPerformer = new Performer()
                    {
                        TaskId = task.Id,
                        UserId = userId,
                        CreatedDate = DateTime.UtcNow,
                    };

                    await _dbContext.Performers.AddAsync(newPerformer);

                    perfofmerAdded = true;
                }               
            };

            if(perfofmerAdded)
            {
                await _dbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpDelete("{id}/DeletePerformers")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeletePerformers(int id, [FromBody] int[] arr)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var performers = await _dbContext.Performers.Where(_ => _.TaskId == id).Where(_ => arr.Contains(_.UserId)).ToArrayAsync();

            if (performers.Length > 0) {
                _dbContext.Performers.RemoveRange(performers);
                await _dbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetTaskResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GetTaskResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetTaskResponse>> Update(int id, [FromBody, Required] UpdateTaskRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _dbContext.Tasks.SingleOrDefaultAsync(_ => _.Id == id);

            if (task == null) return NotFound(new NotFoundByIdResponse(typeof(TaskTracker.Domain.Entities.Task).Name, id));

            task.IsMarked = model.IsMarked;
            task.Title = model.Title;
            task.Description = model.Description;

            _dbContext.Tasks.Update(task);

            await _dbContext.SaveChangesAsync();

            var tasks = await GetTask(id);

            return Ok(tasks[0]);
        }

        private async Task<List<GetTaskResponse>> GetTask(int id, bool all = false)
        {
            var foundTasks = await(from tasks in _dbContext.Tasks.AsNoTracking()
                             where tasks.Id == id || all
                             join authors in _dbContext.Users.AsNoTracking() on tasks.AuthorId equals authors.Id into T10
                             from T100 in T10.DefaultIfEmpty()
                             join counterparties in _dbContext.Counterparties.AsNoTracking() on tasks.CounterpartyId equals counterparties.Id into T20
                             from T200 in T20.DefaultIfEmpty()
                             join projects in _dbContext.Projects.AsNoTracking() on tasks.ProjectId equals projects.Id into T30
                             from T300 in T30.DefaultIfEmpty()
                             join basis_tasks in _dbContext.Tasks.AsNoTracking() on tasks.BasisTaskId equals basis_tasks.Id into T40
                             from T400 in T40.DefaultIfEmpty()
                             select new GetTaskResponse()
                                     {
                                         Id = tasks.Id,
                                         IsMarked = tasks.IsMarked,
                                         Title = tasks.Title,
                                         Description = tasks.Description,
                                         CreatedDate = tasks.CreatedDate,
                                         AuthorId = T100.Id,
                                         AuthorName = T100.Name,
                                         CounterpartyId = T200.Id,
                                         CounterpartyName = T200.Name,
                                         ProjectId = T300.Id,
                                         ProjectName = T300.Name,
                                         BasisTaskId = T400.Id,
                                         BasisTaskTitle = T400.Title
                                     }).ToListAsync();

            var foundPerformers = await(from performers in _dbContext.Performers.AsNoTracking()
                                  orderby performers.TaskId
                                  where performers.TaskId == id || all
                                  join users in _dbContext.Users.AsNoTracking() on performers.UserId equals users.Id
                                  select new
                                  { TaskId = performers.TaskId,
                                    UserId = users.Id,
                                    UserIsMarked = users.IsMarked,
                                    UserName = users.Name
                                  }).ToListAsync();

            foreach (var task in foundTasks)
            {
                List<User> performers = new();

                var res = foundPerformers.Where(_ => _.TaskId == task.Id);

                foreach(var performer in res)
                {
                    performers.Add(new User
                                       { Id = performer.UserId,
                                         IsMarked = performer.UserIsMarked,
                                         Name = performer.UserName
                                       });
                }

                if(performers.Count() > 0)
                {
                    task.Performers = performers;
                }
            }
            return foundTasks;
        }
    }
}
