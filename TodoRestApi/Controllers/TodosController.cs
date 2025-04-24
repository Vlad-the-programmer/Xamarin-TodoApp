using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoRestApi.Models;
using TodoRestApi.Models.Contexts;

namespace TodoRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodosDbContext _context;

        public TodosController(TodosDbContext context)
        {
            _context = context;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.Include(t => t.TodoTags).ThenInclude(tt => tt.Tag).ToListAsync();
        }

        // GET: api/todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos
                                     .Include(t => t.TodoTags)
                                     .ThenInclude(tt => tt.Tag)
                                     .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
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

        // DELETE: api/todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/todos/search?term=someText
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Todo>>> SearchTodos([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Todos.ToListAsync();
            }

            var matchedTodos = await _context.Todos
                .Where(t => t.Content!.ToLower().Contains(term.ToLower()) ||
                            t.Title!.ToLower().Contains(term.ToLower()))
                .ToListAsync();

            return matchedTodos;
        }

        private bool TodoExists(int id) => _context.Todos.Any(t => t.Id == id);
    }
}
