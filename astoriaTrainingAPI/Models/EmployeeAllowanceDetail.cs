using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace astoriaTrainingAPI.Models
{
    public partial class EmployeeAllowanceDetail
    {
        public long EmployeeKey { get; set; }
        public int AllowanceId { get; set; }
        public DateTime ClockDate { get; set; }
        public decimal AllowanceAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual AllowanceMaster Allowance { get; set; }
        public virtual EmployeeMaster EmployeeKeyNavigation { get; set; }
    }
}
