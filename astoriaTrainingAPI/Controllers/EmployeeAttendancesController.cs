using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI.Models;
using Microsoft.AspNetCore.Http;

namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeeAttendancesController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeAttendancesController(astoriaTraining80Context context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is for getting all attendance according to clockdate and company id
        /// </summary>
        /// <param name="FilterClockDate">clock date should be in right format</param>
        /// <param name="FilterCompanyID"> Company Id should be in integer</param>
        /// <returns>Attendance</returns>
        // GET: api/EmployeeAttendances
        [HttpGet("allattendance")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetEmployeeAttendance(string FilterClockDate, int FilterCompanyID)
        {
            try
            {
                //if (FilterClockDate == null && FilterCompanyID == null)
                //{
                //    return BadRequest();
                //}
                var empAtt = from emp in _context.EmployeeMaster.Where(emp => emp.EmpCompanyId == FilterCompanyID && (emp.EmpResignationDate == null || (emp.EmpResignationDate >= Convert.ToDateTime(FilterClockDate).Date)))
                             join Att in _context.EmployeeAttendance.Where(x => x.ClockDate.Date == Convert.ToDateTime(FilterClockDate).Date)
                             on emp.EmployeeKey equals Att.EmployeeKey
                             into grouping
                             from g in grouping.DefaultIfEmpty()

                             select new Attendance
                             {
                                 EmployeeKey = emp.EmployeeKey,
                                 EmployeeId = emp.EmployeeId,
                                 EmployeeName = (emp.EmpFirstName + " " + emp.EmpLastName),
                                 ClockDate = FilterClockDate,
                                 Timein = g.Timein == null ? string.Empty : g.Timein.ToString("HH:mm"),
                                 TimeOut = g.TimeOut == null ? string.Empty : g.TimeOut.ToString("HH:mm"),
                                 Remarks = g.Remarks == null ? string.Empty : g.Remarks,
                                 CreationDate = g.CreationDate.Date

                             };

                var empAttCount = await empAtt.ToListAsync();
               
                if (empAttCount.Count > 0)
                {
                    return empAttCount;
                }                   
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is for posting the data
        /// </summary>
        /// <param name="employeeAttendanceList">fill all the fields of employee attendance list</param>
        /// <param name="EmployeeKey">Pass EmployeeKey</param>
        /// <returns>ActionResult</returns>
        // POST: api/EmployeeAttendances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<EmployeeAttendance>> PostEmployeeAttendance(EmployeeAttendance employeeAttendance)
        public async Task<ActionResult<bool>> PostEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList, bool EmployeeKey)
        {
            try
            {
               
                foreach (var employeeAttendance in employeeAttendanceList)
                {
                    bool IsEmployeeIDExists = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == employeeAttendance.EmployeeKey && e.ClockDate.Date == employeeAttendance.ClockDate.Date);
                    if (employeeAttendanceList.Count == 0)
                    {
                        return BadRequest("List Count is zero");
                    }
                    else if (employeeAttendance.Timein > employeeAttendance.TimeOut)
                    {
                       return BadRequest("Timein should be less than TimeOut");
                    }                  
                    else if (IsEmployeeIDExists == true)
                    {
                        employeeAttendance.ModificationDate = DateTime.Now;
                        _context.Entry(employeeAttendance).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        employeeAttendance.CreationDate = DateTime.Now;
                        employeeAttendance.ModificationDate = DateTime.Now;
                        _context.EmployeeAttendance.Add(employeeAttendance);
                        await _context.SaveChangesAsync();

                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }                      
        }
    }
}
