using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace astoriaTrainingAPI.Models
{
    //[Authorize]   
    [Route("api/[controller]")]
    [ApiController]
    [Produces("Application/json")]
    public class EmployeeMastersController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeMastersController(astoriaTraining80Context context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is for getting all employees
        /// </summary>
        /// <returns>It will return us employee class</returns>
        [HttpGet("allemployees")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        
        {
            try
            {
                var emp = (from em in _context.EmployeeMaster
                           join cm in _context.CompanyMaster on em.EmpCompanyId equals cm.CompanyId
                           join dm in _context.DesignationMaster on em.EmpDesignationId equals dm.DesignationId
                           select new Employee()
                           {
                               EmployeeKey = em.EmployeeKey,
                               EmployeeId = em.EmployeeId,
                               EmployeeName = em.EmpFirstName + " " + em.EmpLastName,
                               CompanyName = cm.CompanyName,
                               DesignationName = dm.DesignationName,
                               JoininDate = em.EmpJoiningDate,
                               Gender = em.EmpGender,
                           }).ToListAsync();

                var EmpCount = await emp;

                if (EmpCount.Count > 0)
                {
                    return await emp;
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
            //return await emp;
        }

       


    // For checking the employee id exist or not
    [HttpGet("checkEmployeeIdExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> GetEmployeeIdExists(string EmployeeID, long employeeKey)
        {
            try
            {
                bool IsEmployeeIDExists = true;
                IsEmployeeIDExists = await _context.EmployeeMaster.AnyAsync(e => e.EmployeeKey != employeeKey && e.EmployeeId.ToLower().Trim() == EmployeeID.ToLower().Trim());
                return IsEmployeeIDExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //For checking the employee id is in use or not
        [HttpGet("IsEmployeeIdInUse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>>GetEmployeeKeyInUse(long EmployeeKey)
        {
            try
            {
                bool isEmployeeKeyInEmployeeAttendence = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == EmployeeKey);
                bool isEmployeeKeyInEmployeeAllowanceDetail = await _context.EmployeeAllowanceDetail.AnyAsync(e => e.EmployeeKey == EmployeeKey);
                if (isEmployeeKeyInEmployeeAttendence || isEmployeeKeyInEmployeeAllowanceDetail)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
           
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //GET: api/CompanyMaster
        [HttpGet("allcompanies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CompanyMaster>>> GetCompanyMaster()
        {
            try
            {
                var EmpCompany = await _context.CompanyMaster.ToListAsync();
                if (EmpCompany.Count() > 0)
                {
                    return EmpCompany;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GET: api/DesignationMaster
        [HttpGet("alldesignation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DesignationMaster>>> GetDesignationMaster()
        {
            try
            {
                var EmpDesignation = await _context.DesignationMaster.ToListAsync();
                if (EmpDesignation.Count() > 0)
                {
                    return EmpDesignation;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: api/EmployeeMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployeeMaster()
        {
         try
            {
                var EmployeeList = await _context.EmployeeMaster.ToListAsync();
                return Ok(EmployeeList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // GET: api/EmployeeMasters/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeMaster>> GetEmployeeMaster(long id)
        {
            try
            {
                //await Task.Delay(5000);   //To test loading bar in angular application
                var employeeMaster = await _context.EmployeeMaster.FindAsync(id);

                if (employeeMaster == null)
                {
                    return NotFound();
                }

                return Ok(employeeMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/EmployeeMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEmployeeMaster(long id, EmployeeMaster employeeMaster)
        {
            try
            {
                var empKey = _context.EmployeeMaster.Any(e => e.EmployeeKey == id);
                if (!empKey)
                {
                    return NotFound();
                }

                else
                {
                    if (string.IsNullOrEmpty(employeeMaster.EmployeeId) ||
                           string.IsNullOrEmpty(employeeMaster.EmpFirstName) ||
                           string.IsNullOrEmpty(employeeMaster.EmpLastName) ||
                           string.IsNullOrEmpty(employeeMaster.EmpGender) ||
                           employeeMaster.EmployeeId.Length > 20 ||
                           employeeMaster.EmpFirstName.Length > 100 ||
                           employeeMaster.EmpLastName.Length > 100 ||
                           (employeeMaster.EmpJoiningDate > employeeMaster.EmpResignationDate) ||
                           (id != employeeMaster.EmployeeKey))

                    {
                        return BadRequest();
                    }

                    bool ExistEmployeeID = _context.EmployeeMaster.Any(x => x.EmployeeKey != employeeMaster.EmployeeKey && x.EmployeeId == employeeMaster.EmployeeId);
                    if (ExistEmployeeID)
                    {
                        return StatusCode(StatusCodes.Status409Conflict);
                    }
                    employeeMaster.ModificationDate = DateTime.Now;
                    _context.Entry(employeeMaster).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
                return Ok(employeeMaster.EmployeeKey);
            }
            catch (DbUpdateConcurrencyException)
            {
                {
                    throw;
                }
            }
            //return EmployeeMaster();
            //return Ok("successfully updated");
        }

        private bool EmployeeMasterExists(long id)
        {
            try
            {
                return _context.EmployeeMaster.Any(e => e.EmployeeKey == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // POST: api/EmployeeMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeMaster>> PostEmployeeMaster(EmployeeMaster employeeMaster)
        {
            try
            {
                
                if (string.IsNullOrEmpty(employeeMaster.EmployeeId) ||
                    string.IsNullOrEmpty(employeeMaster.EmpFirstName) ||
                    string.IsNullOrEmpty(employeeMaster.EmpLastName) ||
                    string.IsNullOrEmpty(employeeMaster.EmpGender) ||
                    employeeMaster.EmployeeId.Length > 20 ||
                    employeeMaster.EmpFirstName.Length > 100 ||
                    employeeMaster.EmpLastName.Length > 100 ||
                    (employeeMaster.EmpJoiningDate > employeeMaster.EmpResignationDate))
                {
                    return BadRequest();
                }
                if (_context.EmployeeMaster.Any(emp => emp.EmployeeId == employeeMaster.EmployeeId))
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }

                employeeMaster.CreationDate = DateTime.Now;
                employeeMaster.ModificationDate = DateTime.Now;
                _context.EmployeeMaster.Add(employeeMaster);
                await _context.SaveChangesAsync();
                return Ok(employeeMaster);
            }
           
            catch (DbUpdateException)
            {
                if (EmployeeMasterExists(employeeMaster.EmployeeKey))
                {
                    return Conflict();
                }
                // _context.EmployeeMaster.Add(EmployeeMaster);
                //await _context.SaveChangesAsync();

                else
                {
                    throw;
                }
            }
            //return CreatedAtAction("GetEmployeeMaster", new { id = employeeMaster.EmployeeKey }, Ok(employeeMaster));
            //return Ok(employeeMaster);
        }

        // DELETE: api/EmployeeMasters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeMaster>> DeleteEmployeeMaster(long id)
        {
            try
            {
                var employeeMaster = await _context.EmployeeMaster.FindAsync(id);
                if (employeeMaster == null)
                {
                    return NotFound();
                }
                bool EmpKEyInUSeInAllowance = await _context.EmployeeAllowanceDetail.AnyAsync(e => e.EmployeeKey == id);
                bool EmpKeyInUseInAttendance = await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeKey == id);
                if (EmpKEyInUSeInAllowance || EmpKeyInUseInAttendance)
                {
                    return Conflict("Employee ID in use");
                }
                _context.EmployeeMaster.Remove(employeeMaster);
                await _context.SaveChangesAsync();
                return Ok(employeeMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //employeeMaster;
        }
    }
}
