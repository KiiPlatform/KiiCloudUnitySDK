using System;
using System.Text;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Contains the range of date.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DateRange
    {
        private DateTime mStart;
        private DateTime mEnd;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Analytics.DateRange"/> class.
        /// </summary>
        /// <remarks>
        /// Arguments must be start &lt;= end
        /// </remarks>
        /// <param name='start'>
        /// Start date.
        /// </param>
        /// <param name='end'>
        /// End date.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when start &gt; end
        /// </exception>
        public DateRange (DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException(ErrorInfo.DATARANGE_INVALID_START_END);
            }
            this.mStart = start;
            this.mEnd = end;
        }
        
        internal string ToQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("startDate=").Append(Uri.EscapeUriString(mStart.ToString("yyyy-MM-dd")));
            sb.Append("&endDate=").Append(Uri.EscapeUriString(mEnd.ToString("yyyy-MM-dd")));
            
            return sb.ToString();
        }
        
        internal DateTime Start
        {
            get
            {
                return mStart;
            }
        }
        
        internal DateTime End
        {
            get
            {
                return mEnd;
            }
        }
        
        
    }
}

