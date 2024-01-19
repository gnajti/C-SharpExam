using C_SharpExam.DTO;
using C_SharpExam.Models;
using C_SharpExam.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C_SharpExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IEmployeeService employeeService, IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<EmployeeGroupDto> employees = await _employeeService.GetAll();
            Cache.Cache.Employees = employees;
            return View(employees);
        }
        public IActionResult GeneratePieChart()
        {
            var employeeData = Cache.Cache.Employees;
            int width = 600;
            int height = 600;
            int radius = (int)(Math.Min(width, height) * 7 / 24);
            string noName = "No Name";

            var bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                double total = employeeData.Sum(x => x.HoursWorked);
                double startAngle = 0;
                int listItemHeight = 20;
                int listTopMargin = (height / 2) - (employeeData.Count() * listItemHeight / 2);

                int circleX = (width / 3) * 2; 
                int circleY = height / 2;

                for (int i = 0; i < employeeData.Count; i++)
                {
                    double sweepAngle = (employeeData[i].HoursWorked / total) * 360;

                    using (Brush brush = new SolidBrush(GetRandomColor()))
                    {
                        int listItemY = listTopMargin + i * listItemHeight;
                        string listItemText = $"{employeeData[i].EmployeeName ?? noName}: {Math.Round(employeeData[i].HoursWorked / total * 100, 2)}%";
                        int squareSize = 15;

                        graphics.FillRectangle(brush, 10, listItemY, squareSize, squareSize);
                        graphics.DrawString(listItemText, new Font("Arial", 10), Brushes.Black, 10 + squareSize + 5, listItemY);

                        graphics.FillPie(brush, circleX - radius, circleY - radius, 2 * radius, 2 * radius, (float)startAngle, (float)sweepAngle);
                    }

                    startAngle += sweepAngle;
                }
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string picturesPath = Path.Combine(webRootPath, "pictures");

            if (!Directory.Exists(picturesPath))
            {
                Directory.CreateDirectory(picturesPath);
            }

            string imagePath = Path.Combine(picturesPath, "pie_chart.png");
            bitmap.Save(imagePath, ImageFormat.Png);
            return PhysicalFile(imagePath, "image/png", "pie_chart.png");
        }
        private Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
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
