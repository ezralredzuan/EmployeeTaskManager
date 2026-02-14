using EmployeeTaskManager.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmployeeTaskManager.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalEmployees = _context.Employees.Count();
            ViewBag.TotalTasks = _context.Tasks.Count();
            ViewBag.CompletedTasks = _context.Tasks.Count(t => t.Status == "Complete");
            ViewBag.PendingTasks = _context.Tasks.Count(t => t.Status != "Complete");

            ViewBag.Pending = _context.Tasks.Where(t => t.Status == "Pending").ToList();
            ViewBag.InProgress = _context.Tasks.Where(t => t.Status == "In Progress").ToList();
            ViewBag.Review = _context.Tasks.Where(t => t.Status == "In Review").ToList();
            ViewBag.Role = role;
            ViewBag.Complete = _context.Tasks.Where(t => t.Status == "Complete").ToList();

            return View();
        }
    }
}
