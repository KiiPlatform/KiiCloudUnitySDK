using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace JsonOrg
{
    public class JsonArray {
    
        private List<object> values;
    
        /**
         * Creates a {@code JSONArray} with no values.
         */
        public JsonArray() {
            values = new List<object>();
        }

        public static JsonArray FromCollection<T>(ICollection<T> collection) {
            JsonArray array = new JsonArray();
            foreach (T o in collection) {
                array.values.Add(o);
            }
            return array;
        }

        /**
         * Creates a new {@code JSONArray} with values from the next array in the
         * tokener.
         *
         * @param readFrom a tokener whose nextValue() method will yield a
         *     {@code JSONArray}.
         * @throws JSONException if the parse fails or doesn't yield a
         *     {@code JSONArray}.
         */
        internal JsonArray(JsonTokener readFrom) {
            /*
             * Getting the parser to populate this could get tricky. Instead, just
             * parse to temporary JSONArray and then steal the data from that.
             */
            object obj = readFrom.NextValue();
            if (obj is JsonArray) {
                values = ((JsonArray) obj).values;
            } else {
                throw new JsonException("Parse Error : " + readFrom.Input);
            }
        }
    
        /**
         * Creates a new {@code JSONArray} with values from the JSON string.
         *
         * @param json a JSON-encoded string containing an array.
         * @throws JSONException if the parse fails or doesn't yield a {@code
         *     JSONArray}.
         */
        public JsonArray(String json) : this(new JsonTokener(json)) {
        }
    
        /**
         * Returns the number of values in this array.
         */
        public int Length() {
            return values.Count;
        }
    
        /**
         * Appends {@code value} to the end of this array.
         *
         * @return this array.
         */
        public JsonArray Put(bool value) {
            values.Add(value);
            return this;
        }
    
        /**
         * Appends {@code value} to the end of this array.
         *
         * @param value a finite value. May not be {@link Double#isNaN() NaNs} or
         *     {@link Double#isInfinite() infinities}.
         * @return this array.
         */
        public JsonArray Put(double value) {
            values.Add(Json.CheckDouble(value));
            return this;
        }
    
        /**
         * Appends {@code value} to the end of this array.
         *
         * @return this array.
         */
        public JsonArray Put(int value) {
            values.Add(value);
            return this;
        }
    
        /**
         * Appends {@code value} to the end of this array.
         *
         * @return this array.
         */
        public JsonArray Put(long value) {
            values.Add(value);
            return this;
        }
    
        /**
         * Appends {@code value} to the end of this array.
         *
         * @param value a {@link JSONObject}, {@link JSONArray}, String, Boolean,
         *     Integer, Long, Double, {@link JSONObject#NULL}, or {@code null}. May
         *     not be {@link Double#isNaN() NaNs} or {@link Double#isInfinite()
         *     infinities}. Unsupported values are not permitted and will cause the
         *     array to be in an inconsistent state.
         * @return this array.
         */
        public JsonArray Put(object value) {
            values.Add(value);
            return this;
        }
    
        /**
         * Sets the value at {@code index} to {@code value}, null padding this array
         * to the required length if necessary. If a value already exists at {@code
         * index}, it will be replaced.
         *
         * @return this array.
         */
        public JsonArray Put(int index, bool value) {
            return Put(index, value);
        }
    
        /**
         * Sets the value at {@code index} to {@code value}, null padding this array
         * to the required length if necessary. If a value already exists at {@code
         * index}, it will be replaced.
         *
         * @param value a finite value. May not be {@link Double#isNaN() NaNs} or
         *     {@link Double#isInfinite() infinities}.
         * @return this array.
         */
        public JsonArray Put(int index, double value) {
            return Put(index, value);
        }
    
        /**
         * Sets the value at {@code index} to {@code value}, null padding this array
         * to the required length if necessary. If a value already exists at {@code
         * index}, it will be replaced.
         *
         * @return this array.
         */
        public JsonArray Put(int index, int value) {
            return Put(index, value);
        }
    
        /**
         * Sets the value at {@code index} to {@code value}, null padding this array
         * to the required length if necessary. If a value already exists at {@code
         * index}, it will be replaced.
         *
         * @return this array.
         */
        public JsonArray Put(int index, long value) {
            return Put(index, value);
        }
    
        /**
         * Sets the value at {@code index} to {@code value}, null padding this array
         * to the required length if necessary. If a value already exists at {@code
         * index}, it will be replaced.
         *
         * @param value a {@link JSONObject}, {@link JSONArray}, String, Boolean,
         *     Integer, Long, Double, {@link JSONObject#NULL}, or {@code null}. May
         *     not be {@link Double#isNaN() NaNs} or {@link Double#isInfinite()
         *     infinities}.
         * @return this array.
         */
        public JsonArray Put(int index, object value) {
            if (Json.IsNumber(value)) {
                // deviate from the original by checking all Numbers, not just floats & doubles
                Json.CheckDouble((double) value);
            }
            while (values.Count <= index) {
                values.Add(null);
            }
            values[index] = value;
            return this;
        }
    
        /**
         * Returns true if this array has no value at {@code index}, or if its value
         * is the {@code null} reference or {@link JSONObject#NULL}.
         */
        public bool IsNull(int index) {
            object value = Opt(index);
            return value == null || value == JsonObject.NULL;
        }
    
        /**
         * Returns the value at {@code index}.
         *
         * @throws JSONException if this array has no value at {@code index}, or if
         *     that value is the {@code null} reference. This method returns
         *     normally if the value is {@code JSONObject#NULL}.
         */
        public object Get(int index) {
            try {
                object value = values[index];
                if (value == null) {
                    throw new JsonException("Value at " + index + " is null.");
                }
                return value;
            } catch (IndexOutOfRangeException) {
                throw new JsonException("Index " + index + " out of range [0.." + values.Count + ")");
            }
        }
    
        /**
         * Returns the value at {@code index}, or null if the array has no value
         * at {@code index}.
         */
        public object Opt(int index) {
            if (index < 0 || index >= values.Count) {
                return null;
            }
            return values[index];
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a boolean or can
         * be coerced to a boolean.
         *
         * @throws JSONException if the value at {@code index} doesn't exist or
         *     cannot be coerced to a boolean.
         */
        public bool GetBoolean(int index) {
            object obj = Get(index);
            try {
                return Json.ToBoolean(obj);
            } catch {
                throw Json.TypeMismatch(index, obj, "boolean");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a boolean or can
         * be coerced to a boolean. Returns false otherwise.
         */
        public bool OptBoolean(int index) {
            return OptBoolean(index, false);
        }

        /**
         * Returns the value at {@code index} if it exists and is a boolean or can
         * be coerced to a boolean. Returns {@code fallback} otherwise.
         */
        public bool OptBoolean(int index, bool fallback) {
            object obj = Opt(index);
            try {
                return Json.ToBoolean(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a double or can
         * be coerced to a double.
         *
         * @throws JSONException if the value at {@code index} doesn't exist or
         *     cannot be coerced to a double.
         */
        public double GetDouble(int index) {
            object obj = Get(index);
            try {
                return Json.ToDouble(obj);
            } catch {
                throw Json.TypeMismatch(index, obj, "double");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a double or can
         * be coerced to a double. Returns {@code NaN} otherwise.
         */
        public double OptDouble(int index) {
            return OptDouble(index, Double.NaN);
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a double or can
         * be coerced to a double. Returns {@code fallback} otherwise.
         */
        public double OptDouble(int index, double fallback) {
            object obj = Opt(index);
            try {
                return Json.ToDouble(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is an int or
         * can be coerced to an int.
         *
         * @throws JSONException if the value at {@code index} doesn't exist or
         *     cannot be coerced to a int.
         */
        public int GetInt(int index) {
            object obj = Get(index);
            try {
                return Json.ToInteger(obj);
            } catch {
                throw Json.TypeMismatch(index, obj, "int");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is an int or
         * can be coerced to an int. Returns 0 otherwise.
         */
        public int OptInt(int index) {
            return OptInt(index, 0);
        }
    
        /**
         * Returns the value at {@code index} if it exists and is an int or
         * can be coerced to an int. Returns {@code fallback} otherwise.
         */
        public int OptInt(int index, int fallback) {
            object obj = Opt(index);
            try {
                return Json.ToInteger(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a long or
         * can be coerced to a long.
         *
         * @throws JSONException if the value at {@code index} doesn't exist or
         *     cannot be coerced to a long.
         */
        public long GetLong(int index) {
            object obj = Get(index);
            try {
                return Json.ToLong(obj);
            } catch {
                throw Json.TypeMismatch(index, obj, "long");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a long or
         * can be coerced to a long. Returns 0 otherwise.
         */
        public long OptLong(int index) {
            return OptLong(index, 0L);
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a long or
         * can be coerced to a long. Returns {@code fallback} otherwise.
         */
        public long OptLong(int index, long fallback) {
            object obj = Opt(index);
            try {
                return Json.ToLong(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists, coercing it if
         * necessary.
         *
         * @throws JSONException if no such value exists.
         */
        public string GetString(int index) {
            object obj = Get(index);
            string result = Json.ToString(obj);
            if (result == null) {
                throw Json.TypeMismatch(index, obj, "String");
            }
            return result;
        }
    
        /**
         * Returns the value at {@code index} if it exists, coercing it if
         * necessary. Returns the empty string if no such value exists.
         */
        public string OptString(int index) {
            return OptString(index, "");
        }
    
        /**
         * Returns the value at {@code index} if it exists, coercing it if
         * necessary. Returns {@code fallback} if no such value exists.
         */
        public string OptString(int index, string fallback) {
            object obj = Opt(index);
            String result = Json.ToString(obj);
            return result != null ? result : fallback;
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a {@code
         * JSONArray}.
         *
         * @throws JSONException if the value doesn't exist or is not a {@code
         *     JSONArray}.
         */
        public JsonArray GetJsonArray(int index) {
            object obj = Get(index);
            if (obj is JsonArray) {
                return (JsonArray) obj;
            } else {
                throw Json.TypeMismatch(index, obj, "JSONArray");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a {@code
         * JSONArray}. Returns null otherwise.
         */
        public JsonArray OptJSONArray(int index) {
            object obj = Opt(index);
            return obj is JsonArray ? (JsonArray) obj : null;
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a {@code
         * JSONObject}.
         *
         * @throws JSONException if the value doesn't exist or is not a {@code
         *     JSONObject}.
         */
        public JsonObject GetJsonObject(int index) {
            object obj = Get(index);
            if (obj is JsonObject) {
                return (JsonObject) obj;
            } else {
                throw Json.TypeMismatch(index, obj, "JSONObject");
            }
        }
    
        /**
         * Returns the value at {@code index} if it exists and is a {@code
         * JSONObject}. Returns null otherwise.
         */
        public JsonObject OptJSONObject(int index) {
            object obj = Opt(index);
            return obj is JsonObject ? (JsonObject) obj : null;
        }
    
        /**
         * Returns a new object whose values are the values in this array, and whose
         * names are the values in {@code names}. Names and values are paired up by
         * index from 0 through to the shorter array's length. Names that are not
         * strings will be coerced to strings. This method returns null if either
         * array is empty.
         */
        public JsonObject ToJSONObject(JsonArray names) {
            JsonObject result = new JsonObject();
            int length = Math.Min(names.Length(), values.Count);
            if (length == 0) {
                return null;
            }
            for (int i = 0; i < length; i++) {
                String name = Json.ToString(names.Opt(i));
                result.Put(name, Opt(i));
            }
            return result;
        }
    
        /**
         * Returns a new string by alternating this array's values with {@code
         * separator}. This array's string values are quoted and have their special
         * characters escaped. For example, the array containing the strings '12"
         * pizza', 'taco' and 'soda' joined on '+' returns this:
         * <pre>"12\" pizza"+"taco"+"soda"</pre>
         */
        public string Join(string separator) {
            JsonStringer stringer = new JsonStringer();
            stringer.Open(JsonStringer.Scope.NULL, "");
            for (int i = 0, size = values.Count; i < size; i++) {
                if (i > 0) {
                    stringer.output.Append(separator);
                }
                stringer.Value(values[i]);
            }
            stringer.Close(JsonStringer.Scope.NULL, JsonStringer.Scope.NULL, "");
            return stringer.output.ToString();
        }
    
        /**
         * Encodes this array as a compact JSON string, such as:
         * <pre>[94043,90210]</pre>
         */
        public override string ToString() {
            try {
                JsonStringer stringer = new JsonStringer();
                WriteTo(stringer);
                return stringer.ToString();
            } catch (JsonException) {
                return null;
            }
        }
    
        /**
         * Encodes this array as a human readable JSON string for debugging, such
         * as:
         * <pre>
         * [
         *     94043,
         *     90210
         * ]</pre>
         *
         * @param indentSpaces the number of spaces to indent for each level of
         *     nesting.
         */
        public string ToString(int indentSpaces) {
            JsonStringer stringer = new JsonStringer(indentSpaces);
            WriteTo(stringer);
            return stringer.ToString();
        }
    
        internal void WriteTo(JsonStringer stringer) {
            stringer.Array();
            foreach (object value in values) {
                stringer.Value(value);
            }
            stringer.EndArray();
        }

        public override bool Equals(object o) {
            return o is JsonArray && ((JsonArray) o).values == values;
        }

        public override int GetHashCode() {
            // diverge from the original, which doesn't implement hashCode
            return values.GetHashCode();
        }
    }
}

