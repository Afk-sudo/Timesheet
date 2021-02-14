using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.DataAccess.Npgsql.Repositories;
using Timesheet.Domain.Abstractions;

namespace Timesheet.Tests
{
    public class AuthServiceTest
    {
        private readonly IEmployeeRepository _employeeRepository = new EmployeeRepository();
        
        [TestCase("Иванов", "QWERTY")]
        [TestCase("Петров", "qwerty123")]
        [TestCase("Сидоров", "password")]
        public void Login_ShouldReturnTrue(string login, string password)
        {   
            //arrange
            var service = new AuthService(_employeeRepository);
            
            //act
            var result = service.Login(login, password);

            //assert
            Assert.IsTrue(result);
        }
        
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("TestUser","qwerty")]
        [TestCase("User", "123456")]
        public void Login_ShouldReturnFalse(string login, string password)
        {   
            //arrange
            var service = new AuthService(_employeeRepository);

            //act
            var result = service.Login(login, password);

            //assert
            Assert.IsFalse(result);
        }
    }
}