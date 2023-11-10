using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TasksApiController : ControllerBase
    {
        private readonly ShowroomContext _context;

        public TasksApiController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: api/TasksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/TasksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(string id)
        {
            var tasks = await _context.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }

        [HttpGet("Employees/{EmployeeId}")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetFromEmployeeId(string EmployeeId)
        {
            return await _context.Tasks.Where(p => p.EmployeeId == EmployeeId).ToListAsync();
        }

        // PUT: api/TasksApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasks(string id, Tasks tasks)
        {
            if (id != tasks.Id)
            {
                return BadRequest();
            }

            _context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TasksApi
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks([Bind("EmployeeId,Content,Dateline")]Tasks tasks)
        {
            var id = _context.Tasks.Select(p => p.Id).Take(1).OrderByDescending(p => p).FirstOrDefault();
            id = (Convert.ToInt32(id.Substring(1)) + 1).ToString();
            int len = id.Length;
            for (int i = 0; i < 9 - len; i++)
            {
                id = "0" + id;
            }
            id = "T" + id;
            tasks.Id = id;

            _context.Tasks.Add(tasks);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TasksExists(tasks.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTasks", new { id = tasks.Id }, tasks);
        }

        // DELETE: api/TasksApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasks(string id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TasksExists(string id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
