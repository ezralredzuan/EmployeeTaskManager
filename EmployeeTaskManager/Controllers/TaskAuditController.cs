using EmployeeTaskManager.Data;
using EmployeeTaskManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace EmployeeTaskManager.Controllers
{
    public class TaskAuditController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskAuditController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
                return RedirectToAction("Index", "Dashboard");

            var logs = _context.TaskAudits
                .Include(a => a.TaskItem)
                .OrderByDescending(a => a.ActionDate)
                .ToList();

            return View(logs);
        }

        public IActionResult ExportExcel()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToAction("Index", "Dashboard");

            var logs = _context.TaskAudits
                .Include(a => a.TaskItem)
                .OrderByDescending(a => a.ActionDate)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Task Logs");

                worksheet.Cell(1, 1).Value = "Task";
                worksheet.Cell(1, 2).Value = "Action";
                worksheet.Cell(1, 3).Value = "User ID";
                worksheet.Cell(1, 4).Value = "Date";

                for (int i = 0; i < logs.Count; i++)
                {
                    var row = i + 2;

                    worksheet.Cell(row, 1).Value = logs[i].TaskItem?.Title;
                    worksheet.Cell(row, 2).Value = logs[i].Action;
                    worksheet.Cell(row, 3).Value = logs[i].UserId;
                    worksheet.Cell(row, 4).Value = logs[i].ActionDate.ToString("yyyy-MM-dd HH:mm");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TaskLogs.xlsx"
                    );
                }
            }
        }
    }
}
