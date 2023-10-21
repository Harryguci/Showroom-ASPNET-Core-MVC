namespace ShowroomManagement.Models
{
    public class Source
    {
        public Source()
        {
            SourceId = Name = string.Empty;
        }
        public string SourceId { get; set; }
        public string Name { get; set; }
    }
}
