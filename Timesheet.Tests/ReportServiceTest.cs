using Moq;
using NUnit.Framework;
using Timesheet.Api.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class ReportServiceTest
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport(string login)
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimesheetRepository>();
            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == login )))
                .Returns(() => new TimeLog[0]);
            
            var service = new ReportService(timesheetRepositoryMoq.Object);

            var expectedTotal = 100m;
            
            //act
            var result = service.GetEmployeeReport(login);
            
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(login, result.Employee.Login);
            
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);
            
            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }
}