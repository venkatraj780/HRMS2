using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Employees
{
    public class EmpSkill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SkillId { get; set; }
        public virtual SkillType Skill { get; set; }
        public int EmpId { get; set; }
        [ForeignKey(nameof(EmpId))]
        public virtual Employee Employee { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
