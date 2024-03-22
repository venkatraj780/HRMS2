using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class CommentAttachement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public int CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
    }
}
