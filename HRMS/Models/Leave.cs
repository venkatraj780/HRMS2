using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Leave
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public int Status { get; set; } = 0;
        public int EmpId { get; set; }
        [ForeignKey(nameof(EmpId))]
        public virtual Employee? EmployeeApplied { get; set; }
        public int? ApprovedBy { get; set; }
        [ForeignKey(nameof(ApprovedBy))]
        public virtual Employee? EmployeeApproved { get; set; }
        public int LeaveType { get; set; }
        public virtual SickLeaveReplacedEmployee? SickLeaveReplacedEmployee { get; set; }
    }
    public enum LeaveType
    {
        Sick,
        Casual
    }
}
