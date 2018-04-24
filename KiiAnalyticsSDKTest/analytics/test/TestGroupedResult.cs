using System;
using JsonOrg;

using NUnit.Framework;

namespace KiiCorp.Cloud.Analytics
{
    [TestFixture()]
    public class TestGroupedResult
    {
        [Test()]
        public void Test_0000_Parse ()
        {
            JsonArray array = new JsonArray("[" +
                "{" +
                "\"name\":\"Male\"," +
                "\"pointInterval\":100," +
                "\"pointStart\":50," +
                "\"data\":[2,3,4]" +
                "}" +
                "]");
            GroupedResult result = GroupedResult.Parse(array);
            
            Assert.AreEqual(1, result.SnapShots.Count);
            
            GroupedSnapShot snapShot = result.SnapShots[0];
            Assert.AreEqual("Male", snapShot.Name);
            Assert.AreEqual(100, snapShot.PointInterval);
            Assert.AreEqual(50, snapShot.PointStart);
            JsonArray data = snapShot.Data;
            Assert.AreEqual(3, data.Length());
        }

        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0001_Parse_noJsonObject ()
        {
            JsonArray array = new JsonArray("[" +
                "1, 2" +
                "]");
            GroupedResult.Parse(array);
        }
        
        #region no_field

        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0010_Parse_noName ()
        {
            JsonArray array = new JsonArray("[" +
                "{" +
                "\"pointInterval\":100," +
                "\"pointStart\":50," +
                "\"data\":[2,3,4]" +
                "}" +
                "]");
            GroupedResult.Parse(array);
        }
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0011_Parse_noPointInterval ()
        {
            JsonArray array = new JsonArray("[" +
                "{" +
                "\"name\":\"Male\"," +
                "\"pointStart\":50," +
                "\"data\":[2,3,4]" +
                "}" +
                "]");
            GroupedResult.Parse(array);
        }

        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0012_Parse_noPointStart ()
        {
            JsonArray array = new JsonArray("[" +
                "{" +
                "\"name\":\"Male\"," +
                "\"pointInterval\":100," +
                "\"data\":[2,3,4]" +
                "}" +
                "]");
            GroupedResult.Parse(array);
        }
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0013_Parse_noData ()
        {
            JsonArray array = new JsonArray("[" +
                "{" +
                "\"name\":\"Male\"," +
                "\"pointInterval\":100," +
                "\"pointStart\":50," +
                "}" +
                "]");
            GroupedResult.Parse(array);
        }
        
        #endregion
    }
}

