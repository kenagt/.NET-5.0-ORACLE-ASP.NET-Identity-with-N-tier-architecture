using System;

namespace NTierOracleIdentityExample.Web.Models
{
    public class ExampleTableViewModel
    {
        public int Pk { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
    }
}
