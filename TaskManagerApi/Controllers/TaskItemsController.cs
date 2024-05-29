using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskManagerRepository _taskManagerRepository;
        private readonly ILogger<TaskItemsController> _logger;

        public TaskItemsController(ITaskManagerRepository taskManagerRepository, 
            ILogger<TaskItemsController> logger)
        {
            _taskManagerRepository = taskManagerRepository;
            _logger = logger;
        }

        // GET: api/TaskItems
        [HttpGet(Name="MyTasks")]
        public async Task<ActionResult<List<TaskItem>>> GetTaskItems()
        {
            var tasks =  await _taskManagerRepository.Get();
            _logger.LogInformation("GetTaskItems method was called");
            return Ok(tasks);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}", Name = "GetTask")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var taskItem = await _taskManagerRepository.GetById(id);

            if (taskItem == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"{nameof(GetTaskItem)} method was called");
            return Ok(taskItem);
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = "UpdateTask")]
        public async Task<ActionResult> UpdateTaskItem(int id, 
            string name, 
            string description, 
            string type, 
            bool isCompleted,
            DateTime deadLine
            )
        {
            await _taskManagerRepository.UpdateTaskById(id, name, description, type, isCompleted, deadLine);
            return Ok();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name ="CreateTask")]
        public async Task<ActionResult> CreateTaskItem(int id, 
            string name, 
            string description, 
            string type, 
            bool isCompleted,
            DateTime deadLine)
        {
            await _taskManagerRepository.CreateTask(name, description, type, isCompleted, deadLine);
            return Ok();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}", Name = "DeleteTask")]
        public async Task<ActionResult> DeleteTaskItem(int id)
        {
            await _taskManagerRepository.DeleteTaskById(id);
            return Ok();
        }
    }
}
