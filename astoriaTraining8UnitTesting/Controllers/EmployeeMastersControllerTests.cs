using astoriaTrainingAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using astoriaTrainingAPI.Models;

//namespace astoriaTrainingAPI.Controllers.Tests
//{
//    [TestClass()]
//    public class EmployeeMastersControllerTests
//    {
//        private astoriaTraining80Context _context;

        //[TestMethod()]
        //public void PostUserInfo_ValidDataMatch_Test()
        //{
        //    //Arrange
        //    var uInfo = new UserInfo();
        //    uInfo.ID = 402;
        //    uInfo.FirstName = "Geeta";
        //    uInfo.LastName = "Ahirwar";
        //    uInfo.UserName = "geetaAhirwar";
        //    uInfo.Email = "geetaahirwar@123gmail.com";
        //    uInfo.Passwords = "Ahirwar@098";
        //    uInfo.CreationDate = DateTime.Now.Date;
            

        //    //Action
        //    var userInfoController = new UserInfoController(_context);
        //    var result = userInfoController.PostUserInfo(uInfo);

        //    //Assert
        //    Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
        //}

        //[TestMethod()]
        //public void PostUserInfo_InvalidData_Match_Test()
        //{
        //    //Arrange
        //    var uInfo = new UserInfo();
        //    uInfo.ID = 402;    //This ID is already exist in Employee allowance details table
        //    uInfo.FirstName = "Geeta";
        //    uInfo.LastName = "Ahirwar";
        //    uInfo.UserName = "geetaAhirwar";
        //    uInfo.Email = "geetaahirwar@123gmail.com";
        //    uInfo.Passwords = "Ahirwar@098";
        //    uInfo.CreationDate = DateTime.Now.Date;


        //    //Action
        //    var userInfoController = new UserInfoController(_context);
        //    var result = userInfoController.PostUserInfo(uInfo);

        //    //Assert
        //    Assert.IsInstanceOfType(result.Result.Result, typeof(ConflictObjectResult));
        //}
//    }
//}

namespace astoriaTrainingAPI.Models.Tests
{
    [TestClass()]
    public class EmployeeMastersControllerTests
    {
        private readonly astoriaTraining80Context _context;

        public EmployeeMastersControllerTests()
        {
            var optionBuilder = new DbContextOptionsBuilder<astoriaTraining80Context>();
            //optionBuilder.UseSqlServer("server=meghaahirwar.database.windows.net; Database=AstoriaTraining8.0_2022; user Id=MeghaAhirwar98; password=Megha@123456;");
            optionBuilder.UseSqlServer("server=ASTORIA-LT57; Database=astoriaTraining8.0Bak; user Id=sa; password=pass123;");
            _context = new astoriaTraining80Context(optionBuilder.Options);
        }

        #region Unit Test Methods of GetAllEmployess API
        [TestMethod()]
            public void GetEmployees_MatchCount_ReturnOk_Test()
            {
            //Arrange
            int expectedAllEmployeesCount = 20;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployees();
            var ResultList = apiResult.Result.Value as List<Employee>;
            int ResultCount = ResultList.Count;

            //Assert
            Assert.AreEqual(expectedAllEmployeesCount, ResultCount);            
            }

        [TestMethod()]
        public void GetEmployees_ReturnOk_Test()
        {
            //Arrange
            var ObjectResult = typeof(OkObjectResult);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster();

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, ObjectResult);
        }


        [TestMethod()]
            public void GetEmployees_ReturnNoContent_Test()
            {
            //Arrange
            var noContentResult = typeof(NoContentResult);


            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployees();
            var ResultType = apiResult.Result.Result;

            //Assert
            Assert.IsInstanceOfType(ResultType, noContentResult);
            throw new NotImplementedException();
            }
        #endregion


        #region Test Methods of GetEmployeeMasterById API
        [TestMethod()]
        public void GetEmployeeMaster_ValidInput_MatchResult_Test()
        {
            //Arrange
            long empKeyInput = 13;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            var ResultObject = ((OkObjectResult)result.Result.Result).Value as EmployeeMaster;

            //Assert
            Assert.AreEqual(empKeyInput, ResultObject.EmployeeKey);
        }

