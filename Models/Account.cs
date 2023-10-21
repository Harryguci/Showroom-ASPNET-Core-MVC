namespace ShowroomManagement.Models
{
    public class Account
    {
        public Account(string accountId, string employeeId)
        {
            AccountId = accountId;
            EmployeeId = employeeId;
        }
        public string AccountId { get; set; }
        public string EmployeeId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? DeteleAt { get; set; }
    }
}
