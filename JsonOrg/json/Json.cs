using System;

namespace JsonOrg
{
    internal class Json {
        /**
         * Returns the input if it is a JSON-permissible value; throws otherwise.
         */
        internal static double CheckDouble(double d) {
            if (double.IsInfinity(d) || double.IsNaN(d)) {
                throw new JsonException("Forbidden numeric value: " + d);
            }
            return d;
        }
    
        internal static bool ToBoolean(object value) {
            if (value is bool) {
                return (bool) value;
            } else if (value is string) {
                string stringValue = (string) value;
                if (string.Compare(stringValue, "true", true) == 0) {
                    return true;
                } else if (string.Compare(stringValue, "false", true) == 0) {
                    return false;
                }
            }
            throw new FormatException();
        }
    
        internal static double ToDouble(object value) {
            if (value is double) {
                return (double) value;
            } else if (Json.IsNumber(value)) {
                return Convert.ToDouble(value);
            } else if (value is string) {
                try {
                    return double.Parse((string) value);
                } catch {
                }
            }
            throw new FormatException();
        }
    
        internal static int ToInteger(object value) {
            if (value is int) {
                return (int) value;
            } else if (Json.IsNumber(value)) {
                return Convert.ToInt32(value);
            } else if (value is string) {
                try {
                    return (int) double.Parse((string) value);
                } catch {
                }
            }
            throw new FormatException();
        }
    
        internal static long ToLong(object value) {
            if (value is long) {
                return (long) value;
            } else if (Json.IsNumber(value)) {
                return Convert.ToInt64(value);
            } else if (value is string) {
                try {
                    return (long) double.Parse((string) value);
                } catch {
                }
            }
            throw new FormatException();
        }
    
        internal static string ToString(object value) {
            if (value is string)
            {
                return (string)value;
            } else if (value is bool)
            {
                return value.ToString().ToLower();
            }
            else if (value != null) {
                return value.ToString();
            }
            return null;
        }

        internal static bool IsNumber(object value) {
            return (value is int) || (value is long) || (value is float) || (value is double) ||
                (value is Int32) || (value is Int64);
        }
    
        public static JsonException TypeMismatch(object indexOrName, object actual,
                String requiredType) {
            if (actual == null) {
                throw new JsonException("Value at " + indexOrName + " is null.");
            } else {
                throw new JsonException("Value " + actual + " at " + indexOrName
                        + " of type " + actual.GetType()
                        + " cannot be converted to " + requiredType);
            }
        }

        public static JsonException TypeMismatch(object actual, string requiredType) {
            if (actual == null) {
                throw new JsonException("Value is null.");
            } else {
                throw new JsonException("Value " + actual
                        + " of type " + actual.GetType()
                        + " cannot be converted to " + requiredType);
            }
        }
    }
}

