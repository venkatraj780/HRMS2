using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRMS.Models.Employees;

namespace HRMS.Models
{
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmployeeUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }
        public bool IsDepartmentManager { get; set; }
        public bool IsProjectManager { get; set; }
        public virtual List<Project> AssignedProjects { get; set; } = new();
        public virtual List<Leave> LeaveDetails { get; set; } = new();
        public virtual List<EmpSkill>? Skills { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; } = true;
        public Employee()
        {
            Address = new();
        }
    }
}
