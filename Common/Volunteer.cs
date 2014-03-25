using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class Volunteer
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email address is required.")]
        public string EmailAddress { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }
    }
}