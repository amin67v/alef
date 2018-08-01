using System;
using System.Json;
using System.Text;
using System.Numerics;

namespace Engine
{
    public static class JsonHelper
    {
        /// <summary>
        /// Returns a pretty version of the input json!
        /// </summary>
        public static string Prettify(string input)
        {
            const string indent = "    ";
            
            bool isEscaped(StringBuilder sb, int index)
            {
                bool escaped = false;
                while (index > 0 && sb[--index] == '\\') escaped = !escaped;
                return escaped;
            }

            void appendIndent(StringBuilder sb, int count)
            {
                for (; count > 0; --count) sb.Append(indent);
            }

            var output = new StringBuilder(input.Length * 2);
            char? quote = null;
            int depth = 0;

            for (int i = 0; i < input.Length; ++i)
            {
                char ch = input[i];

                switch (ch)
                {
                    case '{':
                    case '[':
                        output.AppendLine();
                        appendIndent(output, depth);
                        output.Append(ch);
                        if (!quote.HasValue)
                        {
                            output.AppendLine();
                            appendIndent(output, ++depth);
                        }
                        break;
                    case '}':
                    case ']':
                        if (quote.HasValue)
                            output.Append(ch);
                        else
                        {
                            output.AppendLine();
                            appendIndent(output, --depth);
                            output.Append(ch);
                        }
                        break;
                    case '"':
                    case '\'':
                        output.Append(ch);
                        if (quote.HasValue)
                        {
                            if (!isEscaped(output, i))
                                quote = null;
                        }
                        else quote = ch;
                        break;
                    case ',':
                        output.Append(ch);
                        if (!quote.HasValue)
                        {
                            output.AppendLine();
                            appendIndent(output, depth);
                        }
                        break;
                    case ':':
                        if (quote.HasValue)
                        {
                            output.Append(ch);
                        }
                        else
                        {
                            output.Append(" : ");
                        }
                        break;
                    default:
                        if (quote.HasValue || !char.IsWhiteSpace(ch))
                            output.Append(ch);
                        break;
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Converts the string representation of a vector2 to its equivalent.
        /// </summary>
        public static Vector2 ParseVector2(string str)
        {
            try
            {
                string[] split = str.Split(' ');
                return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
            }
            catch
            {
                throw new FormatException("str does not represent a vector2 in a valid format.");
            }
        }

        /// <summary>
        /// Converts the string representation of a vector3 to its equivalent.
        /// </summary>
        public static Vector3 ParseVector3(string str)
        {
            try
            {
                string[] split = str.Split(' ');
                return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            }
            catch
            {
                throw new FormatException("str does not represent a vector3 in a valid format.");
            }
        }

        /// <summary>
        /// Converts the string representation of a vector4 to its equivalent.
        /// </summary>
        public static Vector4 ParseVector4(string str)
        {
            try
            {
                string[] split = str.Split(' ');
                return new Vector4(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            catch
            {
                throw new FormatException("str does not represent a vector4 in a valid format.");
            }
        }

        /// <summary>
        /// Converts the string representation of a rect to its equivalent.
        /// </summary>
        public static Rect ParseRect(string str)
        {
            try
            {
                string[] split = str.Split(' ');
                return new Rect(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            catch
            {
                throw new FormatException("str does not represent a rect in a valid format.");
            }
        }
    }
}