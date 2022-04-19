using System.ComponentModel.DataAnnotations;

namespace NTierOracleIdentityExample.Web.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }
        public string NormalizedName {
            get { return Name; }
            set { Name.ToUpper(); }
        }
        public string ConcurrencyStamp { get; set; }
    }
}
