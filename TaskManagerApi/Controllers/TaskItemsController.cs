using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly List<TaskItem> _context;

        public TaskItemsController()
        {
            _context = new List<TaskItem>() {
                new TaskItem() {Id = 1, Name = "End Bars education", Description="End succesfully", IsCompleted=false },
                new TaskItem() {Id = 2, Name = "Enter Bars Group team", Description="I will do it", IsCompleted=false }
            };
        }

        // GET: api/TaskItems
        [HttpGet(Name="MyTasks")]
        public IActionResult GetTaskItems()
        {
            return Ok(_context);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult GetTaskItem(int id)
        {
            TaskItem? item = _context.Where(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name ="UpdateTask")]
        public IActionResult UpdateTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id || taskItem == null)
            {
                return BadRequest();
            }

            TaskItem item = _context.Where(x =>x.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            item.Name = taskItem.Name;
            item.Description = taskItem.Description;
            item.IsCompleted = taskItem.IsCompleted;
            return Ok(item);
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name ="CreateTask")]
        public IActionResult CreateTaskItem(TaskItem taskItem)
        {
            if (taskItem == null)
            {
                return null;
            }
            _context.Add(taskItem);
            return Ok();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}", Name = "DeleteTask")]
        public IActionResult DeleteTaskItem(int id)
        {
            TaskItem? item = _context.Where(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                return BadRequest();
            }
            _context.Remove(item);
            return Ok(item);
        }
    }
}
