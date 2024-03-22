using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class TaskSingle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatedUserId { get; set; }

        [ForeignKey(nameof(CreatedUserId))]
        public Employee CreatedBy { get; set; }

        public int AssignedToUserId { get; set; }
        [ForeignKey(nameof(AssignedToUserId))]
        public Employee AssignedTo { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }

        public int Status { get; set; } = (int)CommentStatus.ToDo;
        public DateTime CompletedDate { get; set; }

    }
    public enum CommentStatus
    {
        ToDo = 1,
        InProgress = 2,
        Completed = 3
    }
}
