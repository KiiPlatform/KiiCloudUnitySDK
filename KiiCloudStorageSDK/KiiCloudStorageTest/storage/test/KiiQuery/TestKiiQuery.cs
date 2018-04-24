using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiQuery
    {
        #region constructor
        [Test(), KiiUTInfo(
            action = "When we create KiiQuery with no argument,",
            expected = "ToString() must be output All Query"
            )]
        public void Test_0000_KiiQuery ()
        {
            KiiQuery query = new KiiQuery();

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}}}", query.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create KiiQuery with null,",
            expected = "ToString() must be output All Query"
            )]
        public void Test_0001_KiiQuery_null ()
        {
            KiiQuery query = new KiiQuery(null);

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}}}", query.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create KiiQuery with a KiiClause,",
            expected = "ToString() must be output JsonString with KiiClause.ToString()"
            )]
        public void Test_0002_KiiQuery_clause ()
        {
            KiiQuery query = new KiiQuery(KiiClause.Equals("name", "kii"));

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"eq\",\"field\":\"name\",\"value\":\"kii\"}}}", query.ToString());
        }

        #endregion


        #region sort

        [Test(), KiiUTInfo(
            action = "When we call SortByAsc(),",
            expected = "ToString() must be output JsonString with orderBy and descending=false field"
            )]
        public void Test_0100_SortByAsc ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByAsc("score");

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"},\"orderBy\":\"score\",\"descending\":false}}", query.ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SortByAsc() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0101_SortByAsc_null ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByAsc(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SortByAsc() with empty string,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0102_SortByAsc_empty ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByAsc("");
        }

        [Test(), KiiUTInfo(
            action = "When we call SortByDesc(),",
            expected = "ToString() must be output JsonString with orderBy and descending=true field"
            )]
        public void Test_0200_SortByDesc ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByDesc("score");

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"},\"orderBy\":\"score\",\"descending\":true}}", query.ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SortByDesc() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0201_SortByDesc_null ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByDesc(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SortByDesc() with empty string,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0202_SortByDesc_empty ()
        {
            KiiQuery query = new KiiQuery();
            query.SortByDesc("");
        }

        #endregion

        #region Limit
        [Test(), KiiUTInfo(
            action = "When we set Limit,",
            expected = "ToString() must be output JsonString with bestEffortLimit field"
            )]
        public void Test_0300_Limit ()
        {
            KiiQuery query = new KiiQuery();
            query.Limit = 10;

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}},\"bestEffortLimit\":10}", query.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we set Limit = 1,",
            expected = "ToString() must be output JsonString with bestEffortLimit field"
            )]
        public void Test_0301_Limit_1 ()
        {
            KiiQuery query = new KiiQuery();
            query.Limit = 1;

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}},\"bestEffortLimit\":1}", query.ToString());
        }
            
        [Test(), KiiUTInfo(
            action = "When we set Limit = 0,",
            expected = "ToString() must be output JsonString without bestEffortLimit field"
            )]
        public void Test_0302_Limit_0 ()
        {
            KiiQuery query = new KiiQuery();
            query.Limit = 0;
            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}}}", query.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we set Limit = negative value,",
            expected = "ToString() must be output JsonString without bestEffortLimit field"
        )]
        public void Test_0303_Limit ()
        {
            KiiQuery query = new KiiQuery();
            query.Limit = -1;
            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}}}", query.ToString());
        }
        #endregion

        #region NextPaginationKey

        [Test(), KiiUTInfo(
            action = "When we set NextPaginationKey,",
            expected = "ToString() must be output JsonString with paginationKey field"
            )]
        public void Test_0400_NextPaginationKey ()
        {
            KiiQuery query = new KiiQuery();
            query.NextPaginationKey = "abcd";

            Assert.AreEqual("{\"bucketQuery\":{\"clause\":{\"type\":\"all\"}},\"paginationKey\":\"abcd\"}", query.ToString());
        }

        #endregion
    }
}

