using astoriaTrainingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("Application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;
        public DashboardController(astoriaTraining80Context context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to count of resigned and active employee
        /// </summary>
        /// <returns></returns>
        //GET: EmployeeCount 
        [HttpGet("EmployeeCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<int> GetEmployeeCount()
        {
            try
            {
                int ResignedEmployeeCount = _context.EmployeeMaster.Where(emp => emp.EmpResignationDate < DateTime.Today || emp.EmpResignationDate != null).Count();
                int totalEmployeeCount = _context.EmployeeMaster.Count();
                int activeEmployeeCount = (totalEmployeeCount - ResignedEmployeeCount);

                return new int[] {
            ResignedEmployeeCount,
            //totalEmployeeCount,
            activeEmployeeCount};
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// This method is used to count of working hours for employees
        /// </summary>
        /// <returns></returns>
        //GET: workingHourPerDay
        [HttpGet("workingHourPerDay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Dashboard> GetworkingHourPerDay()
        {
            try
            {
                var workingHourPerDay = _context.EmployeeAttendance
                .GroupBy(c => c.ClockDate)
                .Select(c => new Dashboard()
                {
                    ClockDate = c.Key.Date,
                    WorkingHours = c.Sum(c => c.TimeOut.Hour - c.Timein.Hour)
                }).OrderByDescending(c => c.ClockDate).Take(5).ToList();

                return workingHourPerDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to get employee salary
        /// </summary>
        /// <returns></returns>
        //GET: EmployeeSalary
        [HttpGet("EmployeeSalary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Dashboard> GetEmployeeSalary()
        {
            try
            {
                var EmployeeSalary = (from EM in _context.EmployeeMaster
                                      join EA in _context.EmployeeAttendance
                                      on EM.EmployeeKey equals EA.EmployeeKey
                                      select new { EM, EA }).GroupBy(e => e.EA.ClockDate).Select(e => new Dashboard
                                      {
                                          ClockDate = e.Key.Date,
                                          Salary = e.Sum(e => (e.EA.TimeOut.Hour - e.EA.Timein.Hour) * e.EM.EmpHourlySalaryRate)
                                      }).OrderByDescending(e => e.ClockDate).Take(5);

                var employeeAllowance = (from EA in _context.EmployeeAttendance
                                         join EAD in _context.EmployeeAllowanceDetail on new { EA.ClockDate, EA.EmployeeKey } equals
                                         new { EAD.ClockDate, EAD.EmployeeKey }
                                         into GRP
                                         from i in GRP.DefaultIfEmpty()
                                         select new { EA, i }).GroupBy(e => e.EA.ClockDate).Select(e => new Dashboard
                                         {
                                             ClockDate = e.Key.Date,
                                             Salary = e.Sum(e => e.i.AllowanceAmount == null ? 0 : e.i.AllowanceAmount)
                                         }).OrderByDescending(e => e.ClockDate).Take(5);

                var salaryCount = (from S in EmployeeSalary
                                   from A in employeeAllowance
                                   where S.ClockDate == A.ClockDate
                                   select new Dashboard()
                                   {
                                       ClockDate = S.ClockDate,
                                       Salary = S.Salary + A.Salary
                                   }).ToList();

                return salaryCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
