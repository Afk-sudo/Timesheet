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
        public void AddStaffEmployee_ShouldReturnTrue(string login, decimal salary)
        {
            //arrange
            StaffEmployee employee = new StaffEmployee(login, salary);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                x.AddStaff(It.Is<StaffEmployee>(y =>
                    y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddStaffEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddStaff(employee), Times.Once);
            
            Assert.IsTrue(result);
        }

        [TestCase("", 30000)]
        [TestCase("Cидоров", 0)]
        [TestCase("Петров", -40000)]
        [TestCase(null, 40000)]
        public void AddStaffEmployee_ShouldReturnFalse(string login, decimal salary)
        {
            //arrange
            StaffEmployee employee = new StaffEmployee(login, salary);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                x.AddStaff(It.Is<StaffEmployee>(y =>
                    y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddStaffEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddStaff(employee), Times.Never);
            
            Assert.IsFalse(result);    
        }

        [TestCase("Cидоров", 5000, 2000)]
        [TestCase("Петров", 40000, 2000)]
        public void AddChiefEmployee_ShouldReturnTrue(string login, decimal salary, decimal bonus)
        {
            //arrange
            ChiefEmployee employee = new ChiefEmployee(login, salary, bonus);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x
                => x.AddChief(It.Is<ChiefEmployee>(y
                    => y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddChiefEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddChief(employee), Times.Once);
            Assert.IsTrue(result);
        }

        [TestCase("", 5000, 2000)]
        [TestCase("Петров", 0, 2000)]
        [TestCase("Петров", -10000, 2000)]
        [TestCase(null, 0, 2000)]
        [TestCase("Петров", 100000, 0)]
        public void AddChiefEmployee_ShouldReturnFalse(string login, decimal salary, decimal bonus)
        {
            //arrange
            ChiefEmployee employee = new ChiefEmployee(login, salary, bonus);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x
                => x.AddChief(It.Is<ChiefEmployee>(y
                    => y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            //act
            var result = service.AddChiefEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddChief(employee), Times.Never);
            Assert.IsFalse(result);
        }

        [TestCase("Иванов", 30000)]
        [TestCase("Cидоров", 5000)]
        [TestCase("Петров", 40000)]
        public void AddFreelancerEmployee_ShouldReturnTrue(string login, decimal salary)
        {
            //arrange 
            FreelancerEmployee employee = new FreelancerEmployee(login, salary);
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(x 
                    => x.AddFreelancer(It.Is<FreelancerEmployee>(y 
                        => y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            
            //act 
            var result = service.AddFreelancerEmployee(employee);
            
            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddFreelancer(employee), Times.Once);
            
            Assert.IsTrue(result);
        }
        
        [TestCase("", 30000)]
        [TestCase("Cидоров", 0)]
        [TestCase("Петров", -40000)]
        [TestCase(null, 40000)]
        public void AddFreelancerEmployee_ShouldReturnFalse(string login, decimal salary)
        {
            //arrange 
            FreelancerEmployee employee = new FreelancerEmployee(login, salary);
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(x 
                    => x.AddFreelancer(It.Is<FreelancerEmployee>(y 
                        => y == employee)));
            
            var service = new EmployeeService(employeeRepositoryMock.Object);
            
            //act 
            var result = service.AddFreelancerEmployee(employee);
            
            //assert
            employeeRepositoryMock.Verify(x 
                => x.AddFreelancer(employee), Times.Never);
            
            Assert.IsFalse(result);
        }
    }
}