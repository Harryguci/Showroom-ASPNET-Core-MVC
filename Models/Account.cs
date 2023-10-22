namespace ShowroomManagement.Models
{
    public class Account
    {
        public Account()
        {
            AccountId = string.Empty;
        }
        public string AccountId { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? DeteleAt { get; set; } = null;
    }
}
