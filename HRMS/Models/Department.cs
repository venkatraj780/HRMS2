using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public int? EmpIdHead { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
