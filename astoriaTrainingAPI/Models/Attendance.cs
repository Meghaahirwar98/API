using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astoriaTrainingAPI.Models
{
    public class Attendance
    {
        public long EmployeeKey { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ClockDate { get; set; }
        public string Timein { get; set; }
        public string TimeOut { get; set; }
        public string Remarks { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
