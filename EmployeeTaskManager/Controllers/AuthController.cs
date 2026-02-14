using EmployeeTaskManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManager.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("Username", admin.Username);
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetInt32("UserId", admin.Id);

                return RedirectToAction("Index", "Dashboard");
            }

            var employee = _context.Employees
                .FirstOrDefault(e => e.Email == username && e.Password == password);

            if (employee != null)
            {
                HttpContext.Session.SetString("Username", employee.Name);
                HttpContext.Session.SetString("Role", "Employee");
                HttpContext.Session.SetInt32("UserId", employee.Id);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
