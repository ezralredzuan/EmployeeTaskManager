using EmployeeTaskManager.Data;
using EmployeeTaskManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManager.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");
    
            return View(_context.Employees.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }   

        public IActionResult Edit(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
