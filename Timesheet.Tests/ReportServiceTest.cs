using System;
using System.Collections.Generic;
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
        public void GetEmployeeReport_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimesheetRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedLogin = "Иванов";
            var expectedTotal = 8_750m; //  (8+8+4)/160 * 70_000
            var expectedTotalHours = 20; // 8+8+4

            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == expectedLogin )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = new Employee{ Login = expectedLogin},
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        Employee = new Employee{ Login = expectedLogin},
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        Employee = new Employee{ Login = expectedLogin},
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = "Add new service"
                    }
                });
            employeeRepositoryMoq.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedLogin)))
                .Returns(() => new Employee{Login = expectedLogin, Salary = 70_000});
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedLogin);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedLogin, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
        
        [Test]
        public void GetEmployeeReport_ShouldReturnReportWithOvertimeBill()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimesheetRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedLogin = "Иванов";
            var expectedTotal = 131_250m; //  ((8/160 * 70_000) + (1 / 160 * 70000 * 2)) * 30
            var expectedTotalHours = 270; // 9 * 30

            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == expectedLogin )))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[30];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 9,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            Employee = new Employee {Login = expectedLogin},
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });
            
            employeeRepositoryMoq.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedLogin)))
                .Returns(() => new Employee{Login = expectedLogin, Salary = 70_000});
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedLogin);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedLogin, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }   
        [Test]
        public void GetEmployeeReport_TimeLogsForOneDay_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimesheetRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedLogin = "Иванов";
            var expectedTotal = 8m / 160m * 70000;
            var expectedTotalHours = 8;

            timesheetRepositoryMoq.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == expectedLogin )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = new Employee{Login = expectedLogin},
                        WorkingHours = 8,
                        Date = DateTime.Now.AddDays(-1),
                        Comment = "Comment"
                    }
                });
            employeeRepositoryMoq.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == expectedLogin)))
                .Returns(() => new Employee{Login = expectedLogin, Salary = 70_000});
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedLogin);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedLogin, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
        [Test]
        public void GetEmployeeReport_TimeLogsWithOvertimeForOneDay_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMoq = new Mock<ITimesheetRepository>();
            var employeeRepositoryMoq = new Mock<IEmployeeRepository>();
            var expectedLogin = "Иванов";
            var expectedTotal = 8m / 160m * 70000 + 4m / 160m * 70000 * 2;
            var expectedTotalHours = 12;

            timesheetRepositoryMoq.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == expectedLogin )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        Employee = new Employee{Login = expectedLogin},
                        WorkingHours = 12,
                        Date = DateTime.Now.AddDays(-1),
                        Comment = "Comment"
                    }
                });
            employeeRepositoryMoq.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == expectedLogin)))
                .Returns(() => new Employee{Login = expectedLogin, Salary = 70_000});
            
            
            var service = new ReportService(timesheetRepositoryMoq.Object, employeeRepositoryMoq.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedLogin);
            
            //assert
            timesheetRepositoryMoq.VerifyAll();
            employeeRepositoryMoq.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedLogin, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }
}