using System.ComponentModel.DataAnnotations;

namespace HRMS.DTO
{
    public class EmployeeRegisterDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public bool? IsDepartmentManager { get; set; }
        public bool? IsProjectManager { get; set; }

        public string ApartmentNumber { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
    }
}
