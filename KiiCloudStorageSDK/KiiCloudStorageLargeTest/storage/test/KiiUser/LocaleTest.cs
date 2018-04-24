using System;
using System.Globalization;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class LocaleTest : LargeTestBase
    {
        [SetUp ()]
        public override void SetUp ()
        {
            base.SetUp ();
        }

        [TearDown ()]
        public override void TearDown ()
        {
            base.TearDown ();
        }

        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        [Test ()]
        [Ignore("This test case spends long time.")]
        public void TestAllCultures ()
        {
            KiiUser user = KiiUser.BuilderWithEmail(CurrentTimeMillis() + "@kii.com").Build();
            user.Register("password");
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                user.Locale = new LocaleContainer(cultureInfo);
                user.Update();
                KiiUser actual = KiiUser.UserWithID(user.ID);
                actual.Refresh();
                Assert.AreEqual(cultureInfo, actual.Locale.CultureInfo, cultureInfo.ToString());
            }
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                if (cultureInfo.IsNeutralCulture)
                {
                    user.Locale = new LocaleContainer(cultureInfo);
                    user.Update();
                    KiiUser actual = KiiUser.UserWithID(user.ID);
                    actual.Refresh();
                    String expected = cultureInfo.Name.ToLower();
                    if (expected == "zh-chs")
                    {
                        expected = "zh-cn";
                    }
                    else if (expected == "zh-cht")
                    {
                        expected = "zh-hk";
                    }
                    Assert.AreEqual(expected, actual.Locale.LocaleString, cultureInfo.Name);
                }
            }
        }

    }
}

