using System;

namespace KiiCorp.Cloud.Analytics
{
    internal class ErrorInfo
    {
        internal const string KIIANALYTICS_UNKNOWN_SITE = "Unknown site";
        internal const string KIIANALYTICS_APPINFO_NULL = "AppID / AppKey / ServerUrl / DeviceID must not be null";
        internal const string KIIANALYTICS_EVENT_NULL = "Event must not be null";
        internal const string KIIANALYTICS_EVENT_TOO_LONG = "Length of eventList must be <= 50";
        internal const string KIIANALYTICS_EVENT_ALREADY_SENT = "Event is already sent to KiiCloud";
        internal const string KIIANALYTICS_INVALID_SERVER_URL = "Invalid serverUrl";

        internal const string KIIANALYTICS_RULE_ID_NULL = "rule id must not be null";
        
        internal const string KIIANALYTICS_DATARANGE_NULL = "DataRange must not be null";
            
        internal const string KIIEVENT_TYPE_INVALID = "Invalid event type";
        internal const string KIIEVENT_KEY_INVALID = "Invalid key";
        internal const string KIIEVENT_KEY_NULL = "Key must not be null";
        internal const string KIIEVENT_VALUE_NULL = "Value must not be null";
        internal const string KIIEVENT_VALUE_EMPTY = "Value must not be empty string";
        internal const string KIIEVENT_VALUE_DOUBLE = "The type of Value must not be double";
        
        internal const string DATARANGE_INVALID_START_END = "args must be start <= end";
    }
}

