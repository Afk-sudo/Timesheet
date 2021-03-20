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
                .Returns(new StaffEmployee(login, 50000));
            
            var service = new AuthService(employeeRepositoryMock.Object);
            
            //act
            var result = service.Login(login, password);

            //assert
            Assert.IsTrue(result);
        }


        [TestCase("Ivanov", "password")]
        public void Login_InvokeTwiceForOneLogin_ShouldReturnTrue(string login, string password)
        {
            //arrange
            var expectedEmployee = new StaffEmployee(login, 50000)
            {
                PasswordHash = password
            };
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.GetEmployeeByLoginPassword(It.Is<string>(y => y == login),
                        It.Is<string>(z => z == password)))
                .Returns(() => expectedEmployee);
            
            var service = new AuthService(employeeRepositoryMock.Object);
            
            //act
            var result = service.Login(login, password);
            result = service.Login(login, password);
            
            //assert 
            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(expectedEmployee));
            Assert.IsTrue(result);
        }

        [TestCase(null, "password")]
        [TestCase("Ivanov", null)]
        public void Login_NotValidArgument_ShouldReturnFalse(string login, string password)
        {
            //arrange
            var expectedEmployee = new StaffEmployee(login, 50000)
            {
                PasswordHash = password
            };
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                x.GetEmployeeByLoginPassword(It.Is<string>(y => y == login),
                    It.Is<string>(z => z == password)))
                .Returns(() => expectedEmployee);

            var service = new AuthService(employeeRepositoryMock.Object);

            //act 
            var result = service.Login(login, password);
            
            //assert
            Assert.IsFalse(result);
        }
        
        [TestCase("","qwerty")]
        public void Login_UserDoesNotExist_ShouldReturnFalse(string login, string password)
        {   
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.GetEmployeeByLoginPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(login, password);
            
            //assert
            Assert.IsFalse(result);
        }
    }
}