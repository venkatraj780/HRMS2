using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Holiday
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateTime Date { get; set; }
    }
}
