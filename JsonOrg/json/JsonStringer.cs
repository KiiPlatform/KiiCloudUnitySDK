using System;
using System.Text;
using System.Collections.Generic;

namespace JsonOrg
{
    internal class JsonStringer {

    /** The output data, containing at most one top-level array or object. */
    internal StringBuilder output = new StringBuilder();

    /**
     * Lexical scoping elements within this stringer, necessary to insert the
     * appropriate separator characters (ie. commas and colons) and to detect
     * nesting errors.
     */
    internal enum Scope {

        /**
         * An array with no elements requires no separators or newlines before
         * it is closed.
         */
        EMPTY_ARRAY,

        /**
         * A array with at least one value requires a comma and newline before
         * the next element.
         */
        NONEMPTY_ARRAY,

        /**
         * An object with no keys or values requires no separators or newlines
         * before it is closed.
         */
        EMPTY_OBJECT,

        /**
         * An object whose most recent element is a key. The next element must
         * be a value.
         */
        DANGLING_KEY,

        /**
         * An object with at least one name/value pair requires a comma and
         * newline before the next element.
         */
        NONEMPTY_OBJECT,

        /**
         * A special bracketless array needed by JSONStringer.join() and
         * JSONObject.quote() only. Not used for JSON encoding.
         */
        NULL,
    }

    /**
     * Unlike the original implementation, this stack isn't limited to 20
     * levels of nesting.
     */
    private List<Scope> stack = new List<Scope>();

    /**
     * A string containing a full set of spaces for a single level of
     * indentation, or null for no pretty printing.
     */
    private string indent;

    public JsonStringer() {
        indent = null;
    }

    internal JsonStringer(int indentSpaces) {
        char[] indentChars = new char[indentSpaces];
        for (int i = 0 ; i < indentChars.Length ; ++i)
        {
            indentChars[i] = ' ';
        }
        indent = new String(indentChars);
    }

    /**
     * Begins encoding a new array. Each call to this method must be paired with
     * a call to {@link #endArray}.
     *
     * @return this stringer.
     */
    public JsonStringer Array() {
        return Open(Scope.EMPTY_ARRAY, "[");
    }

    /**
     * Ends encoding the current array.
     *
     * @return this stringer.
     */
    public JsonStringer EndArray() {
        return Close(Scope.EMPTY_ARRAY, Scope.NONEMPTY_ARRAY, "]");
    }

    /**
     * Begins encoding a new object. Each call to this method must be paired
     * with a call to {@link #endObject}.
     *
     * @return this stringer.
     */
    public JsonStringer Obj() {
        return Open(Scope.EMPTY_OBJECT, "{");
    }

    /**
     * Ends encoding the current object.
     *
     * @return this stringer.
     */
    public JsonStringer EndObject() {
        return Close(Scope.EMPTY_OBJECT, Scope.NONEMPTY_OBJECT, "}");
    }

    /**
     * Enters a new scope by appending any necessary whitespace and the given
     * bracket.
     */
    internal JsonStringer Open(Scope empty, String openBracket) {
        if (stack.Count == 0 && output.Length > 0) {
            throw new JsonException("Nesting problem: multiple top-level roots");
        }
        BeforeValue();
        stack.Add(empty);
        output.Append(openBracket);
        return this;
    }

    /**
     * Closes the current scope by appending any necessary whitespace and the
     * given bracket.
     */
    internal JsonStringer Close(Scope empty, Scope nonempty, String closeBracket) {
        Scope context = Peek();
        if (context != nonempty && context != empty) {
            throw new JsonException("Nesting problem");
        }

        stack.RemoveAt(stack.Count - 1);
        if (context == nonempty) {
            Newline();
        }
        output.Append(closeBracket);
        return this;
    }

    /**
     * Returns the value on the top of the stack.
     */
    private Scope Peek() {
        if (stack.Count == 0) {
            throw new JsonException("Nesting problem");
        }
        return stack[stack.Count - 1];
    }

    /**
     * Replace the value on the top of the stack with the given value.
     */
    private void ReplaceTop(Scope topOfStack) {
        stack[stack.Count - 1] = topOfStack;
    }

    /**
     * Encodes {@code value}.
     *
     * @param value a {@link JSONObject}, {@link JSONArray}, String, Boolean,
     *     Integer, Long, Double or null. May not be {@link Double#isNaN() NaNs}
     *     or {@link Double#isInfinite() infinities}.
     * @return this stringer.
     */
    public JsonStringer Value(Object value) {
        if (stack.Count == 0) {
            throw new JsonException("Nesting problem");
        }

        if (value is JsonArray) {
            ((JsonArray) value).WriteTo(this);
            return this;

        } else if (value is JsonObject) {
            ((JsonObject) value).WriteTo(this);
            return this;
        }

        BeforeValue();

        if (value == null
                || value == JsonObject.NULL) {
            output.Append(value);
        } else if (value is bool) {
            output.Append(value.ToString().ToLower());

        } else if (Json.IsNumber(value)) {
            output.Append(JsonObject.NumberToString(value));

        } else {
            Str(value.ToString());
        }

        return this;
    }

