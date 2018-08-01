using System;
using System.Text;
using System.Json;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public static class Hash
    {
        /// <summary>
        /// Generates hash code for the given string
        /// </summary>
        public static int FromString(string str)
        {
            unchecked
            {
                const int prime = 486187739;
                int hash = prime;
                int len = str.Length;
                for (int i = 0; i < len; i++)
                    hash = (hash + str[i]) * prime;

                return hash;
            }
        }

        /// <summary>
        /// Generates hash code for the given byte array
        /// </summary>
        public static int FromByteArray(byte[] bytes)
        {
            unchecked
            {
                const int prime = 486187739;
                int hash = prime;
                int len = bytes.Length;
                for (int i = 0; i < len; i++)
                    hash = (hash + bytes[i]) * prime;

                return hash;
            }
        }
    }
}