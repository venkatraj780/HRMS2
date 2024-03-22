using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Employee User { get; set; }
        public virtual IEnumerable<CommentAttachement> Attachements { get; set; }
        public int TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public TaskSingle Task { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
