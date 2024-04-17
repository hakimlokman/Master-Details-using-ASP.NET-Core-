namespace EvidencePractise_final1.Models
{
    public class EmployeeVM
    {
        public EmployeeVM()
        {
            this.SkillList = new List<int>();
        }
        public int EmployeeId { get; set; }

        public string EmploeeName { get; set; } = null!;

        public DateTime Joindate { get; set; }

        public string? Image { get; set; }
        public IFormFile ? ImageFile { get; set; }

        public bool ActiveStatus { get; set; }

        public List<int> SkillList { get; set; }
    }
}
