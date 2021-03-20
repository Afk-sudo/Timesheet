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
        [TestCase(8, "Иванов")]
        [TestCase(18, "Петров")]
        [TestCase(10, "Сидоров")]
        public void TrackTime_ShouldReturnTrue(int workingHourse, string login)
        {
            //arrange
            var employee = new StaffEmployee(login, 70_000);
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkingHours = workingHourse,
                Employee = employee,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);

            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.IsEmployeeExist(It.IsAny<string>()))
                .Returns(true);

            
            var service = new TimesheetService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
         
            //act
            var result = service.TrackTime(timeLog);

            //assert
            timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }
        
        [TestCase(25, null)]
        [TestCase(-5, "")]
        [TestCase(0, "TestUser")]
        [TestCase(26, "Петров")]
        public void TrackTime_ShouldReturnFalse(int workingHourse, string login)
        {
            //arrange
            var employee = new StaffEmployee(login, 70_000);
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkingHours = workingHourse,
                Employee = employee,
                Comment = ""
            };
            UserSession.Sessions.Add(employee);
            
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x =>
                    x.IsEmployeeExist(It.IsAny<string>()))
                .Returns(true);
            
            var service = new TimesheetService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            
            //act
            var result = service.TrackTime(timeLog);

            //assert
            timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never());
            Assert.IsFalse(result);
        }
    }
}