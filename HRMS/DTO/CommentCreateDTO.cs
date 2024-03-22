using HRMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.DTO
{
    public class CommentCreateDTO
    {
        [Required]
        public string Description { get; set; }
        public List<IFormFile> Attachements { get; set; }
        public int TaskId { get; set; }
    }
}
