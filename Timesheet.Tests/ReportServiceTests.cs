using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Timesheet.Api.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Tests
{
    public class ReportServiceTests
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimeLogRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedTotal = 8_750m; //  (8+8+4)/160 * 70_000
            var expectedTotalHours = 20; // 8+8+4
            var expectedEmployee = new StaffEmployee("Иванов", 70_000);

            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == expectedEmployee.Login )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = expectedEmployee,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        Employee = expectedEmployee,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        Employee = expectedEmployee,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = "Add new service"
                    }
                });
            employeeRepositoryMoq.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => expectedEmployee);
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedEmployee.Login);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedEmployee.Login, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
        
        [Test]
        public void GetEmployeeReport_ShouldReturnReportWithOvertimeBill()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimeLogRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedTotal = 131_250m; //  ((8/160 * 70_000) + (1 / 160 * 70000 * 2)) * 30
            var expectedTotalHours = 270; // 9 * 30
            var expectedEmployee = new StaffEmployee("Иванов", 70_000);


            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[30];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 9,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            Employee = expectedEmployee,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });
            
            employeeRepositoryMoq.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => expectedEmployee);
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedEmployee.Login);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedEmployee.Login, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }

        [Test]
        public void GetChiefEmployeeReport_ShouldReturnReport()
        {
            
        }
        [Test]
        public void GetEmployeeReport_TimeLogsForOneDay_ShouldReturnReport()
        {
            //arrange 
            var expectedTotal = 8m / 160m * 70_000;
            var expectedTotalHours = 8;
            var expectedEmployee = new StaffEmployee("Иванов", 70_000);

            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = expectedEmployee,
                        WorkingHours = 8,
                        Date = DateTime.Now.AddDays(-1),
                        Comment = "Comment"
                    }
                });
            
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => expectedEmployee);
            
            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedEmployee.Login);
            
            //assert
            timesheetRepositoryMock.VerifyAll();
            employeeRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedEmployee.Login, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
        [Test]
        public void GetEmployeeReport_TimeLogsWithOvertimeForOneDay_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimeLogRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedTotal = 8m / 160m * 70000 + 4m / 160m * 70_000 * 2;
            var expectedTotalHours = 12;
            var expectedEmployee = new StaffEmployee("Иванов", 70_000);

            timesheetRepositoryMoq.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == expectedEmployee.Login )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = expectedEmployee,
                        WorkingHours = 12,
                        Date = DateTime.Now.AddDays(-1),
                        Comment = "Comment"
                    }
                });
            employeeRepositoryMoq.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => expectedEmployee);
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedEmployee.Login);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedEmployee.Login, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }
}