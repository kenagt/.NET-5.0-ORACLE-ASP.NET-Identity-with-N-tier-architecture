using System;

namespace NTierOracleIdentityExample.Dll.Entities
{
    public class Log
    {
        public int pk { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? Date { get; set; }
        public string Value { get; set; }
    }
}
