using System.ComponentModel.DataAnnotations;

namespace NTierOracleIdentityExample.Web.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First name required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email required.")]
        public string Email { get; set; }

        public string NormalizedUserName
        {
            get { return UserName; }
            set { UserName.ToUpper(); }
        }

        public string NormalizedEmail
        {
            get { return Email; }
            set { Email.ToUpper(); }
        }

        public bool EmailConfirmed { get { return false;  } }
        public bool PhoneNumberConfirmed { get { return false; } }
        public bool TwoFactorEnabled { get { return false; } }
        public bool LockoutEnabled { get { return true; } }
        public int AccessFailedCount { get { return 0; } }
        public string ConcurrencyStamp { get; set; }
    }
}
