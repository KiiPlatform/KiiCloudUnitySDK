using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiSet
    {
        [Test(), KiiUTInfo(
            action = "When we add 3 different items to KiiSet,",
            expected = "The Count must be 3"
            )]
        public void Test_0000_Add ()
        {
            KiiSet<string> kiiSet = new KiiSet<string>();
            kiiSet.Add("a");
            kiiSet.Add("b");
            kiiSet.Add("c");
            Assert.AreEqual(3, kiiSet.Count);
        }

        [Test(), KiiUTInfo(
            action = "When we add 3 items (a, b, a) to KiiSet,",
            expected = "The Count must be 2 because a is duplicated"
            )]
        public void Test_0001_Add_duplicate ()
        {
            KiiSet<string> kiiSet = new KiiSet<string>();
            kiiSet.Add("a");
            kiiSet.Add("b");
            kiiSet.Add("a");
            Assert.AreEqual(2, kiiSet.Count);
        }


    }
}