    /**
     * Encodes {@code value} to this stringer.
     *
     * @return this stringer.
     */
    public JsonStringer Value(bool value) {
        if (stack.Count == 0) {
            throw new JsonException("Nesting problem");
        }
        BeforeValue();
        output.Append(value);
        return this;
    }

    /**
     * Encodes {@code value} to this stringer.
     *
     * @param value a finite value. May not be {@link Double#isNaN() NaNs} or
     *     {@link Double#isInfinite() infinities}.
     * @return this stringer.
     */
    public JsonStringer Value(double value) {
        if (stack.Count == 0) {
            throw new JsonException("Nesting problem");
        }
        BeforeValue();
        output.Append(JsonObject.NumberToString(value));
        return this;
    }

    /**
     * Encodes {@code value} to this stringer.
     *
     * @return this stringer.
     */
    public JsonStringer Value(long value) {
        if (stack.Count == 0) {
            throw new JsonException("Nesting problem");
        }
        BeforeValue();
        output.Append(value);
        return this;
    }

    private void Str(string value) {
        output.Append("\"");
        for (int i = 0, length = value.Length ; i < length; i++) {
            char c = value[i];

            /*
             * From RFC 4627, "All Unicode characters may be placed within the
             * quotation marks except for the characters that must be escaped:
             * quotation mark, reverse solidus, and the control characters
             * (U+0000 through U+001F)."
             */
            switch (c) {
                case '"':
                case '\\':
                case '/':
                    output.Append('\\').Append(c);
                    break;

                case '\t':
                    output.Append("\\t");
                    break;

                case '\b':
                    output.Append("\\b");
                    break;

                case '\n':
                    output.Append("\\n");
                    break;

                case '\r':
                    output.Append("\\r");
                    break;

                case '\f':
                    output.Append("\\f");
                    break;

                default:
                    if (c <= 0x1F) {
                        output.Append(string.Format("\\u{0:0000X}", (int) c));
                    } else {
                        output.Append(c);
                    }
                    break;
            }

        }
        output.Append("\"");
    }

    private void Newline() {
        if (indent == null) {
            return;
        }

        output.Append("\n");
        for (int i = 0; i < stack.Count; i++) {
            output.Append(indent);
        }
    }

    /**
     * Encodes the key (property name) to this stringer.
     *
     * @param name the name of the forthcoming value. May not be null.
     * @return this stringer.
     */
    public JsonStringer Key(string name) {
        if (name == null) {
            throw new JsonException("Names must be non-null");
        }
        BeforeKey();
        Str(name);
        return this;
    }

    /**
     * Inserts any necessary separators and whitespace before a name. Also
     * adjusts the stack to expect the Key's value.
     */
    private void BeforeKey() {
        Scope context = Peek();
        if (context == Scope.NONEMPTY_OBJECT) { // first in object
            output.Append(',');
        } else if (context != Scope.EMPTY_OBJECT) { // not in an object!
            throw new JsonException("Nesting problem");
        }
        Newline();
        ReplaceTop(Scope.DANGLING_KEY);
    }

    /**
     * Inserts any necessary separators and whitespace before a literal value,
     * inline array, or inline object. Also adjusts the stack to expect either a
     * closing bracket or another element.
     */
    private void BeforeValue() {
        if (stack.Count == 0) {
            return;
        }

        Scope context = Peek();
        if (context == Scope.EMPTY_ARRAY) { // first in array
            ReplaceTop(Scope.NONEMPTY_ARRAY);
            Newline();
        } else if (context == Scope.NONEMPTY_ARRAY) { // another in array
            output.Append(',');
            Newline();
        } else if (context == Scope.DANGLING_KEY) { // value for key
            output.Append(indent == null ? ":" : ": ");
            ReplaceTop(Scope.NONEMPTY_OBJECT);
        } else if (context != Scope.NULL) {
            throw new JsonException("Nesting problem");
        }
    }

    /**
     * Returns the encoded JSON string.
     *
     * <p>If invoked with unterminated arrays or unclosed objects, this method's
     * return value is undefined.
     *
     * <p><strong>Warning:</strong> although it contradicts the general contract
     * of {@link Object#toString}, this method returns null if the stringer
     * contains no data.
     */
    public override string ToString() {
        return output.Length == 0 ? null : output.ToString();
    }
}
}

