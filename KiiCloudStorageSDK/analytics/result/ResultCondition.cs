using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Contains the conditions for getting analytics results.
    /// </summary>
    /// <remarks>
    /// Key(argument of AddFilter and GroupingKey) must batch &quot;^[a-zA-Z][a-zA-Z0-9_]{0,63}$&quot;
    /// </remarks>
    public class ResultCondition
    {
        private string mGroupingKey;
        private DateRange mDateRange;
        private IDictionary<string, string> filterMap = new Dictionary<string, string>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Analytics.ResultCondition"/> class.
        /// </summary>
        public ResultCondition ()
        {
        }
        
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid.
        /// </exception>
        public void AddFilter(string key, bool value)
        {
            CheckKey(key);
            filterMap[key] = (value) ? "true" : "false";
        }
  
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid.
        /// </exception>
        public void AddFilter(string key, int value)
        {
            CheckKey(key);
            filterMap[key] = "" + value;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid.
        /// </exception>
        public void AddFilter(string key, long value)
        {
            CheckKey(key);
            filterMap[key] = "" + value;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid.
        /// </exception>
        public void AddFilter(string key, float value)
        {
            CheckKey(key);
            filterMap[key] = "" + value;
        }
        
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid.
        /// </exception>
        public void AddFilter(string key, double value)
        {
            CheckKey(key);
            filterMap[key] = "" + value;
        }
        
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <remarks>
        /// Key must be valid
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when key is null or value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when key is invalid or value is empty string.
        /// </exception>
        public void AddFilter(string key, string value)
        {
            CheckKey(key);
            if (value == null)
            {
                throw new ArgumentNullException("Value must not be null");
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("Value must not be empty");         
            }
            filterMap[key] = value;
        }
        
        internal string ToQueryString() 
        {
            StringBuilder sb = new StringBuilder();
            
            if (mGroupingKey != null)
            {
                sb.Append("&group=").Append(Uri.EscapeUriString(mGroupingKey));
            }
            if (mDateRange != null)
            {
                sb.Append("&").Append(mDateRange.ToQueryString());
            }
            int i = 0;
            foreach (KeyValuePair<string, string> entry in filterMap)
            {
                sb.Append("&filter").Append(i).Append(".name=").Append(Uri.EscapeUriString(entry.Key));
                sb.Append("&filter").Append(i).Append(".value=").Append(Uri.EscapeUriString(entry.Value));
                
                ++i;
            }
            
            if (sb.Length == 0)
            {
                return "";
            }
            return sb.ToString().Substring(1);
        }
  
        private void CheckKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(ErrorInfo.KIIEVENT_KEY_NULL);
            }            
            if (!KiiEvent.IsValidKey(key))
            {
                throw new ArgumentException(ErrorInfo.KIIEVENT_KEY_INVALID);
            }
        }
        #region properties

        /// <summary>
        /// Sets the grouping key.
        /// </summary>
        /// <remarks>
        /// A value must be valid. 
        /// </remarks>
        /// <value>
        /// The grouping key.
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when value is invalid.
        /// </exception>
        public string GroupingKey
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(ErrorInfo.KIIEVENT_KEY_NULL);
                }
                if (!KiiEvent.IsValidKey(value))
                {
                    throw new ArgumentException(ErrorInfo.KIIEVENT_KEY_INVALID);
                }
                mGroupingKey = value;
            }
        }
        
        /// <summary>
        /// Sets the date range.
        /// </summary>
        /// <remarks>
        /// Value must not be null.
        /// </remarks>
        /// <value>
        /// The date range.
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when value is null.
        /// </exception>
        public DateRange DateRange
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(ErrorInfo.KIIANALYTICS_DATARANGE_NULL);
                }
                mDateRange = value;
            }
        }
        
        #endregion
    }
}

