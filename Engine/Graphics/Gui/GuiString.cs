using System;
using System.Text;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace Engine
{
    public class GuiString : IEquatable<GuiString>
    {
        byte[] buffer;
        int len;

        public GuiString(int capacity) 
        { 
            buffer = new byte[capacity];
            len = 0;
        }

        public byte[] Buffer => buffer;

        public int Length => len;

        public string String
        {
            get => Encoding.UTF8.GetString(buffer, 0, len);
            set
            {
                if (value.Length > len)
                    buffer = new byte[value.Length];

                len = value.Length;
                Encoding.UTF8.GetBytes(value, 0, len, buffer, 0);
            }
        }

        public override string ToString() => String;

        public override int GetHashCode() => buffer.GetFastHash();

        public override bool Equals(object obj)
        {
            var other = obj as GuiString;
            if (other == null)
                return false;
            else
                return Equals(other);
        }

        public bool Equals(GuiString other)
        {
            if (len != other.len)
                return false;

            for (int i = 0; i < len; i++)
            {
                if (buffer[i] != other.buffer[i])
                    return false;
            }

            return true;
        }
    }
}