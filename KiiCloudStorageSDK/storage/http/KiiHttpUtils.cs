using System;
using System.Net;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Kii http utils.
    /// </summary>
    /// <remarks>
    /// For internal use.
    /// </remarks>
    public class KiiHttpUtils
    {
        private KiiHttpUtils ()
        {
        }
        /// <summary>
        /// Convert Http status into the KiiCloudException.
        /// </summary>
        /// <returns>KiiCloudException</returns>
        /// <param name="statusCode">Http status code.</param>
        /// <param name="body">Response body.</param>
        /// <remarks></remarks>
        public static CloudException TypedException(int statusCode, string body)
        {
            string errorCode = null;
            try {
                // This if statement is necessary for avoiding crash on iOS.
                if (!Utils.IsEmpty(body))
                {
                    JsonObject obj = new JsonObject (body);
                    errorCode = obj.GetString ("errorCode");
                }
            } catch (Exception) {
                // Nothing to do.
            }
            switch(statusCode) {
            case 400:
                BadRequestException.Reason reason = BadRequestException.Reason.__UNKNOWN__;
                if (errorCode != null && Enum.IsDefined (typeof(BadRequestException.Reason), errorCode)) {
                    reason = (BadRequestException.Reason)Enum.Parse(typeof (BadRequestException.Reason), errorCode);
                }
                return new BadRequestException ("Bad request", null, body, reason);
            case 401:
                return new UnauthorizedException ("Request is not authorized", null, body);
            case 403:
                return new ForbiddenException ("Forbidden request", null, body);
            case 404:
                NotFoundException.Reason reasonNotFound = NotFoundException.Reason.__UNKNOWN__;
                if (errorCode != null && Enum.IsDefined (typeof(NotFoundException.Reason), errorCode)) {
                    reasonNotFound = (NotFoundException.Reason)Enum.Parse(typeof(NotFoundException.Reason), errorCode);
                }
                return new NotFoundException ("Requested entity not found", null, body, reasonNotFound);
            case 409:
                ConflictException.Reason reasonConflict = ConflictException.Reason.__UNKNOWN__;
                if (errorCode != null && Enum.IsDefined (typeof(ConflictException.Reason), errorCode)) {
                    reasonConflict = (ConflictException.Reason)Enum.Parse(typeof(ConflictException.Reason), errorCode);
                }
                return new ConflictException ("Conflict happens", null, body, reasonConflict);
            case 410:
                return new GoneException ("Requested resource is no longer available and will not be available again", null, body);
            default:
                return new CloudException (statusCode, body);
            }
        }
    }
}

