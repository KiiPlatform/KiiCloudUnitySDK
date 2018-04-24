using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiClause_and_or
    {
        #region And
        [Test(), KiiUTInfo(
            action = "When we call And() with 2 clauses,",
            expected = "We can get and clause(see assertion)"
            )]
        public void Test_0000_And ()
        {
            KiiClause c = KiiClause.And(
                KiiClause.Equals("name", "kii"),
                KiiClause.GreaterThan("score", 100)
                );

            Assert.AreEqual("{\"type\":\"and\",\"clauses\":[" +
                "{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}," +
                "{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":100,\"lowerIncluded\":false}]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call And() with 1 clause,",
            expected = "We can get original clause"
            )]
        public void Test_0001_And_1 ()
        {
            KiiClause c = KiiClause.And(
                KiiClause.Equals("name", "kii")
                );

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call And() with 0 clause,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0002_And_0 ()
        {
            KiiClause.And();
        }

        [Test(), KiiUTInfo(
            action = "When we call And() with 2 clauses as array,",
            expected = "We can get and clause(see assertion)"
            )]
        public void Test_0010_And_array ()
        {
            KiiClause[] arr = new KiiClause[] {
                KiiClause.Equals("name", "kii"),
                KiiClause.GreaterThan("score", 100)
            };

            KiiClause c = KiiClause.And(arr);

            Assert.AreEqual("{\"type\":\"and\",\"clauses\":[" +
                "{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}," +
                "{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":100,\"lowerIncluded\":false}]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call And() with clauses as array that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0011_And_null ()
        {
            KiiClause.And(
                KiiClause.Equals("name", "kii"),
                null,
                KiiClause.GreaterThan("score", 100),
                null
                );
        }

        #endregion

        #region Or

        [Test(), KiiUTInfo(
            action = "When we call Or() with 2 clauses,",
            expected = "We can get or clause(see assertion)"
            )]
        public void Test_0100_Or ()
        {
            KiiClause c = KiiClause.Or(
                KiiClause.Equals("name", "kii"),
                KiiClause.GreaterThan("score", 100)
                );

            Assert.AreEqual("{\"type\":\"or\",\"clauses\":[" +
                "{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}," +
                "{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":100,\"lowerIncluded\":false}]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Or() with 1 clause,",
            expected = "We can get original clause"
            )]
        public void Test_0101_Or_1 ()
        {
            KiiClause c = KiiClause.Or(
                KiiClause.Equals("name", "kii")
                );

            Assert.AreEqual("{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Or() with 0 clause,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0102_Or_0 ()
        {
            KiiClause.Or();
        }

        [Test(), KiiUTInfo(
            action = "When we call Or() with 2 clauses as array,",
            expected = "We can get or clause(see assertion)"
            )]
        public void Test_0110_Or_array ()
        {
            KiiClause[] arr = new KiiClause[]
            {
                KiiClause.Equals("name", "kii"),
                KiiClause.GreaterThan("score", 100)
            };

            KiiClause c = KiiClause.Or(arr);

            Assert.AreEqual("{\"type\":\"or\",\"clauses\":[" +
                "{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}," +
                "{\"type\":\"range\",\"field\":\"score\",\"lowerLimit\":100,\"lowerIncluded\":false}]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Or() with clauses as array that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0111_Or_null ()
        {
            KiiClause.Or(
                KiiClause.Equals("name", "kii"),
                null,
                KiiClause.GreaterThan("score", 100),
                null
                );
        }

        #endregion

        #region In_int
        [Test(), KiiUTInfo(
            action = "When we call InWithIntValue() with 2 items,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0200_In_int ()
        {
            KiiClause c = KiiClause.InWithIntValue("score", 1, 2, 3);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,2,3]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithIntValue() with 1 item,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0201_In_int_1 ()
        {
            KiiClause c = KiiClause.InWithIntValue("score", 1);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithIntValue() with 0 item,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0202_In_int_0 ()
        {
            KiiClause.InWithIntValue("score");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithIntValue() with items that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0203_In_key_null ()
        {
            KiiClause.InWithIntValue(null, 1, 2, 3);
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithIntValue() with 3 items as array,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0204_In_int_array ()
        {
            int[] values = {1, 2, 3};
            KiiClause c = KiiClause.InWithIntValue("score", values);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,2,3]}", c.ToJson().ToString());
        }


        #endregion

        #region In_long
        [Test(), KiiUTInfo(
            action = "When we call InWithLongValue() with 3 items,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0300_In_long ()
        {
            KiiClause c = KiiClause.InWithLongValue("score", 1, (long)1234567890123, 3);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,1234567890123,3]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithLongValue() with 1 item,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0301_In_long_1 ()
        {
            KiiClause c = KiiClause.InWithLongValue("score", 1);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithLongValue() with 0 item,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0302_In_long_0 ()
        {
            KiiClause.InWithLongValue("score");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithLongValue() with 3 items that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0303_In_key_null ()
        {
            KiiClause.InWithLongValue(null, 1, 2, 3);
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithLongValue() with 3 items as array,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0304_In_long_array ()
        {
            long[] values = {1, 1234567890123, 3};
            KiiClause c = KiiClause.InWithLongValue("score", values);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,1234567890123,3]}", c.ToJson().ToString());
        }


        #endregion

        #region In_double
        [Test(), KiiUTInfo(
            action = "When we call InWithDoubleValue() with 3 items,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0400_In_double ()
        {
            KiiClause c = KiiClause.InWithDoubleValue("score", 1, 23.45, 3);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,23.45,3]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithDoubleValue() with 1 item,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0401_In_double_1 ()
        {
            KiiClause c = KiiClause.InWithDoubleValue("score", 1);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithDoubleValue() with 0 item,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0402_In_double_0 ()
        {
            KiiClause.InWithDoubleValue("score");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithDoubleValue() with 4 items that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0403_In_key_null ()
        {
            KiiClause.InWithDoubleValue(null, 1, 2, 3);
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithDoubleValue() with 3 items as array,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0404_In_double_array ()
        {
            double[] values = {1, 23.45, 3};
            KiiClause c = KiiClause.InWithDoubleValue("score", values);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"score\",\"values\":[1,23.45,3]}", c.ToJson().ToString());
        }

        #endregion

        #region In_string
        [Test(), KiiUTInfo(
            action = "When we call InWithStringValue() with 4 items,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0500_In_string ()
        {
            KiiClause c = KiiClause.InWithStringValue("name", "kaa", "kii", "kuu");

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"name\",\"values\":[\"kaa\",\"kii\",\"kuu\"]}", c.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithStringValue() with 1 item,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0501_In_string_1 ()
        {
            KiiClause c = KiiClause.InWithStringValue("name", "kii");

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"name\",\"values\":[\"kii\"]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithStringValue() with 0 item,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0502_In_string_0 ()
        {
            KiiClause.InWithStringValue("name");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithStringValue() with 4 items that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0503_In_key_null ()
        {
            KiiClause.InWithStringValue(null, "kaa", "kii", "kuu");
        }

        [Test(), KiiUTInfo(
            action = "When we call InWithStringValue() with 3 items as array,",
            expected = "We can get in clause(see assertion)"
            )]
        public void Test_0504_In_string_array ()
        {
            string[] values = {"kaa", "kii", "kuu"};
            KiiClause c = KiiClause.InWithStringValue("name", values);

            Assert.AreEqual("{\"type\":\"in\",\"field\":\"name\",\"values\":[\"kaa\",\"kii\",\"kuu\"]}", c.ToJson().ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call InWithStringValue() with 4 items as array that has null entry,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0505_In_value_null ()
        {
            KiiClause.InWithStringValue("name", "kaa", null, "kuu");
        }

        #endregion
    }
}

