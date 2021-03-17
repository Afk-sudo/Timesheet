using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        
        [TestCase("Иванов", "QWERTY")]
        [TestCase("Петров", "qwerty123")]
        [TestCase("Сидоров", "password")]
        public void Login_ShouldReturnTrue(string login, string password)
        {   
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.GetEmployeeByLoginPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Employee());
            
            var service = new AuthService(employeeRepositoryMock.Object);
            
            //act
            var result = service.Login(login, password);

            //assert
            Assert.IsTrue(result);
        }
        
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("","qwerty")]
        [TestCase(null, "123456")]
        public void Login_ShouldReturnFalse(string login, string password)
        {   
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.GetEmployeeByLoginPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Employee());

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(login, password);
            
            //assert
            Assert.IsFalse(result);
        }
    }
}