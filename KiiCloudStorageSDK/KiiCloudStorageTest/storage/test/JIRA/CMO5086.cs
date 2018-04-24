using System;
using System.Reflection;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class CMO5086
    {

         [Test()]
        public void TestHTTP400_Reason_Unknown ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"\"" +"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.__UNKNOWN__);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP400_Reason_INVALID_JSON ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"INVALID_JSON\"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.INVALID_JSON);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP400_Reason_INVALID_BUCKET ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"INVALID_BUCKET\"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.INVALID_BUCKET);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP400_Reason_QUERY_NOT_SUPPORTED ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"QUERY_NOT_SUPPORTED\"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.QUERY_NOT_SUPPORTED);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP400_Reason_INVALID_INPUT_DATA ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"INVALID_INPUT_DATA\"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.INVALID_INPUT_DATA);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP400_Reason_INVALID_ACCOUNT_STATUS ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(400,"{" +
                    "\"errorCode\" : \"INVALID_ACCOUNT_STATUS\"}");
            }catch(BadRequestException be)
            {
                Assert.AreEqual(be.reason,BadRequestException.Reason.INVALID_ACCOUNT_STATUS);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test(), ExpectedException(typeof(UnauthorizedException)), KiiUTInfo(
            action = "When http 401 received, should throw UnauthorizedException",
            expected = "UnauthorizedException must be thrown"
            )]
        public void TestHTTP401 ()
        {
            throw KiiHttpUtils.TypedException(401,"{" +
                    "\"errorCode\" : \"UNAUTHORIZED\"}");
        }

        [Test(), ExpectedException(typeof(ForbiddenException)), KiiUTInfo(
            action = "When http 403 received, should throw UnauthorizedException",
            expected = "ForbiddenException must be thrown"
            )]
        public void TestHTTP403 ()
        {
            throw KiiHttpUtils.TypedException(403,"{" +
                    "\"errorCode\" : \"INVALID_VERIFICATION_CODE\"}");
        }


        [Test()]
        public void TestHTTP404_Reason_UNKNOWN ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.__UNKNOWN__);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_ACL_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"ACL_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.ACL_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_BUCKET_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"BUCKET_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.BUCKET_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_OBJECT_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"OBJECT_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.OBJECT_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_USER_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"USER_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.USER_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_GROUP_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"GROUP_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.GROUP_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

         [Test()]
        public void TestHTTP404_Reason_OBJECT_BODY_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"OBJECT_BODY_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.OBJECT_BODY_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

         [Test()]
        public void TestHTTP404_Reason_APP_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"APP_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.APP_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }



        [Test()]
        public void TestHTTP404_Reason_USER_ADDRESS_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"USER_ADDRESS_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.USER_ADDRESS_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_TOPIC_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"TOPIC_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.TOPIC_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_FILTER_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"FILTER_NOT_FOUND\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.FILTER_NOT_FOUND);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP404_Reason_PUSH_SUBSCRIPTION_NOT_FOUND ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(404,"{" +
                    "\"errorCode\" : \"\"}");
            }catch(NotFoundException ne)
            {
                Assert.AreEqual(ne.reason,NotFoundException.Reason.__UNKNOWN__);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_UNKNOWN ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.__UNKNOWN__);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_ACL_ALREADY_EXISTS ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"ACL_ALREADY_EXISTS\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.ACL_ALREADY_EXISTS);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_BUCKET_ALREADY_EXISTS ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"BUCKET_ALREADY_EXISTS\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.BUCKET_ALREADY_EXISTS);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_OBJECT_VERSION_IS_STALE ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"OBJECT_VERSION_IS_STALE\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.OBJECT_VERSION_IS_STALE);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_OBJECT_ALREADY_EXISTS ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"OBJECT_ALREADY_EXISTS\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.OBJECT_ALREADY_EXISTS);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test()]
        public void TestHTTP409_Reason_USER_ALREADY_EXISTS ()
        {
            // call methods
            try
            {
                throw KiiHttpUtils.TypedException(409,"{" +
                    "\"errorCode\" : \"USER_ALREADY_EXISTS\"}");
            }catch(ConflictException ce)
            {
                Assert.AreEqual(ce.reason,ConflictException.Reason.USER_ALREADY_EXISTS);
            }catch(Exception ex)
            {
                Assert.Fail("throw unexpected exception " + ex);
            }

        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When http 500 received, should throw CloudException",
            expected = "CloudException must be thrown"
            )]
        public void TestHTTP500 ()
        {

            throw KiiHttpUtils.TypedException(500,"{" +
                    "\"errorCode\" : \"\"}");
        }

         [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When http 504 received, should throw CloudException",
            expected = "CloudException must be thrown"
            )]
        public void TestHTTP504 ()
        {

            throw KiiHttpUtils.TypedException(504,"{" +
                    "\"errorCode\" : \"\"}");
        }

         [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When http 505 received, should throw CloudException",
            expected = "CloudException must be thrown"
            )]
        public void TestHTTP505 ()
        {

            throw KiiHttpUtils.TypedException(505,"{" +
                    "\"errorCode\" : \"\"}");
        }
    }
}

