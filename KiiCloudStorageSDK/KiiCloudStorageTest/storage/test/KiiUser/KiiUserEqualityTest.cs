using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUserEqualityTest
    {
        public static KiiUserEqualityTestData[] EQUALS_TESTDATA;
        public static KiiUserEqualityTestData[] EQUALITY_TESTDATA;

        static KiiUserEqualityTest()
        {
            string username = TextUtils.randomAlphaNumeric(10);
            KiiUser notSavedUser = KiiUser.BuilderWithName(username).Build();
            KiiUser notSavedUserDiff = KiiUser.BuilderWithName(username).Build();
            Uri userUri = new Uri("kiicloud://users/"
                          + TextUtils.generateUUID());
            KiiUser savedUser = KiiUser.CreateByUri(userUri);
            KiiUser savedUserDiff = KiiUser.CreateByUri(userUri);
            string displayname = TextUtils.randomAlphaNumeric(10);
            KiiUser savedUserWithDisplayname = KiiUser.CreateByUri(userUri);
            savedUser.Displayname = displayname;

            EQUALS_TESTDATA = new KiiUserEqualityTestData[] {
                new KiiUserEqualityTestData(notSavedUser, null, false, "Compare notSavedUser and null user"),
                new KiiUserEqualityTestData(notSavedUser, notSavedUser, true, "Compare notSavedUser and notSavedUser"),
                new KiiUserEqualityTestData(notSavedUser, notSavedUserDiff, false, "Compare notSavedUser and notSavedUserDiff"),
                new KiiUserEqualityTestData(savedUser, null, false, "Compare savedUser and null user"),
                new KiiUserEqualityTestData(savedUser, notSavedUser, false, "Compare savedUser and notSavedUser"),
                new KiiUserEqualityTestData(savedUser, savedUser, true, "Compare savedUser and savedUser"),
                new KiiUserEqualityTestData(savedUser, savedUserDiff, true, "Compare savedUser and savedUserDiff"),
                new KiiUserEqualityTestData(savedUser, savedUserWithDisplayname, true, "Compare savedUser and savedUserWithDisplayname")
            };

            EQUALITY_TESTDATA = new KiiUserEqualityTestData[] {
                new KiiUserEqualityTestData(notSavedUser, null, false, "Compare notSavedUser and null user"),
                new KiiUserEqualityTestData(notSavedUser, notSavedUser, true, "Compare notSavedUser and notSavedUser"),
                new KiiUserEqualityTestData(notSavedUser, notSavedUserDiff, false, "Compare notSavedUser and notSavedUserDiff"),
                new KiiUserEqualityTestData(savedUser, null, false, "Compare savedUser and null user"),
                new KiiUserEqualityTestData(savedUser, notSavedUser, false, "Compare savedUser and notSavedUser"),
                new KiiUserEqualityTestData(savedUser, savedUser, true, "Compare savedUser and savedUser"),
                new KiiUserEqualityTestData(savedUser, savedUserDiff, false, "Compare savedUser and savedUserDiff"),
                new KiiUserEqualityTestData(savedUser, savedUserWithDisplayname, false, "Compare savedUser and savedUserWithDisplayname")
            };
        }

        [Test()]
        public void testKiiUserEquality()
        {
            SortedList<int, AssertionException> failureList = new SortedList<int, AssertionException>();
            int count = 0;
            foreach (KiiUserEqualityTestData testData in EQUALS_TESTDATA)
            {
                try
                {
                    Console.WriteLine("Test : " + count);
                    if (testData.SuccessExpected)
                    {
                        Assert.IsTrue(testData.TargetUser.Equals(
                            testData.ComparisionUser), testData.Description);
                    }
                    else
                    {
                        Assert.IsFalse(testData.TargetUser.Equals(
                            testData.ComparisionUser), testData.Description);
                    }
                }
                catch (AssertionException e)
                {
                    Console.WriteLine(e.StackTrace);
                    failureList.Add(count, e);
                }
                finally
                {
                    count++;
                }
            }
            assertResult(EQUALS_TESTDATA, failureList);
        }

        [Test()]
        public void testKiiUserEqualityByEqualOperator()
        {
            SortedList<int, AssertionException> failureList = new SortedList<int, AssertionException>();
            int count = 0;
            foreach (KiiUserEqualityTestData testData in EQUALITY_TESTDATA)
            {
                try
                {
                    Console.WriteLine("Test : " + count);
                    if (testData.SuccessExpected)
                    {
                        Assert.IsTrue((testData.TargetUser == testData.ComparisionUser), testData.Description);
                    }
                    else
                    {
                        Assert.IsFalse((testData.TargetUser == testData.ComparisionUser), testData.Description);
                    }
                }
                catch (AssertionException e)
                {
                    Console.WriteLine(e.StackTrace);
                    failureList.Add(count, e);
                }
                finally
                {
                    count++;
                }
            }
            assertResult(EQUALITY_TESTDATA, failureList);
        }

        [Test()]
        public void testKiiUserEqualityByNotEqualOperator()
        {
            SortedList<int, AssertionException> failureList = new SortedList<int, AssertionException>();
            int count = 0;
            foreach (KiiUserEqualityTestData testData in EQUALITY_TESTDATA)
            {
                try
                {
                    Console.WriteLine("Test : " + count);
                    if (testData.SuccessExpected)
                    {
                        Assert.IsFalse((testData.TargetUser != testData.ComparisionUser), testData.Description);
                    }
                    else
                    {
                        Assert.IsTrue((testData.TargetUser != testData.ComparisionUser), testData.Description);
                    }
                }
                catch (AssertionException e)
                {
                    Console.WriteLine(e.StackTrace);
                    failureList.Add(count, e);
                }
                finally
                {
                    count++;
                }
            }
            assertResult(EQUALITY_TESTDATA, failureList);
        }

        private void assertResult(KiiUserEqualityTestData[] testData, SortedList<int, AssertionException> failureList)
        {
            if (failureList == null || failureList.Count == 0)
            {
                // Test is finished successfully.
                return;
            }

            string message = failureList.Count + " Test(s) failed : ";
            foreach (KeyValuePair<int, AssertionException> pair in failureList)
            {
                message += "\n";
                message += testData[pair.Key].Description;
                message += "\n";
                message += pair.Value.StackTrace;
            }
            Assert.Fail(message);
        }

        public class KiiUserEqualityTestData
        {
            private KiiUser targetUser;
            private KiiUser comparisonUser;
            private bool successExpected;
            private string description;

            public KiiUserEqualityTestData(KiiUser targetUser, KiiUser comparisonUser,
                                           bool successExpected, string description = "")
            {
                this.targetUser = targetUser;
                this.comparisonUser = comparisonUser;
                this.successExpected = successExpected;
                this.description = description;
            }

            public KiiUser TargetUser
            {
                get
                {
                    return targetUser;
                }
            }

            public KiiUser ComparisionUser
            {
                get
                {
                    return comparisonUser;
                }
            }

            public bool SuccessExpected
            {
                get
                {
                    return successExpected;
                }
            }

            public string Description
            {
                get
                {
                    return description;
                }
            }
        }
    }
}

