using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class ReadExistingObjectsInBucketTest : LargeTestBase
    {
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        [SetUp ()]
        public override void SetUp ()
        {
            base.SetUp ();
            String username = "user" + CurrentTimeMillis();
            AppUtil.CreateNewUser(username, "dummypassword");
        }
        
        [TearDown ()]
        public override void TearDown ()
        {
            AppUtil.DeleteUser (KiiUser.CurrentUser);
            base.TearDown ();
        }

        private void CheckEntriesAfterAdd(KiiUser user, IList<KiiACLEntry<KiiBucket, BucketAction>> list)
        {
            Assert.AreEqual(5, list.Count);
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            foreach (KiiACLEntry<KiiBucket, BucketAction> ent in list)
            {
                switch(ent.Action)
                {
                    case BucketAction.CREATE_OBJECTS_IN_BUCKET:
                        ++count1;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.DROP_BUCKET_WITH_ALL_CONTENT:
                        ++count2;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.QUERY_OBJECTS_IN_BUCKET:
                        ++count3;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.READ_OBJECTS_IN_BUCKET:
                        ++count4;
                        if (ent.Subject is KiiUser)
                        {
                            Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        }
                        else
                        {
                            Assert.IsTrue(ent.Subject is KiiAnyAuthenticatedUser);
                        }
                        break;
                }
            }
            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
            Assert.AreEqual(1, count3);
            Assert.AreEqual(2, count4);
        }

        private void CheckEntriesAfterRemove(KiiUser user, IList<KiiACLEntry<KiiBucket, BucketAction>> list)
        {
            Assert.AreEqual(4, list.Count);
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            foreach (KiiACLEntry<KiiBucket, BucketAction> ent in list)
            {
                switch(ent.Action)
                {
                    case BucketAction.CREATE_OBJECTS_IN_BUCKET:
                        ++count1;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.DROP_BUCKET_WITH_ALL_CONTENT:
                        ++count2;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.QUERY_OBJECTS_IN_BUCKET:
                        ++count3;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                    case BucketAction.READ_OBJECTS_IN_BUCKET:
                        ++count4;
                        Assert.AreEqual(user.ID, ((KiiUser)ent.Subject).ID);
                        break;
                }
            }
            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
            Assert.AreEqual(1, count3);
            Assert.AreEqual(1, count4);
        }

        [Test()]
        public void TestReadObjectInBucket ()
        {
            CountDownLatch cd = new CountDownLatch(1);
            KiiUser user = KiiUser.CurrentUser;
            KiiBucket bucket = user.Bucket("aclTest");
            KiiBucketAcl acl = bucket.Acl(BucketAction.READ_OBJECTS_IN_BUCKET);

            KiiACLEntry<KiiBucket, BucketAction> entry = acl.Subject(KiiAnyAuthenticatedUser.Get());

            entry.Save(ACLOperation.GRANT, (KiiACLEntry<KiiBucket, BucketAction> savedEntry, Exception e) => {
                Assert.IsNull(e);
                Assert.AreEqual(BucketAction.READ_OBJECTS_IN_BUCKET, savedEntry.Action);
                bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> list, Exception e2) => {
                    Assert.IsNull(e2);
                    CheckEntriesAfterAdd(user, list);

                    // Remove ACL Entry
                    entry.Save(ACLOperation.REVOKE, (KiiACLEntry<KiiBucket, BucketAction> savedEntry2, Exception e3) => {
                        Assert.IsNull(e3);
                        Assert.AreEqual(BucketAction.READ_OBJECTS_IN_BUCKET, savedEntry2.Action);

                        bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> list2, Exception e4) => {
                            Assert.IsNull(e4);
                            CheckEntriesAfterRemove(user, list2);
                            cd.Signal();
                        });
                    });
                });
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
        }
    }
}

