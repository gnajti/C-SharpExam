using C_SharpExam.DTO;
using C_SharpExam.Models;
using C_SharpExam.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace C_SharpExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;

        public HomeController(ILogger<HomeController> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<EmployeeGroupDto> employees = await _employeeService.GetAll();

            return View(employees);
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
    }
}
