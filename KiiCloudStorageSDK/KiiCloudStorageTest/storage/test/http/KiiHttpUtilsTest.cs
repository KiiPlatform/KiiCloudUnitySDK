using System;
using System.Collections.Generic;
using JsonOrg;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiHttpUtilsTest
    {
        [Test()]
        public void TypedExceptionMappingTest()
        {
            Dictionary<int, Type> testCases = new Dictionary<int, Type>();
            testCases.Add(400, typeof(BadRequestException));
            testCases.Add(401, typeof(UnauthorizedException));
            testCases.Add(403, typeof(ForbiddenException));
            testCases.Add(404, typeof(NotFoundException));
            testCases.Add(409, typeof(ConflictException));
            testCases.Add(410, typeof(GoneException));
            testCases.Add(500, typeof(CloudException));

            foreach (int status in testCases.Keys)
            {
                Exception e = KiiHttpUtils.TypedException(status, "");
                if (e.GetType() != testCases [status])
                {
                    Assert.Fail("expected exception class is " + testCases [status].ToString() + " but actual is " + e.GetType().ToString());
                }
                Assert.AreEqual(status, ((CloudException)e).Status);
            }
        }

    }
}