        [TestMethod()]
        public void GetEmployeeMaster_ValidInput_MatchResultType_Test()
        {
            //Arrange
            long empKeyInput = 13;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void GetEmployeeMaster_InValidInput_MatchResult_Test()
        {
            //Arrange
            long empKeyInput = 1;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
           
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetEmployeeMaster_EmplyeeKey_MatchResult_Test()
        {
            //Arrange
            long empKeyInput = 12;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            var ResultObject = ((OkObjectResult)apiResult.Result.Result).Value as EmployeeMaster;

            //Assert
            Assert.AreEqual(empKeyInput, ResultObject.EmployeeKey);
        }
        #endregion


        #region Test Methods of PostEmployeeMaster API
        [TestMethod()]
        public void PostEmployeeMaster_ValidInput_MatchResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-205";
            objEmployee.EmpFirstName = "Geeta";
            objEmployee.EmpLastName = "Ahirwar";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 5;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 51.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));

        }

        [TestMethod()]
        public void PostEmployeeMaster_EmptyEmployeeID_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "";
            objEmployee.EmpFirstName = "Rama";
            objEmployee.EmpLastName = "Devi";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestResult));

        }

        [TestMethod()]
        public void PostEmployeeMaster_InvalidEmployeeID_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-10444444444444444444444qwer@@";
            objEmployee.EmpFirstName = "Jaishree";
            objEmployee.EmpLastName = "Malode";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_DuplicateEmployeeID_MatchResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-114";
            objEmployee.EmpFirstName = "Laxmi";
            objEmployee.EmpLastName = "Ahirwar";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_InvalidDate_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-201";
            objEmployee.EmpFirstName = "Ugandher";
            objEmployee.EmpLastName = "Sir";
            objEmployee.EmpGender = "M";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 32.5M;
            objEmployee.EmpResignationDate = new DateTime(2022,11,11);
            objEmployee.EmpJoiningDate = new DateTime(2022,11,12);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestResult));

        }

        #endregion


        #region Test Methods of PutEmployeeMaster API
        [TestMethod()]
        public void PutEmployeeMaster_ValidInput_MatchResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 19;
            objEmployee.EmployeeId = "ATIL-609";
            objEmployee.EmpFirstName = "Poojaaa";
            objEmployee.EmpLastName = "verma";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now;
            objEmployee.CreationDate = DateTime.Now;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        }

        [TestMethod()]
        public void PutEmployeeMaster_EmptyEmployeeID_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 12;
            objEmployee.EmployeeId = "";
            objEmployee.EmpFirstName = "pooja";
            objEmployee.EmpLastName = "verma";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));

        }

        [TestMethod()]
        public void PutEmployeeMaster_InvalidEmployeeID_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 19;
            objEmployee.EmployeeId = "ATIL-104444444444444444444444444444444444444444";
            objEmployee.EmpFirstName = "Jaishree";
            objEmployee.EmpLastName = "Malode";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);


            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_InvalidEmployeeKey_MatchResultType_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 1234567890;
           
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_DuplicateEmployeeID_MatchResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 12;
            objEmployee.EmployeeId = "ATIL-110";
            objEmployee.EmpFirstName = "Priya";
            objEmployee.EmpLastName = "Dehariya";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 5;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 25.500M;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));

        }

        [TestMethod()]
       
        public void PutEmployeeMaster_InValidDate_MatchResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 19;
            objEmployee.EmployeeId = "ATIL-113";
            objEmployee.EmpFirstName = "Pooja";
            objEmployee.EmpLastName = "verma";
            objEmployee.EmpGender = "F";
            objEmployee.EmpCompanyId = 4;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 27.5M;
            objEmployee.EmpResignationDate = new DateTime(2022, 11, 11);
            objEmployee.EmpJoiningDate = new DateTime(2022, 11, 12);
            objEmployee.CreationDate = DateTime.Now;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));

        }
        #endregion


        #region Unit Test Methods of GetAllCompanines API
        [TestMethod()]
        public void GetCompanyMaster_MatchCount_ReturnOk_Test()
        {
            //Arrange
            int expectedCompanyCount = 3;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetCompanyMaster();
            var ResultList = apiResult.Result.Value as List<CompanyMaster>;
            int ResultCount = ResultList.Count;

            //Assert
            Assert.AreEqual(expectedCompanyCount, ResultCount);
        }

        [TestMethod()]
        public void GetCompanyMaster_ReturnNoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);


            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetCompanyMaster();
            var ResultType = apiResult.Result.Result;

            //Assert
            Assert.IsInstanceOfType(ResultType, noContentResult);
            
        }
        #endregion


        #region Unit Test Methods of GetAllDesignation API
        [TestMethod()]
        public void GetDesignationMaster_MatchCount_ReturnOk_Test()
        {
            //Arrange
            int expectedDesignationCount = 5;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetDesignationMaster();
            var ResultList = apiResult.Result.Value as List<DesignationMaster>;
            int ResultCount = ResultList.Count;

            //Assert
            Assert.AreEqual(expectedDesignationCount, ResultCount);
        }

        [TestMethod()]
        public void GetDesignationMaster_ReturnNoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);


            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetDesignationMaster();
            var ResultType = apiResult.Result.Result;

            //Assert
            Assert.IsInstanceOfType(ResultType, noContentResult);
            throw new NotImplementedException();
        }
        #endregion


        #region Unit Test Methods of DeleteEmployee API
        [TestMethod()]
        public void DeleteEmployeeMaster_ValidEmployeeKey_ReturnOk_Test()
        {
            //Arrange
            long empKeyInput = 30149;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);
         
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
            
        }

        [TestMethod()]
        public void DeleteEmployeeMaster_ReturnNoContent_Test()
        {
            //Arrange
            long empKeyInput = 2014589;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public void DeleteEmployeeMaster_EmployeeInUse_ReturnConflict_Test()
        {
            //Arrange
            long empKeyInput = 129;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);
            var employeeResult = result.Result.Result;

            //Assert
            Assert.IsInstanceOfType(employeeResult, typeof(ConflictObjectResult));
        }
        #endregion
    }
}