using EmployeeTaskManager.Data;
using EmployeeTaskManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.Tasks.Include(t => t.Employee).ToList();
            return View(tasks);
        }

        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name");

            var task = new TaskItem();
            return View(task);
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name");
                Console.WriteLine("ModelState invalid:");
                foreach (var kvp in ModelState)
                {
                    Console.WriteLine($"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(task);
            }

            _context.Tasks.Add(task);
            _context.SaveChanges();

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            _context.TaskAudits.Add(new TaskAudit
            {
                TaskItemId = task.Id,
                UserId = userId,
                Action = "Created",
                ActionDate = DateTime.Now
            });
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name");
            return View(_context.Tasks.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name");
                return View(task);
            }

            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateStatus(int taskId, string status)
        {
            var task = _context.Tasks.Find(taskId);
            if (task == null)
                return NotFound();

            var previousStatus = task.Status;

            if (previousStatus != status)
            {
                task.Status = status;

                var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                _context.TaskAudits.Add(new TaskAudit
                {
                    TaskItemId = taskId,
                    UserId = userId,
                    Action = $"Status changed: {previousStatus} → {status}",
                    ActionDate = DateTime.Now
                });

                _context.SaveChanges();
            }

            return Ok();
        }
    }
}
