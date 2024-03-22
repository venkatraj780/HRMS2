using HRMS.Models.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ProjSkill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SkillId { get; set; }
        public virtual SkillType Skill { get; set; }
        public int ProjId { get; set; }
        [ForeignKey(nameof(ProjId))]
        public virtual Project Project { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
