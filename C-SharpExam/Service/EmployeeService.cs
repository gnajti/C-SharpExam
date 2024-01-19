using RestSharp.Authenticators;
using RestSharp;
using System.Threading;
using C_SharpExam.DTO;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace C_SharpExam.Service
{
    public class EmployeeService : IEmployeeService
    {

        private readonly AppSettings _settings;
        public EmployeeService(AppSettings settings)
        {
            _settings = settings;
        }

        public async Task<List<EmployeeGroupDto>> GetAll()
        {
            var options = new RestClientOptions(_settings.ApiEndpoint);
            var client = new RestClient(options);

            var request = new RestRequest();

            var employeesL = await client.GetAsync(request);

            var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(employeesL.Content);

            var newListEmployees = new List<EmployeeGroupDto>();
            
            foreach(var employee in employees) 
            {
                var existingEmployee = newListEmployees.Where(x=> x.EmployeeName == employee.EmployeeName).FirstOrDefault();

                if (existingEmployee != null)
                {
                    existingEmployee.HoursWorked += employee.HoursWorked;
                }
                else
                {
                    newListEmployees.Add(new EmployeeGroupDto
                    {
                        EmployeeName = employee.EmployeeName,
                        StarTimeUtc = DateTime.UtcNow,
                        EndTimeUtc = DateTime.UtcNow,
                        HoursWorked = employee.HoursWorked,
                        Id = employee.Id
                    });
                }
            }

            return newListEmployees.OrderByDescending(x=> x.HoursWorked).ToList();
        }
    }
}
