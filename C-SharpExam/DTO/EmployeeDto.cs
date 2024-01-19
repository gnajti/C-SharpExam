namespace C_SharpExam.DTO
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public int HoursWorked => EndTimeUtc.Subtract(StarTimeUtc).Hours;
    }
    public class EmployeeGroupDto
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public int HoursWorked { get; set; }

    }
}
