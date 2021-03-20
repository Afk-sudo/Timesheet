using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Tests
{
    public class EmployeeServiceTests
    {
        [TestCase("Иванов", 30000)]
        [TestCase("Cидоров", 5000)]
        [TestCase("Петров", 40000)]
        public void AddEmployee_ShouldReturnTrue(string login, decimal salary)
        {
            //arrange
            Employee employee = new StaffEmployee(login, salary);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x => x.Add(employee))
                .Verifiable();
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x => x.Add(employee), Times.Once);
            Assert.IsTrue(result);
        }

        [TestCase("", 30000)]
        [TestCase("Cидоров", 0)]
        [TestCase("Петров", -40000)]
        [TestCase(null, 40000)]
        public void AddEmployee_ShouldReturnFalse(string login, decimal salary)
        {
            //arrange
            Employee employee = new StaffEmployee(login, salary);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x => x.Add(employee))
                .Verifiable();
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x => x.Add(employee), Times.Never);
            Assert.IsFalse(result);
        }
    }
}