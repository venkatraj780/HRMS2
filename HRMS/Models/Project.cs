using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRMS.Models.Employees;

namespace HRMS.Models
{
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Employee> AssignedEmployees { get; set; } = new List<Employee>();
        public List<ProjSkill> SkillsRequired { get; set; } = new List<ProjSkill>();
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
