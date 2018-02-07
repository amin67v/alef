using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Color : IEquatable<Color>
    {
        public static readonly Color AliceBlue = FromRgb(0xF0F8FF);
        public static readonly Color AntiqueWhite = FromRgb(0xFAEBD7);
        public static readonly Color Aqua = FromRgb(0x00FFFF);
        public static readonly Color Aquamarine = FromRgb(0x7FFFD4);
        public static readonly Color Azure = FromRgb(0xF0FFFF);
        public static readonly Color Beige = FromRgb(0xF5F5DC);
        public static readonly Color Bisque = FromRgb(0xFFE4C4);
        public static readonly Color Black = FromRgb(0x000000);
        public static readonly Color BlanchedAlmond = FromRgb(0xFFEBCD);
        public static readonly Color Blue = FromRgb(0x0000FF);
        public static readonly Color BlueViolet = FromRgb(0x8A2BE2);
        public static readonly Color Brown = FromRgb(0xA52A2A);
        public static readonly Color BurlyWood = FromRgb(0xDEB887);
        public static readonly Color CadetBlue = FromRgb(0x5F9EA0);
        public static readonly Color Chartreuse = FromRgb(0x7FFF00);
        public static readonly Color Chocolate = FromRgb(0xD2691E);
        public static readonly Color Coral = FromRgb(0xFF7F50);
        public static readonly Color CornflowerBlue = FromRgb(0x6495ED);
        public static readonly Color Cornsilk = FromRgb(0xFFF8DC);
        public static readonly Color Crimson = FromRgb(0xDC143C);
        public static readonly Color Cyan = FromRgb(0x00FFFF);
        public static readonly Color DarkBlue = FromRgb(0x00008B);
        public static readonly Color DarkCyan = FromRgb(0x008B8B);
        public static readonly Color DarkGoldenRod = FromRgb(0xB8860B);
        public static readonly Color DarkGray = FromRgb(0xA9A9A9);
        public static readonly Color DarkGreen = FromRgb(0x006400);
        public static readonly Color DarkGrey = FromRgb(0xA9A9A9);
        public static readonly Color DarkKhaki = FromRgb(0xBDB76B);
        public static readonly Color DarkMagenta = FromRgb(0x8B008B);
        public static readonly Color DarkOliveGreen = FromRgb(0x556B2F);
        public static readonly Color DarkOrange = FromRgb(0xFF8C00);
        public static readonly Color DarkOrchid = FromRgb(0x9932CC);
        public static readonly Color DarkRed = FromRgb(0x8B0000);
        public static readonly Color DarkSalmon = FromRgb(0xE9967A);
        public static readonly Color DarkSeaGreen = FromRgb(0x8FBC8F);
        public static readonly Color DarkSlateBlue = FromRgb(0x483D8B);
        public static readonly Color DarkSlateGray = FromRgb(0x2F4F4F);
        public static readonly Color DarkSlateGrey = FromRgb(0x2F4F4F);
        public static readonly Color DarkTurquoise = FromRgb(0x00CED1);
        public static readonly Color DarkViolet = FromRgb(0x9400D3);
        public static readonly Color DeepPink = FromRgb(0xFF1493);
        public static readonly Color DeepSkyBlue = FromRgb(0x00BFFF);
        public static readonly Color DimGray = FromRgb(0x696969);
        public static readonly Color DimGrey = FromRgb(0x696969);
        public static readonly Color DodgerBlue = FromRgb(0x1E90FF);
        public static readonly Color FireBrick = FromRgb(0xB22222);
        public static readonly Color FloralWhite = FromRgb(0xFFFAF0);
        public static readonly Color ForestGreen = FromRgb(0x228B22);
        public static readonly Color Fuchsia = FromRgb(0xFF00FF);
        public static readonly Color Gainsboro = FromRgb(0xDCDCDC);
        public static readonly Color GhostWhite = FromRgb(0xF8F8FF);
        public static readonly Color Gold = FromRgb(0xFFD700);
        public static readonly Color GoldenRod = FromRgb(0xDAA520);
        public static readonly Color Gray = FromRgb(0x808080);
        public static readonly Color Green = FromRgb(0x008000);
        public static readonly Color GreenYellow = FromRgb(0xADFF2F);
        public static readonly Color Grey = FromRgb(0x808080);
        public static readonly Color HoneyDew = FromRgb(0xF0FFF0);
        public static readonly Color HotPink = FromRgb(0xFF69B4);
        public static readonly Color IndianRed = FromRgb(0xCD5C5C);
        public static readonly Color Indigo = FromRgb(0x4B0082);
        public static readonly Color Ivory = FromRgb(0xFFFFF0);
        public static readonly Color Khaki = FromRgb(0xF0E68C);
        public static readonly Color Lavender = FromRgb(0xE6E6FA);
        public static readonly Color LavenderBlush = FromRgb(0xFFF0F5);
        public static readonly Color LawnGreen = FromRgb(0x7CFC00);
        public static readonly Color LemonChiffon = FromRgb(0xFFFACD);
        public static readonly Color LightBlue = FromRgb(0xADD8E6);
        public static readonly Color LightCoral = FromRgb(0xF08080);
        public static readonly Color LightCyan = FromRgb(0xE0FFFF);
        public static readonly Color LightGoldenRodYellow = FromRgb(0xFAFAD2);
        public static readonly Color LightGray = FromRgb(0xD3D3D3);
        public static readonly Color LightGreen = FromRgb(0x90EE90);
        public static readonly Color LightGrey = FromRgb(0xD3D3D3);
        public static readonly Color LightPink = FromRgb(0xFFB6C1);
        public static readonly Color LightSalmon = FromRgb(0xFFA07A);
        public static readonly Color LightSeaGreen = FromRgb(0x20B2AA);
        public static readonly Color LightSkyBlue = FromRgb(0x87CEFA);
        public static readonly Color LightSlateGray = FromRgb(0x778899);
        public static readonly Color LightSlateGrey = FromRgb(0x778899);
        public static readonly Color LightSteelBlue = FromRgb(0xB0C4DE);
        public static readonly Color LightYellow = FromRgb(0xFFFFE0);
        public static readonly Color Lime = FromRgb(0x00FF00);
        public static readonly Color LimeGreen = FromRgb(0x32CD32);
        public static readonly Color Linen = FromRgb(0xFAF0E6);
        public static readonly Color Magenta = FromRgb(0xFF00FF);
        public static readonly Color Maroon = FromRgb(0x800000);
        public static readonly Color MediumAquaMarine = FromRgb(0x66CDAA);
        public static readonly Color MediumBlue = FromRgb(0x0000CD);
        public static readonly Color MediumOrchid = FromRgb(0xBA55D3);
        public static readonly Color MediumPurple = FromRgb(0x9370DB);
        public static readonly Color MediumSeaGreen = FromRgb(0x3CB371);
        public static readonly Color MediumSlateBlue = FromRgb(0x7B68EE);
        public static readonly Color MediumSpringGreen = FromRgb(0x00FA9A);
        public static readonly Color MediumTurquoise = FromRgb(0x48D1CC);
        public static readonly Color MediumVioletRed = FromRgb(0xC71585);
        public static readonly Color MidnightBlue = FromRgb(0x191970);
        public static readonly Color MintCream = FromRgb(0xF5FFFA);
        public static readonly Color MistyRose = FromRgb(0xFFE4E1);
        public static readonly Color Moccasin = FromRgb(0xFFE4B5);
        public static readonly Color NavajoWhite = FromRgb(0xFFDEAD);
        public static readonly Color Navy = FromRgb(0x000080);
        public static readonly Color OldLace = FromRgb(0xFDF5E6);
        public static readonly Color Olive = FromRgb(0x808000);
        public static readonly Color OliveDrab = FromRgb(0x6B8E23);
        public static readonly Color Orange = FromRgb(0xFFA500);
        public static readonly Color OrangeRed = FromRgb(0xFF4500);
        public static readonly Color Orchid = FromRgb(0xDA70D6);
        public static readonly Color PaleGoldenRod = FromRgb(0xEEE8AA);
        public static readonly Color PaleGreen = FromRgb(0x98FB98);
        public static readonly Color PaleTurquoise = FromRgb(0xAFEEEE);
        public static readonly Color PaleVioletRed = FromRgb(0xDB7093);
        public static readonly Color PapayaWhip = FromRgb(0xFFEFD5);
        public static readonly Color PeachPuff = FromRgb(0xFFDAB9);
        public static readonly Color Peru = FromRgb(0xCD853F);
        public static readonly Color Pink = FromRgb(0xFFC0CB);
        public static readonly Color Plum = FromRgb(0xDDA0DD);
        public static readonly Color PowderBlue = FromRgb(0xB0E0E6);
        public static readonly Color Purple = FromRgb(0x800080);
        public static readonly Color RebeccaPurple = FromRgb(0x663399);
        public static readonly Color Red = FromRgb(0xFF0000);
        public static readonly Color RosyBrown = FromRgb(0xBC8F8F);
        public static readonly Color RoyalBlue = FromRgb(0x4169E1);
        public static readonly Color SaddleBrown = FromRgb(0x8B4513);
        public static readonly Color Salmon = FromRgb(0xFA8072);
        public static readonly Color SandyBrown = FromRgb(0xF4A460);
        public static readonly Color SeaGreen = FromRgb(0x2E8B57);
        public static readonly Color SeaShell = FromRgb(0xFFF5EE);
        public static readonly Color Sienna = FromRgb(0xA0522D);
        public static readonly Color Silver = FromRgb(0xC0C0C0);
        public static readonly Color SkyBlue = FromRgb(0x87CEEB);
        public static readonly Color SlateBlue = FromRgb(0x6A5ACD);
        public static readonly Color SlateGray = FromRgb(0x708090);
        public static readonly Color SlateGrey = FromRgb(0x708090);
        public static readonly Color Snow = FromRgb(0xFFFAFA);
        public static readonly Color SpringGreen = FromRgb(0x00FF7F);
        public static readonly Color SteelBlue = FromRgb(0x4682B4);
        public static readonly Color Tan = FromRgb(0xD2B48C);
        public static readonly Color Teal = FromRgb(0x008080);
        public static readonly Color Thistle = FromRgb(0xD8BFD8);
        public static readonly Color Tomato = FromRgb(0xFF6347);
        public static readonly Color Turquoise = FromRgb(0x40E0D0);
        public static readonly Color Violet = FromRgb(0xEE82EE);
        public static readonly Color Wheat = FromRgb(0xF5DEB3);
        public static readonly Color White = FromRgb(0xFFFFFF);
        public static readonly Color WhiteSmoke = FromRgb(0xF5F5F5);
        public static readonly Color Yellow = FromRgb(0xFFFF00);
        public static readonly Color YellowGreen = FromRgb(0x9ACD32);
        public static readonly Color Transparent = new Color(255, 255, 255, 0);

        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;

        [FieldOffset(0)]
        int m_value; // used for easy hash and compare

        public Color(byte r, byte g, byte b, byte a = 255) : this()
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int r, int g, int b, int a = 255) : this()
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
            A = (byte)a;
        }

        public Color(float r, float g, float b, float a = 1) : this()
        {
            R = (byte)(r * 255);
            G = (byte)(g * 255);
            B = (byte)(b * 255);
            A = (byte)(a * 255);
        }

        public Color(Vector4 vec) : this(vec.X, vec.Y, vec.Z, vec.W) { }

        public float Brightness => 0.00082914f * R + 0.00278928f * G + 0.00028158f * B;

        /// <summary>
        /// Creates color from Rgb
        /// </summary>
        public static Color FromRgb(int rgb)
        {
            return new Color((byte)((rgb & 0x00ff0000u) >> 16),
                             (byte)((rgb & 0x0000ff00u) >> 8),
                             (byte)((rgb & 0x000000ffu) >> 0));
        }

        /// <summary>
        /// Creates color from Argb
        /// </summary>
        public static Color FromArgb(int rgba)
        {
            return new Color((byte)((rgba & 0x00ff0000u) >> 16),
                             (byte)((rgba & 0x0000ff00u) >> 8),
                             (byte)((rgba & 0x000000ffu) >> 0),
                             (byte)((rgba & 0xff000000u) >> 24));
        }

        /// <summary>
        /// Blends color a with color b with the given amount 
        /// </summary>
        public static Color Blend(Color a, Color b, float amount)
        {
            return new Color((byte)((b.R - a.R) * amount + a.R),
                             (byte)((b.G - a.G) * amount + a.G),
                             (byte)((b.B - a.B) * amount + a.B),
                             (byte)((b.A - a.A) * amount + a.A));
        }

        /// <summary>
        /// Creates color from Hsv color space
        /// </summary>
        public static Color FromHsv(float h, float s, float v, float a)
        {
            int i;
            float f, p, q, t;
            if (s == 0)
                return new Color(v, v, v, 1f);

            h /= 60;
            i = (int)h;
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    return new Color(v, t, p, 1f);
                case 1:
                    return new Color(q, v, p, 1f);
                case 2:
                    return new Color(p, v, t, 1f);
                case 3:
                    return new Color(p, q, v, 1f);
                case 4:
                    return new Color(t, p, v, 1f);
                default:
                    return new Color(v, p, q, 1f);
            }
        }

        /// <summary>
        /// Converts this color to Hsv color space
        /// </summary>
        /// <returns></returns>
        public Vector4 ToHsv()
        {
            float h, s, v;
            float min, max, delta;
            float rf = R / 255f;
            float gf = G / 255f;
            float bf = B / 255f;
            min = Math.Min(rf / 255f, Math.Min(gf, bf));
            max = Math.Max(rf / 255f, Math.Min(gf, bf));
            v = max;
            delta = max - min;
            if (max != 0)
                s = delta / max;
            else
            {
                s = 0;
                h = 0;
                return new Vector4(h, s, v, A);
            }

            if (rf == max)
                h = (gf - bf) / delta;
            else if (gf == max)
                h = 2 + (bf - rf) / delta;
            else
                h = 4 + (rf - gf) / delta;

            h *= 60;
            if (h < 0)
                h += 360;

            return new Vector4(h, s, v, A);
        }

        /// <summary>
        /// Converts this color to Argb int representation
        /// </summary>
        /// <returns></returns>
        public int ToArgb()
        {
            return (R << 16) | (G << 8) | (B << 0) | (A << 24);
        }

        /// <summary>
        /// Converts this color to Rgba int representation
        /// </summary>
        /// <returns></returns>
        public int ToRgba()
        {
            return (R << 24) | (G << 16) | (B << 8) | (A << 0);
        }

        /// <summary>
        /// Converts this color to Rgb int representation
        /// </summary>
        /// <returns></returns>
        public int ToRgb()
        {
            return (R << 16) | (G << 8) | B;
        }

        /// <summary>
        /// Darker version of this color, value must be in range 0 - 1
        /// </summary>
        public Color Darker(float value)
        {
            value = 1 - value;
            return new Color((byte)(R * value), (byte)(G * value), (byte)(B * value), A);
        }

        /// <summary>
        /// Lighter version of this color, value must be in range 0 - 1
        /// </summary>
        public Color Lighter(float value)
        {
            return new Color((byte)((255 - R) * value + R), (byte)((255 - G) * value + G), (byte)((255 - B) * value + B), A);
        }

        public override string ToString()
        {
            return $"(R:{this.R} - G:{this.G} - B:{this.B} - A:{this.A})";
        }

        public Vector4 ToVector4()
        {
            const float inv255 = 0.003921568627451f;
            return new Vector4(R * inv255, G * inv255, B * inv255, A * inv255);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
                return Equals((Color)obj);
            else
                return false;
        }

        public bool Equals(Color other) => m_value == other.m_value;

        public override int GetHashCode() => m_value;

        public static bool operator ==(Color a, Color b) => a.Equals(b);

        public static bool operator !=(Color a, Color b) => !a.Equals(b);
    }
}