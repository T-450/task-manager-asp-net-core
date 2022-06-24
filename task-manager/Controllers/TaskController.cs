using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_manager.Core.Models;
using task_manager.Infrastructure.Data;

namespace task_manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        public TaskController(ILogger<TaskController> logger, ApplicationDbContext ctx)
        {
            _dbContext = ctx;
            _logger = logger;
        }

        private ApplicationDbContext _dbContext { get; }
        private ILogger<TaskController> _logger { get; }

        [HttpGet]
        public OkObjectResult Get()
        {
            throw new Exception("SOMETHING WENT WRONG!");
            return Ok(_dbContext.TaskItems.AsNoTracking().ToList());
        }

        [HttpPost]
        public async Task<OkObjectResult> Add(TaskModel taskDto)
        {
            try
            {
                var entity = new TaskModel
                {
                    Name = taskDto.Name,
                    Completed = taskDto.Completed
                };
                _dbContext.TaskItems.Add(entity);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return Ok("Entity created!");
            }
            catch (Exception e)
            {
                _logger.LogError(e?.Message);
                throw;
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, TaskModel taskDto)
        {
            try
            {
                TaskModel? entityToUpdate = await _dbContext.TaskItems.FindAsync(id).ConfigureAwait(false);

                if (entityToUpdate is null) return BadRequest("Entity not found!");

                entityToUpdate.Name = taskDto.Name;
                entityToUpdate.Completed = taskDto.Completed;
                _dbContext.TaskItems.Update(entityToUpdate);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return Ok("Entity updated!");
            }
            catch (Exception e)
            {
                _logger.LogError(e?.Message);
                throw;
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                TaskModel? entityToDelete = await _dbContext.TaskItems.FindAsync(id).ConfigureAwait(false);

                if (entityToDelete is null) return BadRequest("Entity not found!");

                _dbContext.TaskItems.Remove(entityToDelete);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return Ok($"Entity with id {entityToDelete.Id} was deleted!");
            }
            catch (Exception e)
            {
                _logger.LogError(e?.Message);
                return Problem("Internal Server Error", statusCode: 500);
            }
        }
    }
}
