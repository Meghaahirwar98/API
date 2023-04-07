using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI.Models;

namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("Application/json")]
    public class EmployeeAllowanceDetailsController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeAllowanceDetailsController(astoriaTraining80Context context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to get Employee Allowance details
        /// </summary>
        /// <returns></returns>
        // GET: api/EmployeeAllowanceDetails
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeAllowanceDetail>>> GetEmployeeAllowanceDetail()
        {
            return await _context.EmployeeAllowanceDetail.ToListAsync();
        }


        /// <summary>
        /// This methods is used to get employee allowance details by id
        /// </summary>
        /// <param name="id">ID should be in integer</param>
        /// <returns>Returns EmployeeAllowanceDetails Entity</returns>
        // GET: api/EmployeeAllowanceDetails/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeAllowanceDetail>> GetEmployeeAllowanceDetail(long id)
        {
            var employeeAllowanceDetail = await _context.EmployeeAllowanceDetail.FindAsync(id);

            if (employeeAllowanceDetail == null)
            {
                return NotFound();
            }
            return employeeAllowanceDetail;
        }


        /// <summary>
        /// This method is for Gmail and password
        /// </summary>
        /// <param name="userID">Required UserName</param>
        /// <param name="password">Required Password</param>
        /// <returns>Returns UserInfo </returns>
        //GET: api/CompanyMaster
        [HttpGet("GmailandPassword")]
        public async Task<ActionResult<bool>> GetCredential(string userID, string password)
        {           
            bool userInfo = await _context.UserInfo.AnyAsync(e => e.UserName == userID && e.Passwords == password);
            return userInfo;
        }


        /// <summary>
        /// This method is used for all allowances in the list
        /// </summary>
        /// <returns> returns AllowanceMAster</returns>
        //GET: api/CompanyMaster
        [HttpGet("allAllowance")]
        public async Task<ActionResult<IEnumerable<AllowanceMaster>>> GetAllAllowance()
        {
            var AllowanceCount = _context.AllowanceMaster.Count();
            if (AllowanceCount > 0)
            {
                return await _context.AllowanceMaster.ToListAsync();
            }
            else
            {
                return NoContent();
            }
        }

        //Get: api/AllTodayAttendance
        [HttpGet("allTodayAttendance")]
        public async Task<ActionResult<IEnumerable<AddAllowance>>> GetAllTodayAttendance()
        {
            var EmpAttendance = from emp in _context.EmployeeMaster
                                join Attendance in _context.EmployeeAttendance.Where(x => x.ClockDate.Date == (DateTime.Today).Date)
                                on emp.EmployeeKey equals Attendance.EmployeeKey
                                select new AddAllowance
                                {
                                    EmployeeKey = emp.EmployeeKey,
                                    EmployeeName = emp.EmpFirstName + " " + emp.EmpLastName
                                };
            return await EmpAttendance.ToListAsync();
        }

        //Get: api/AllTodayAttendance
        [HttpGet("AllowanceWithoutToday")]
        public async Task<ActionResult<IEnumerable<AddAllowance>>> GetAllowanceWithoutTodayDate()
        {
            var EmpAllowance = from emp in _context.EmployeeAllowanceDetail.Where(x => x.ClockDate.Date != (DateTime.Today).Date)
                               join em in _context.EmployeeMaster 
                               on emp.EmployeeKey equals em.EmployeeKey
                               select new AddAllowance
                                {
                                    EmployeeKey = emp.EmployeeKey,
                                    EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                                    EmployeeAllowance = emp.AllowanceId,
                                    ClockDate = emp.ClockDate
                                };
            return await EmpAllowance.ToListAsync();
        }


        // PUT: api/EmployeeAllowanceDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAllowanceDetail(long id, EmployeeAllowanceDetail employeeAllowanceDetail)
        {
            if (id != employeeAllowanceDetail.EmployeeKey)
            {
                return BadRequest();
            }

            _context.Entry(employeeAllowanceDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAllowanceDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeeAllowanceDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("postEmployeeAllowance")]
        public async Task<ActionResult<bool>> PostEmployeeAllowanceDetail(List<EmployeeAllowanceDetail> employeeAllowanceDetailList)
        {

                foreach (EmployeeAllowanceDetail empAllowance in employeeAllowanceDetailList)
                {
                    if (empAllowance.AllowanceAmount > 0)
                    {
                        EmployeeAllowanceDetail employeeAllowance = _context.EmployeeAllowanceDetail.Where(ead => ead.EmployeeKey == empAllowance.EmployeeKey
                        && ead.ClockDate == empAllowance.ClockDate
                        && ead.AllowanceId == empAllowance.AllowanceId).FirstOrDefault();
                        if (employeeAllowance != null)
                        {
                            employeeAllowance.AllowanceAmount = empAllowance.AllowanceAmount;
                            employeeAllowance.ModificationDate = DateTime.Now;
                            _context.Entry(employeeAllowance).State = EntityState.Modified;
                        }
                        else
                        {
                            empAllowance.CreationDate = empAllowance.ModificationDate = DateTime.Now;
                            _context.EmployeeAllowanceDetail.Add(empAllowance);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            

            return true;
        }
    
           

        // DELETE: api/EmployeeAllowanceDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeAllowanceDetail>> DeleteEmployeeAllowanceDetail(long id)
        {
            var employeeAllowanceDetail = await _context.EmployeeAllowanceDetail.FindAsync(id);
            if (employeeAllowanceDetail == null)
            {
                return NotFound();
            }

            _context.EmployeeAllowanceDetail.Remove(employeeAllowanceDetail);
            await _context.SaveChangesAsync();

            return employeeAllowanceDetail;
        }

        private bool EmployeeAllowanceDetailExists(long id)
        {
            return _context.EmployeeAllowanceDetail.Any(e => e.EmployeeKey == id);
        }
    }
}
