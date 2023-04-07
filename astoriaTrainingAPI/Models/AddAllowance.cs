using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI.Models
{
    public class AddAllowance
    {
        public long EmployeeKey { get; set; }
        //public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public object EmployeeAllowance { get; internal set; }
        public DateTime ClockDate { get; internal set; }
        //public DateTime ClockDate { get; set; },

    }
}
