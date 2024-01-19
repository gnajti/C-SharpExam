using C_SharpExam.DTO;

namespace C_SharpExam.Service
{
    public interface IEmployeeService
    {
        Task<List<EmployeeGroupDto>> GetAll();
    }
}
