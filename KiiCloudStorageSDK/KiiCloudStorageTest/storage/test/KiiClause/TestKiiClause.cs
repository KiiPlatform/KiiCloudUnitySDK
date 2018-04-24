using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiClause
    {
        #region Equals
        [Test(), KiiUTInfo(
            action = "When we call Equals() with int,",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0000_Equal_int ()
        {
            KiiClause c = KiiClause.Equals("score", 10);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"score\",\"value\":10}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with long,",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0010_Equal_long ()
        {
            KiiClause c = KiiClause.Equals("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"score\",\"value\":1234567890123}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with double,",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0020_Equal_double ()
        {
            KiiClause c = KiiClause.Equals("score", 1.234);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"score\",\"value\":1.234}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with bool(true),",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0030_Equal_bool_true ()
        {
            KiiClause c = KiiClause.Equals("enable", true);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"enable\",\"value\":true}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with bool(false),",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0031_Equal_bool_false ()
        {
            KiiClause c = KiiClause.Equals("enable", false);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"enable\",\"value\":false}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with string,",
            expected = "We can get eq clause(see assertion)"
            )]
        public void Test_0040_Equal_string ()
        {
            KiiClause c = KiiClause.Equals("name", "kii");

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Equals() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0050_Equal_key_null ()
        {
            KiiClause.Equals(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Equals() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0050_Equal_key_empty ()
        {
            KiiClause.Equals("", "kii");
        }

        [Test(), KiiUTInfo(
            action = "When we call Equals() with null value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0051_Equal_value_null ()
        {
            KiiClause c = KiiClause.Equals("name", null);

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"name\"}", c.ToJson().ToString());
        }

        #endregion

        #region KiiClause.NotEquals
        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with int,",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0100_NotEqual_int ()
        {
            KiiClause c = KiiClause.NotEquals("score", 10);

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"score\",\"value\":10}}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with long,",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0110_NotEqual_long ()
        {
            KiiClause c = KiiClause.NotEquals("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"score\",\"value\":1234567890123}}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with double,",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0120_NotEqual_double ()
        {
            KiiClause c = KiiClause.NotEquals("score", 1.234);

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"score\",\"value\":1.234}}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with bool(true),",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0130_NotEqual_bool_true ()
        {
            KiiClause c = KiiClause.NotEquals("enable", true);

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"enable\",\"value\":true}}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with bool(false),",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0131_NotEqual_bool_false ()
        {
            KiiClause c = KiiClause.NotEquals("enable", false);

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"enable\",\"value\":false}}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call NotEquals() with string,",
            expected = "We can get not with eq clause(see assertion)"
            )]
        public void Test_0140_NotEqual_string ()
        {
            KiiClause c = KiiClause.NotEquals("name", "kii");

            Assert.AreEqual("{\"type\":\"not\",\"clause\":{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call NotEquals() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0150_NotEqual_key_null ()
        {
            KiiClause.NotEquals(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call NotEquals() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0150_NotEqual_key_empty ()
        {
            KiiClause.NotEquals("", "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call NotEquals() with null value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0151_NotEqual_value_null ()
        {
            KiiClause.NotEquals("name", null);
        }

        #endregion

        #region GreaterThan
        [Test(), KiiUTInfo(
            action = "When we call GreaterThan() with int,",
            expected = "We can get range with lowerLimit clause(see assertion)"
            )]
        public void Test_0200_GreaterThan_int ()
        {
            KiiClause c = KiiClause.GreaterThan("score", 10);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":10,\"lowerIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThan() with long,",
            expected = "We can get range with lowerLimit clause(see assertion)"
            )]
        public void Test_0210_GreaterThan_long ()
        {
            KiiClause c = KiiClause.GreaterThan("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":1234567890123,\"lowerIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThan() with double,",
            expected = "We can get range with lowerLimit clause(see assertion)"
            )]
        public void Test_0220_GreaterThan_double ()
        {
            KiiClause c = KiiClause.GreaterThan("score", 1.234);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":1.234,\"lowerIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThan() with bool(true),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0230_GreaterThan_bool_true ()
        {
            KiiClause.GreaterThan("enable", true);
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThan() with string,",
            expected = "We can get range with lowerLimit clause(see assertion)"
            )]
        public void Test_0240_GreaterThan_string ()
        {
            KiiClause c = KiiClause.GreaterThan("name", "kii");

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"lowerLimit\":\"kii\",\"lowerIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThan() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0250_GreaterThan_key_null ()
        {
            KiiClause.GreaterThan(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThan() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0250_GreaterThan_key_empty ()
        {
            KiiClause.GreaterThan("", "kii");
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThan() with null value,",
            expected = "We can get clause that doesn't have lowerLimit"
            )]
        public void Test_0251_GreaterThan_value_null ()
        {
            KiiClause c = KiiClause.GreaterThan("name", null);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"lowerIncluded\":false}", c.ToJson().ToString());
        }

        #endregion

        #region GreaterThanOrEqual
        [Test(), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with int,",
            expected = "We can get range with lowerLimit/included=true clause(see assertion)"
            )]
        public void Test_0300_GreaterThanOrEqual_int ()
        {
            KiiClause c = KiiClause.GreaterThanOrEqual("score", 10);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":10,\"lowerIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with long,",
            expected = "We can get range with lowerLimit/included=true clause(see assertion)"
            )]
        public void Test_0310_GreaterThanOrEqual_long ()
        {
            KiiClause c = KiiClause.GreaterThanOrEqual("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":1234567890123,\"lowerIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with double,",
            expected = "We can get range with lowerLimit/included=true clause(see assertion)"
            )]
        public void Test_0320_GreaterThanOrEqual_double ()
        {
            KiiClause c = KiiClause.GreaterThanOrEqual("score", 1.234);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":1.234,\"lowerIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with bool(true),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0330_GreaterThanOrEqual_bool_true ()
        {
            KiiClause.GreaterThanOrEqual("enable", true);
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with string,",
            expected = "We can get range with lowerLimit/included=true clause(see assertion)"
            )]
        public void Test_0340_GreaterThanOrEqual_string ()
        {
            KiiClause c = KiiClause.GreaterThanOrEqual("name", "kii");

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"lowerLimit\":\"kii\",\"lowerIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0350_GreaterThanOrEqual_key_null ()
        {
            KiiClause.GreaterThanOrEqual(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0350_GreaterThanOrEqual_key_empty ()
        {
            KiiClause.GreaterThanOrEqual("", "kii");
        }

        [Test(), KiiUTInfo(
            action = "When we call GreaterThanOrEqual() with null value,",
            expected = "We can get clause that doesn't have lowerLimit"
            )]
        public void Test_0351_GreaterThanOrEqual_value_null ()
        {
            KiiClause c = KiiClause.GreaterThanOrEqual("name", null);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"lowerIncluded\":true}", c.ToJson().ToString());
        }

        #endregion

        #region LessThan
        [Test(), KiiUTInfo(
            action = "When we call LessThan() with int,",
            expected = "We can get range with upperLimit/included=false clause(see assertion)"
            )]
        public void Test_0400_LessThan_int ()
        {
            KiiClause c = KiiClause.LessThan("score", 10);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":10,\"upperIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThan() with long,",
            expected = "We can get range with upperLimit/included=false clause(see assertion)"
            )]
        public void Test_0410_LessThan_long ()
        {
            KiiClause c = KiiClause.LessThan("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":1234567890123,\"upperIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThan() with double,",
            expected = "We can get range with upperLimit/included=false clause(see assertion)"
            )]
        public void Test_0420_LessThan_double ()
        {
            KiiClause c = KiiClause.LessThan("score", 1.234);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":1.234,\"upperIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThan() with bool(true),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0430_LessThan_bool_true ()
        {
            KiiClause.LessThan("enable", true);
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThan() with string,",
            expected = "We can get range with upperLimit/included=false clause(see assertion)"
            )]
        public void Test_0440_LessThan_string ()
        {
            KiiClause c = KiiClause.LessThan("name", "kii");

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"upperLimit\":\"kii\",\"upperIncluded\":false}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThan() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0450_LessThan_key_null ()
        {
            KiiClause.LessThan(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThan() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0450_LessThan_key_empty ()
        {
            KiiClause.LessThan("", "kii");
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThan() with null value,",
            expected = "We can get clause that doesn't have upperLimit"
            )]
        public void Test_0451_LessThan_value_null ()
        {
            KiiClause c = KiiClause.LessThan("name", null);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"upperIncluded\":false}", c.ToJson().ToString());
        }

        #endregion

        #region LessThanOrEqual
        [Test(), KiiUTInfo(
            action = "When we call LessThanOrEqual() with int,",
            expected = "We can get range with upperLimit/included=true clause(see assertion)"
            )]
        public void Test_0500_LessThanOrEqual_int ()
        {
            KiiClause c = KiiClause.LessThanOrEqual("score", 10);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":10,\"upperIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThanOrEqual() with long,",
            expected = "We can get range with upperLimit/included=true clause(see assertion)"
            )]
        public void Test_0510_LessThanOrEqual_long ()
        {
            KiiClause c = KiiClause.LessThanOrEqual("score", (long)1234567890123);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":1234567890123,\"upperIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThanOrEqual() with double,",
            expected = "We can get range with upperLimit/included=true clause(see assertion)"
            )]
        public void Test_0520_LessThanOrEqual_double ()
        {
            KiiClause c = KiiClause.LessThanOrEqual("score", 1.234);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"score\",\"upperLimit\":1.234,\"upperIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThanOrEqual() with bool(true),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0530_LessThanOrEqual_bool_true ()
        {
            KiiClause.LessThanOrEqual("enable", true);
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThanOrEqual() with string,",
            expected = "We can get range with upperLimit/included=true clause(see assertion)"
            )]
        public void Test_0540_LessThanOrEqual_string ()
        {
            KiiClause c = KiiClause.LessThanOrEqual("name", "kii");

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"upperLimit\":\"kii\",\"upperIncluded\":true}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThanOrEqual() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0550_LessThanOrEqual_key_null ()
        {
            KiiClause.LessThanOrEqual(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call LessThanOrEqual() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0550_LessThanOrEqual_key_empty ()
        {
            KiiClause.LessThanOrEqual("", "kii");
        }

        [Test(), KiiUTInfo(
            action = "When we call LessThanOrEqual() with null value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0551_LessThanOrEqual_value_null ()
        {
            KiiClause c = KiiClause.LessThanOrEqual("name", null);

            Assert.AreEqual("{\"type\":\"range\",\"field\":\"name\",\"upperIncluded\":true}", c.ToJson().ToString());
        }

        #endregion

        #region KiiClause.StartWith(string)

        [Test(), KiiUTInfo(
            action = "When we call StartsWith(),",
            expected = "We can get prefix clause(see assertion)"
            )]
        public void Test_0600_StartWith ()
        {
            KiiClause c = KiiClause.StartsWith("name", "ki");

            Assert.AreEqual("{\"type\":\"prefix\",\"field\":\"name\",\"prefix\":\"ki\"}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call StartsWith() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0601_StartWith_key_null ()
        {
            KiiClause.StartsWith(null, "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call StartsWith() with empty key,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0601_StartWith_key_empty ()
        {
            KiiClause.StartsWith("", "kii");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call StartsWith() with null value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0602_StartWith_value_null ()
        {
            KiiClause.StartsWith("name", null);
        }

        #endregion

        #region in clause
        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="length of values exceeds maximum of 200",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithDoubleValue() with value, the length of which is more than 200,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0701_InWithDoubleValue_length_exceed_200 ()
        {
            KiiClause.InWithDoubleValue("100", new double[201]);
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithDoubleValue() with value, the length of which is equal 200,",
            expected = "No ArgumentException thrown"
            )]
        public void Test_0702_InWithDoubleValue_length_equal_200 ()
        {
            try{
                KiiClause.InWithDoubleValue("100", new double[200]);
            }catch(Exception exception){
                Assert.Fail("should not throw exception");
            }
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="key must not null or empty string",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithDoubleValue() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0703_InWithDoubleValue_null_key ()
        {
            KiiClause.InWithDoubleValue(null, new double[200]);
        }
        [Test(), 
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithDoubleValue() with empty key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0703_InWithDoubleValue_empty_key ()
        {
            KiiClause.InWithDoubleValue("", new double[200]);
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException),
                          ExpectedMessage="value is null",
                          MatchType=MessageMatch.Exact), 
         KiiUTInfo(
            action = "When we call InWithDoubleValue() with empty value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0704_IInWithDoubleValue_null_value ()
        {
            KiiClause.InWithDoubleValue("100", null);
        }

        [Test(),
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="length of values exceeds maximum of 200",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithIntValue() with value, the length of which is more than 200,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0711_InWithIntValue_length_exceed_200 ()
        {
            KiiClause.InWithIntValue("100", new int[201]);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call InWithIntValue() with value, the length of which is equal 200,",
            expected = "No ArgumentException thrown"
            )]
        public void Test_0712_InWithIntValue_length_equal_200 ()
        {
            try{
                KiiClause.InWithIntValue("100", new int[200]);
            }catch(Exception exception){
                Assert.Fail("should not throw exception");
            }
        }

        [Test(),
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="key must not null or empty string",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithIntValue() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0713_InWithIntValue_null_key ()
        {
            KiiClause.InWithIntValue(null, new int[200]);
        }

        [Test(),
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithIntValue() with empty key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0713_InWithIntValue_empty_key ()
        {
            KiiClause.InWithIntValue("", new int[200]);
        }

        [Test(),
         ExpectedException(typeof(ArgumentException),
                          ExpectedMessage="value is null",
                          MatchType=MessageMatch.Exact), 
         KiiUTInfo(
            action = "When we call InWithIntValue() with empty value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0714_InWithIntValue_null_value ()
        {
            KiiClause.InWithIntValue("100", null);
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="length of values exceeds maximum of 200",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithLongValue() with value, the length of which is more than 200,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0721_InWithLongValue_length_exceed_200 ()
        {    
            KiiClause.InWithLongValue("100", new long[201]);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call InWithLongValue() with value, the length of which is equal 200,",
            expected = "No ArgumentException thrown"
            )]
        public void Test_0722_InWithLongValue_length_equal_200 ()
        {
            try{
                KiiClause.InWithLongValue("100", new long[200]);
            }catch(Exception exception){
                Assert.Fail("should not throw exception");
            }
        }

        [Test(),
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="key must not null or empty string",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithLongValue() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0723_InWithLongValue_null_key ()
        {
            KiiClause.InWithLongValue(null, new long[200]);
        }
        [Test(),
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithLongValue() with empty key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0723_InWithLongValue_empty_key ()
        {
            KiiClause.InWithLongValue("", new long[200]);
        }

        [Test(),          
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="value is null",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithLongValue() with empty value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0724_InWithLongValue_null_value ()
        {
            KiiClause.InWithLongValue("100", null);
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="length of values exceeds maximum of 200",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithStringValue() with value, the length of which is more than 200,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0731_InWithStringValue_length_exceed_200 ()
        {
            KiiClause.InWithStringValue("100", new string[201]);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call InWithStringValue() with value, the length of which is equal 200,",
            expected = "No ArgumentException thrown"
            )]
        public void Test_0732_InWithStringValue_length_equal_200 ()
        {
            try{
                string[] valueArray = new string[200];
                for(int i=0; i<valueArray.Length; i++)
                {
                    valueArray[i]= i.ToString();
                }

                KiiClause.InWithStringValue("100", valueArray);
            }catch(Exception exception){
                Assert.Fail("should not throw exception");
            }
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="key must not null or empty string",
                          MatchType=MessageMatch.Exact),
         KiiUTInfo(
            action = "When we call InWithStringValue() with null key,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0733_InWithStringValue_null_key ()
        {
            string[] valueArray = new string[200];
            for(int i=0; i<valueArray.Length; i++)
            {
                valueArray[i]= i.ToString();
            }
            KiiClause.InWithStringValue(null, valueArray);
        }
        [Test(), 
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithStringValue() with empty key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0733_InWithStringValue_empty_key ()
        {
            string[] valueArray = new string[200];
            for(int i=0; i<valueArray.Length; i++)
            {
                valueArray[i]= i.ToString();
            }
            KiiClause.InWithStringValue("", valueArray);
        }

        [Test(), 
         ExpectedException(typeof(ArgumentException),
                                   ExpectedMessage="value is null",
                                   MatchType=MessageMatch.Exact), 
         KiiUTInfo(
            action = "When we call InWithStringValue() with empty value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0734_InWithStringValue_null_value ()
        {
            KiiClause.InWithStringValue("100", null);
        } 

        [Test(), 
         ExpectedException(typeof(ArgumentException), 
                          ExpectedMessage="value is null", 
                          MatchType=MessageMatch.Exact), 
         KiiUTInfo(
            action = "When we call InWithStringValue() with empty value,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0735_InWithStringValue_empty_value ()
        {
            string[] valueArray = new string[200];
            KiiClause.InWithStringValue("100", valueArray);
        }
        #endregion

        #region HasField
        [Test(), KiiUTInfo(
            action = "When we call HasField() with integer field type,",
            expected = "We can get hasField clause for INTEGER"
            )]
        public void Test_0800_HasField_string ()
        {
            KiiClause c = KiiClause.HasField("name",FieldType.STRING);

            Assert.AreEqual("{\"type\":\"hasField\",\"field\":\"name\",\"fieldType\":\"STRING\"}", c.ToJson().ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call HasField() with integer field type,",
            expected = "We can get hasField clause for INTEGER"
            )]
        public void Test_0801_HasField_int ()
        {
            KiiClause c = KiiClause.HasField("score",FieldType.INTEGER);

            Assert.AreEqual("{\"type\":\"hasField\",\"field\":\"score\",\"fieldType\":\"INTEGER\"}", c.ToJson().ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call HasField() with integer field type,",
            expected = "We can get hasField clause for INTEGER"
            )]
        public void Test_0802_HasField_decimal ()
        {
            KiiClause c = KiiClause.HasField("fuel_indicator",FieldType.DECIMAL);

            Assert.AreEqual("{\"type\":\"hasField\",\"field\":\"fuel_indicator\",\"fieldType\":\"DECIMAL\"}", c.ToJson().ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call HasField() with boolean field type,",
            expected = "We can get hasField clause for BOOLEAN"
            )]
        public void Test_0802_HasField_boolean ()
        {
            KiiClause c = KiiClause.HasField("is_active",FieldType.BOOLEAN);

            Assert.AreEqual("{\"type\":\"hasField\",\"field\":\"is_active\",\"fieldType\":\"BOOLEAN\"}", c.ToJson().ToString());
        }
        [Test(), 
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithStringValue() with null key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0803_HasField_null_key ()
        {
            KiiClause.HasField(null,FieldType.BOOLEAN);
        }
        [Test(), 
            ExpectedException(typeof(ArgumentException), 
                ExpectedMessage="key must not null or empty string",
                MatchType=MessageMatch.Exact),
            KiiUTInfo(
                action = "When we call InWithStringValue() with empty key,",
                expected = "ArgumentException must be thrown"
            )]
        public void Test_0803_HasField_empty_key ()
        {
            KiiClause.HasField("",FieldType.BOOLEAN);
        }
        #endregion
    }
}

