using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace astoriaTrainingAPI.Models
{
    public partial class CompanyMaster
    {
        public CompanyMaster()
        {
            EmployeeMaster = new HashSet<EmployeeMaster>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMaster { get; set; }
    }
}
