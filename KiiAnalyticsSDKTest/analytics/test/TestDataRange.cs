using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Analytics
{
    [TestFixture()]
    public class TestDataRange
    {
        [Test()]
        public void Test_0000_ok ()
        {
            DateTime start = new DateTime(2013, 7, 9);
            DateTime end = new DateTime(2013, 7, 10);
            DateRange range = new DateRange(start, end);
            
            Assert.AreEqual(start, range.Start);
            Assert.AreEqual(end, range.End);
            Assert.AreEqual("startDate=2013-07-09&endDate=2013-07-10", range.ToQueryString());
        }

        [Test()]
        public void Test_0001_ok_equal ()
        {
            DateTime start = new DateTime(2013, 7, 9);
            DateTime end = new DateTime(2013, 7, 9);
            DateRange range = new DateRange(start, end);
            
            Assert.AreEqual(start, range.Start);
            Assert.AreEqual(end, range.End);
            Assert.AreEqual("startDate=2013-07-09&endDate=2013-07-09", range.ToQueryString());
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0002_ok_greater_than ()
        {
            DateTime start = new DateTime(2013, 7, 10);
            DateTime end = new DateTime(2013, 7, 9);
            new DateRange(start, end);
        }
    }
}

