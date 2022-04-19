using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NTierOracleIdentityExample.Dll.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
    }
}
