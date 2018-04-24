using System;

namespace JsonOrg
{
    /// <summary>
    /// JSON null class
    /// </summary>
    public class JsonNull
    {
        public override bool Equals(object o) {
                return o == this || o == null; // API specifies this broken equals implementation
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override string ToString() {
                return "null";
        }

    }
}

