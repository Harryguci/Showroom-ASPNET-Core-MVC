using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeId = Firstname = Lastname = Cccd = Postion = string.Empty;
            Salary = 0;
        }
        [Key]
        public string EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Cccd { get; set; }
        public string Postion { get; set; }
        public DateTime? StartDate { get; set; }
        public long Salary { get; set; }
        public string? Email { get; set; }
        public string? SaleId { get; set; }
    }
}
