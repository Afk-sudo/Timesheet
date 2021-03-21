using System;
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
        public void GetStaffEmployeeReport_ShouldReturnReport()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedTotal = 8_750m; //  (8+8+4)/160 * 70_000
            var expectedTotalHours = 20; // 8+8+4
            var expectedEmployee = new StaffEmployee("Иванов", 70_000);
            
            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == expectedEmployee.Login )))
                .Returns(() => new TimeLog[]
                {
                    new TimeLog
                    {
                        EmployeeLogin = expectedEmployee.Login,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        EmployeeLogin = expectedEmployee.Login,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = "Add new service"
                    },
                    new TimeLog
                    {
                        EmployeeLogin = expectedEmployee.Login,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = "Add new service"
                    }
                });
            employeeRepositoryMock.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == expectedEmployee.Login)))
                .Returns(() => expectedEmployee);
            
            
            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            
            //act
            var result = service.GetEmployeeReport(expectedEmployee.Login);
            
            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedEmployee.Login, result.Employee.Login);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
            Assert.AreEqual(expectedTotal, result.Bill);
        }
        
        [Test]
        public void GetStaffEmployeeReport_ShouldReturnReportWithOvertimeBill()
        {
            //arrange 
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var totalBill = 43_750; //  ((8/160 * 70_000) + (1 / 160 * 70000 * 2)) * 10
            var totalHours = 90; // 9 * 10
            var salary = 70_000m;
            var employee = new StaffEmployee("Иванов", salary);

            timesheetRepositoryMock.Setup(x => 
                x.GetTimeLogs(It.Is<string>(l => l == employee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[10];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 9,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            EmployeeLogin = employee.Login,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });
            
            employeeRepositoryMock.Setup(x 
                => x.GetEmployee(It.Is<string>(l => l == employee.Login)))
                .Returns(() => employee);
            
            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            
            //act
            var result = service.GetEmployeeReport(employee.Login);
            
            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(employee.Login, result.Employee.Login);
            Assert.AreEqual(totalHours, result.TotalHours);
            Assert.AreEqual(totalBill, result.Bill);
        }

        [Test]
        public void GetStaffEmployeeReport_TimeLogsForOneDay_ShouldReturnReport()
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
                        EmployeeLogin = expectedEmployee.Login,
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
                        EmployeeLogin = expectedEmployee.Login,
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
            //arrange 
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var totalBill = 50_000m; //  ((8/160 * 100_000m)
            var totalHours = 80; // 8 * 10
            var salary = 100_000m;
            var bonus = 20_000m;
            var employee = new ChiefEmployee("Иванов", salary, bonus);

            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == employee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[10];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 8,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            EmployeeLogin = employee.Login,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });

            employeeRepositoryMock.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == employee.Login)))
                .Returns(() => employee);
            var service = new ReportService(timesheetRepositoryMock.Object, 
                employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(employee.Login);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(employee.Login, result.Employee.Login);
            Assert.AreEqual(totalHours, result.TotalHours);
            Assert.AreEqual(totalBill, result.Bill);
        }

        [Test]
        public void GetChiefEmployeeReport_ShouldReturnReportWithOvertime()
        {
            //arrange 
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var totalBill = 120_000m; //  ((160/160 * 100_000m) + ((8 / 160 * 20_000) * 20) 
            var totalHours = 180; // 9 * 20
            var salary = 100_000m;
            var bonus = 20_000m;
            var employee = new ChiefEmployee("Иванов", salary, bonus);

            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == employee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[20];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 9,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            EmployeeLogin = employee.Login,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });

            employeeRepositoryMock.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == employee.Login)))
                .Returns(() => employee);
            var service = new ReportService(timesheetRepositoryMock.Object, 
                employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(employee.Login);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(employee.Login, result.Employee.Login);
            Assert.AreEqual(totalHours, result.TotalHours);
            Assert.AreEqual(totalBill, result.Bill);
        }

        [Test]
        public void GetFreelancerEmployeeReport_ShouldReturnReport()
        {
            //arrange 
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var totalBill = 22_500m; //  ((120/160 * 30_000m) 
            var totalHours = 120; // 6 * 20
            var salary = 30_000m;
            var employee = new FreelancerEmployee("Иванов", salary);

            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == employee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[20];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 6,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            EmployeeLogin = employee.Login,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });

            employeeRepositoryMock.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == employee.Login)))
                .Returns(() => employee);
            var service = new ReportService(timesheetRepositoryMock.Object, 
                employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(employee.Login);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(employee.Login, result.Employee.Login);
            Assert.AreEqual(totalHours, result.TotalHours);
            Assert.AreEqual(totalBill, result.Bill);
        }
        
        [Test]
        public void GetFreelancerEmployeeReport_ShouldReturnReportWithOvertime()
        {
            //arrange 
            var timesheetRepositoryMock = new Mock<ITimeLogRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var totalBill = 37_500m; //  ((200/160 * 30_000m) 
            var totalHours = 200; // 10 * 20
            var salary = 30_000m;
            var employee = new FreelancerEmployee("Иванов", salary);

            timesheetRepositoryMock.Setup(x => 
                    x.GetTimeLogs(It.Is<string>(l => l == employee.Login)))
                .Returns(() =>
                {
                    var timelogs = new TimeLog[20];

                    for (int i = 0; i < timelogs.Length; i++)
                    {
                        timelogs[i] = new TimeLog
                        {
                            WorkingHours = 10,
                            Date = new DateTime(2021, 1, 1).AddDays(i),
                            EmployeeLogin = employee.Login,
                            Comment = "Abstract comment"
                        };
                    }
                    
                    return timelogs;
                });

            employeeRepositoryMock.Setup(x 
                    => x.GetEmployee(It.Is<string>(l => l == employee.Login)))
                .Returns(() => employee);
            var service = new ReportService(timesheetRepositoryMock.Object, 
                employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(employee.Login);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(employee.Login, result.Employee.Login);
            Assert.AreEqual(totalHours, result.TotalHours);
            Assert.AreEqual(totalBill, result.Bill);
        }
        
    }
}