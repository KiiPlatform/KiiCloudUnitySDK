using System;

namespace KiiCorp.Cloud.Storage
{
    internal class ErrorInfo
    {
        public const string KII_APP_ID_IS_NULL = "AppId is null";
        public const string KII_APP_KEY_IS_NULL = "AppKey is null";
        public const string KII_SERVER_URL_IS_NULL = "serverUrl is null";
        public const string KII_INVALID_SERVER_URL = "Invalid serverUrl";
        public const string KII_SITE_INVALID = "Invalid Site";

        public const string KIIBASEOBJECT_INVALID_FORMAT = "Invalid format:";
        public const string KIIBASEOBJECT_NO_ID = "Missing unique identifier";
        public const string KIIBASEOBJECT_URL_IS_NULL = "Input uri is null";
        public const string KIIBASEOBJECT_URI_NO_SUPPORT = "Do not support this uri:";
        public const string KIIBASEOBJECT_MISS_ENTITY = "Missing entity type";


        public const string KIIUSER_PASSWORD_INVALID = "Invalid password format:";
        public const string KIIUSER_EMAIL_EMPTY = "Email cannot be empty";
        public const string KIIUSER_PHONE_EMPTY = "Phone cannot be empty";
        public const string KIIUSER_PHONE_FORMAT_INVALID = "Invalid Phone format:";
        public const string KIIUSER_LOCAL_PHONE_INVALID = "Requires a country code to local phone number.";
        public const string KIIUSER_EMAIL_FORMAT_INVALID = "Invalid Email format:";
        public const string KIIUSER_USERNAME_INVALID = "Invalid username format:";
        public const string KIIUSER_DISPLAYNAME_INVALID = "Invalid displayName format:";
        public const string KIIUSER_COUNTRY_INVALID = "Invalid country format:";
        public const string KIIUSER_REGISTER_EMPTY = "Username or email or phone cannot be empty";
        public const string KIIUSER_URI_IS_NULL = "Input uri is null";
        public const string KIIUSER_URI_NO_SUPPORT = "Do not support this uri:";
        public const string KIIUSER_NO_ID = "Object doesn't exist in the cloud, missing unique identifier.";
        public const string KIIUSER_NOT_MODIFY = "Cannot update except owner.";
        public const string KIIUSER_NEW_PASSWORD_INVALID = "Invalid password format for new:";
        public const string KIIUSER_OLD_PASSWORD_INVALID = "Invalid password format for old:";
        public const string KIIUSER_NOT_MODIFY_PWD = "Cannot update password except owner";
        public const string KIIUSER_DATA_PROBLEM = "The current user data has problem";
        public const string KIIUSER_UNSPECIFIED_IDENTIFIER = "Unspecified identifier:";

        public const string UTILS_KIICLIENT_NULL = "KiiClient is not initialized!";
        public const string UTILS_NO_LOGIN = "No login user, please login first!";

        public const string KIIBUCKET_NAME_INVALID = "Invalid bucketName : ";
        public const string KIIBUCKET_UNKNOWN_SCOPE = "Unknown scope";
        public const string KIIBUCKET_NO_LOGIN = "No login user";
        public const string KIIBUCKET_NO_GROUP_ID = "Group ID is null";

        public const string KIICLAUSE_KEY_NULL = "key is null";
        public const string KIICLAUSE_VALUE_NULL = "value is null";
        public const string KIICLAUSE_CLAUSE_NULL = "clause is null";
        public const string KIICLAUSE_VALUE_EXCEED_MAX_LENGTH = "length of values exceeds maximum of 200";

        public const string KIIGROUP_NAME_NULL = "Group name is null";
        public const string KIIGROUP_URL_IS_NULL = "Input uri is null";
        public const string KIIGROUP_URI_NO_SUPPORT = "Do not support this uri:";
        public const string KIIGROUP_NO_ID = "Group doesn't exist in the cloud, missing unique identifier.";

        public const string KIIPUSHINSTALLATION_DEVICE_ID_NULL = "registrationID/deviceToken must not be null/empty";

        public const string KIIPUSHSUBSCRIPTION_USER_NULL = "User must not be null";
        public const string KIIPUSHSUBSCRIPTION_TAGET_NULL = "target must not be null";
    }
}

