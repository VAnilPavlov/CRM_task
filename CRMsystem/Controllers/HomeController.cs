using CRMsystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ExceptionServices;
using System.Security.Claims;

namespace CRMsystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;
        private readonly IConfiguration _configuration;
        private Dictionary<Guid,EmployeeForView> employeeForViews = new Dictionary<Guid, EmployeeForView>();

        public HomeController(ILogger<HomeController> logger, ApplicationContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
            DbInitializer.Initialize(_db);
            if (employeeForViews.Count == 0)
            {
                foreach (var i in _db.employees.ToList())
                {
                    if (!employeeForViews.ContainsKey(i.Id))
                        employeeForViews.Add(i.Id, new EmployeeForView());
                    var emp = employeeForViews[i.Id];
                    emp.Id = i.Id;
                    emp.Title = i.Title;
                    emp.FullName = i.FullName;
                }
            }           
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (login != _configuration["LoginAdmin:login"] || password != _configuration["LoginAdmin:password"])
                return Json(new { success = false });

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var context = HttpContext;
            context.Response.Cookies.Append("Tk", encodedJwt);

            return Json(new
            {
                success = true,
                access_token = encodedJwt,
                username = login
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(string fullName, string title)
        {
            var newEmployee = new Employee { Id = Guid.NewGuid(), FullName= fullName, Title= title};
            _db.employees.Add(newEmployee);
            _db.SaveChanges();
            employeeForViews.Add(newEmployee.Id,new EmployeeForView { Id= newEmployee.Id, FullName = newEmployee.FullName, Title = newEmployee.Title});
            return RedirectToAction("Index");
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteEmployee([FromBody] RequestForDelete req)
        {
            var employee = (from e in _db.employees where e.Id == req.Id select e).FirstOrDefault();
            if (employee != null) 
            {
                _db.employees.Remove(employee!);
                employeeForViews.Remove(employee.Id);
                _db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditEmployee(Employee employee)
        {
            var employeew = (from e in _db.employees where e.Id == employee.Id select e).FirstOrDefault();

            if (employeew != null)
            {
                employeew.FullName = employee.FullName;
                employeew.Title = employee.Title;
                _db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditTasks(Models.Task task)
        {
            var taskForChange = (from t in _db.tasks where t.Id == task.Id select t).FirstOrDefault();

            if (taskForChange != null)
            {
                taskForChange.Title = task.Title;
                taskForChange.Description = task.Description;
                taskForChange.StartDate = task.StartDate;
                taskForChange.EndDate = task.EndDate;

                _db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [Authorize]
        public IActionResult Report()
        {
            var list = new List<ReportData>();
            
            var select = (from t in _db.tasks 
                          join e in _db.employees 
                          on t.EmployeeId equals e.Id 
                          select new ReportData { FullName = e.FullName, Title = t.Title, StartDate = t.StartDate, EndDate = t.EndDate, Percent = t.Percent, Overdue = CheckEndDate(t.EndDate) }).ToList();

            return View(select);
        }

        private static int CheckEndDate(DateTime endDate)
        {
            DateTime currentDate = DateTime.Now;
            if (currentDate > endDate)
            {
                int daysPassed = (currentDate - endDate).Days;
                return daysPassed;
            }
            else
            {
                return 0;
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            RefreshDataForEmployeeForViews();
            return View(employeeForViews);
        }

        private void RefreshDataForEmployeeForViews()
        {
            foreach (var i in _db.tasks.ToList())
            {
                var emp = employeeForViews[i.EmployeeId];
                emp.TaskCount++;
                emp.TaskDonePercent += i.Percent;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Tasks()
        {
            return View((_db.tasks.ToList(), _db.employees.ToList()));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Tasks(Models.Task task)
        {
            var newTask = new Models.Task { Id = Guid.NewGuid(), EmployeeId = task.EmployeeId, Title = task.Title, Description = task.Description, StartDate = task.StartDate, EndDate = task.EndDate };
            _db.tasks.Add(newTask);
            _db.SaveChanges();
            return Json(new { success = true});
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteTask([FromBody] RequestForDelete req)
        {
            var task = _db.tasks.Find(req.Id);
            if (task != null)
            {
                _db.tasks.Remove(task);
                _db.SaveChanges();
            }
            return Json(new { success = true});
        }
    }
}