using System.ComponentModel.DataAnnotations;

namespace HRMS.DTO
{
    public class EmpEditDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        
        public string ApartmentNumber { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public bool IsActive { get; set; }
    }
}
