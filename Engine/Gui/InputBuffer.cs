using System;
using System.Text;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace Engine
{
    public class InputBuffer : IEquatable<InputBuffer>
    {
        byte[] buffer;

        public InputBuffer(int capacity)
        {
            buffer = new byte[capacity];
        }

        public byte[] Buffer => buffer;

        public int Count
        {
            get
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 0)
                        return i;
                }

                return buffer.Length;
            }
        }

        public string String
        {
            get => Encoding.UTF8.GetString(buffer, 0, Count);
            set
            {
                int num = Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);

                if (num < buffer.Length)
                    buffer[num] = 0;
            }
        }

        public override string ToString() => String;

        public override int GetHashCode() => buffer.GetFastHash();

        public override bool Equals(object obj)
        {
            var other = obj as InputBuffer;
            if (other == null)
                return false;
            else
                return Equals(other);
        }

        public bool Equals(InputBuffer other)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != other.buffer[i])
                    return false;
                else if (buffer[i] == 0 && other.buffer[i] == 0)
                    return true;
            }

            return true;
        }
    }
}