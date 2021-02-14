using System;
using NUnit.Framework;
using Timesheet.Api.Models;
using Timesheet.Api.Services;

namespace Timesheet.Tests
{
    public class TimesheetServiceTests
    {
        [Test]
        public void TrackTime_ShouldReturnTrue()
        {
            //arrange
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkingHourse = 1,
                Login = ""
            };
                
            
            var service = new TimesheetService();
            //act
            var result = service.TrackTime(timeLog);

                //assert
            Assert.IsTrue(result);
        }
        [Test]
        public void TrackTime_ShouldReturnFalse()
        {
            //arrange
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkingHourse = 1,
                Login = ""
            };
                
            
            var service = new TimesheetService();
            //act
            var result = service.TrackTime(timeLog);

            //assert
            Assert.IsFalse(result);
        }
    }
}