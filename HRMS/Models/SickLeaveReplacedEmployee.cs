using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class SickLeaveReplacedEmployee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public int EmpIdOnLeave { get; set; }
        public int EmpReplcedId { get; set; }

        [ForeignKey(nameof(LeaveId))]
        public virtual Leave Leave { get; set; }

        [ForeignKey(nameof(EmpIdOnLeave))]
        public virtual Employee EmployeeOnleave { get; set; }
        [ForeignKey(nameof(EmpReplcedId))]
        public virtual Employee EmployeeReplaced { get; set; }

    }
}
