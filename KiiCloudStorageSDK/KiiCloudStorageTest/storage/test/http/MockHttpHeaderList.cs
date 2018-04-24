using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    internal class MockHttpHeaderList : KiiHttpHeaderList
    {
        public IDictionary<string, string> values = new Dictionary<string, string>();

        internal MockHttpHeaderList ()
        {
        }
        public string this[string key]
        {
            set
            {
                values[key] = value;
            }
            get
            {
                return values[key];
            }
        }
    }
}

