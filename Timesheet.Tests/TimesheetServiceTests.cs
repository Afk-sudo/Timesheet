using System;
using Moq;
using NUnit.Framework;
using Timesheet.Api.Services;
using Timesheet.Application.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Tests
{
    public class TimesheetServiceTests
    {
        public TimesheetServiceTests()
        {
            _timeLogRepositoryMock = new Mock<ITimeLogRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            _service =  new TimesheetService(_timeLogRepositoryMock.Object, _employeeRepositoryMock.Object);
        }
        
        private TimesheetService _service;
        
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<ITimeLogRepository> _timeLogRepositoryMock;
        [SetUp]
        public void SetUp()
        {
            UserSession.Sessions.Clear();
        }

        [TestCase(25, null)]
        [TestCase(-5, "")]
        [TestCase(0, "TestUser")]
        [TestCase(26, "Петров")]
        public void TrackTime_ShouldReturnFalse(int workingHourse, string login)
        {
            //arrange
            decimal salary = 70_000m;
            var employee = new StaffEmployee(login, salary);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = workingHourse,
                EmployeeLogin = employee.Login,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new StaffEmployee(login, salary));
            
            
            //act
            var result = _service.TrackTime(timeLog, "Ivanov");

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Never());
            Assert.IsFalse(result);
        }
        
        [Test]
        public void TrackTime_ChiefEmployee_ShouldReturnTrue()
        {
            //arrange
            int workingHourse = 8;
            string login = "Invanov";
            decimal salary = 40_000m;
            decimal bonus = 20_000m;
            
            var employee = new ChiefEmployee(login, salary, bonus);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = workingHourse,
                EmployeeLogin = employee.Login,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new ChiefEmployee(login, salary, bonus));
         
            //act
            var result = _service.TrackTime(timeLog, login);

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Once());
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_ChiefEmployeeBackdating_ShouldReturnTrue()
        {
            //arrange
            int workingHourse = 8;
            string login = "Invanov";
            decimal salary = 100_000m;
            decimal bonus = 20_000m;

            var employee = new ChiefEmployee(login, salary, bonus);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-10),
                WorkingHours = workingHourse,
                EmployeeLogin = employee.Login,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x => 
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new ChiefEmployee(login, salary, bonus));
            
            //act
            var result = _service.TrackTime(timeLog, login);

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployee_ShouldReturnTrue()
        {
            //arrange
            int workingHourse = 8;
            string login = "Invanov";
            decimal salary = 40_000m;

            var employee = new StaffEmployee(login, salary);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = workingHourse,
                EmployeeLogin = employee.Login,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new StaffEmployee(login, salary));
         
            //act
            var result = _service.TrackTime(timeLog, login);

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Once());
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeBackdating_ShouldReturnTrue()
        {
            //arrange
            int workingHourse = 8;
            string login = "Invanov";
            decimal salary = 40_000m;

            var employee = new StaffEmployee(login, salary);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-10),
                WorkingHours = workingHourse,
                EmployeeLogin = employee.Login,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x => 
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new StaffEmployee(login, salary));
            
            //act
            var result = _service.TrackTime(timeLog, login);

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeAdditionNotHimself_ShouldReturnFalse()
        {
            //arrange
            int workingHourse = 8;
            string login = "Invanov";
            decimal salary = 40_000m;
            
            var employee = new StaffEmployee(login, salary);
            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(1),
                WorkingHours = workingHourse,
                EmployeeLogin = "User",
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            _employeeRepositoryMock.Setup(x => 
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new StaffEmployee(login, salary));
            
            //act
            var result = _service.TrackTime(timeLog, login);

            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void TrackTime_Freelancer_ShouldReturnTrue()
        {
            //arrange
            string login = "Invanov";
            decimal salary = 40_000m;
            
            var employee = new FreelancerEmployee(login, salary);
            UserSession.Sessions.Add(employee);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = 4,
                EmployeeLogin = employee.Login,
                Comment = "Test comment"
            };
            
            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new FreelancerEmployee(login, salary));
            
            //act
            var result = _service.TrackTime(timeLog, login);
            
            //assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TrackTime_FreelancerAdditionNotHimself_ShouldReturnFalse()
        {
            //arrange
            string login = "Invanov";
            decimal salary = 40_000m;
            
            var employee = new FreelancerEmployee(login, salary);
            UserSession.Sessions.Add(employee);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(1),
                WorkingHours = 4,
                EmployeeLogin = "User",
                Comment = "Test comment"
            };

            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new FreelancerEmployee(login, salary));
            
            //act
            var result = _service.TrackTime(timeLog, login);
            
            //assert
            _timeLogRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            
            Assert.IsFalse(result);
        } 
        
        [Test]
        public void TrackTime_FreelancerBackdating_ShouldReturnFalse()
        {
            //arrange
            string login = "Invanov";
            decimal salary = 40_000m;
            
            var employee = new FreelancerEmployee(login, salary);
            UserSession.Sessions.Add(employee);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-3),
                WorkingHours = 4,
                EmployeeLogin = employee.Login,
                Comment = "Test comment"
            };
            
            _employeeRepositoryMock.Setup(x =>
                    x.GetEmployee(It.Is<string>(y => y == login)))
                .Returns(new FreelancerEmployee(login, salary));
            
            //act
            var result = _service.TrackTime(timeLog, login);
            
            //assert
            Assert.IsFalse(result);
        } 
    }
}