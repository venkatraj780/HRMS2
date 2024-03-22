using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public string PostCode { get; set; }
    }
}
