using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace astoriaTrainingAPI.Models
{
    public partial class DesignationMaster
    {
        public DesignationMaster()
        {
            EmployeeMaster = new HashSet<EmployeeMaster>();
        }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMaster { get; set; }
    }
}
