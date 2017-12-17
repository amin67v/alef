using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Font : Resource
    {
        Glyph[] glyph_map = new Glyph[96]; // ascii chars from range 32 to 128
        Texture texture = null;
        float space = 0;
        float height = 0;

        Font() { }

        public Glyph this[char c]
        {
            get
            {
                uint index = (uint)(c - 32);
                if (index >= glyph_map.Length)
                    return null;
                else
                    return glyph_map[index];
            }
        }

        public Texture Texture => texture;

        public float SpaceWidth => space;

        public float Height => height;

        public static Font Create(Glyph[] glyphs, Texture tex, float space, float height)
        {
            var font = new Font();

            for (int i = 0; i < glyphs.Length; i++)
                font.glyph_map[glyphs[i].Code - 32] = glyphs[i];

            font.texture = tex;
            font.space = space;
            font.height = height;

            return font;
        }

        /// <summary>
        /// Loads and cache font from file
        /// </summary>
        public static Font Load(string file)
        {
            Resource load_file(Stream stream)
            {
                TextReader reader = new StreamReader(stream);
                try
                {
                    var src = reader.ReadToEnd();
                    var json = JsonValue.Parse(src);
                    var tex_name = json["texture"];
                    JsonArray arr = json["glyphs"] as JsonArray;
                    var glyphs = new Glyph[arr.Count];
                    for (int i = 0; i < arr.Count; i++)
                    {
                        var rect = JsonHelper.ParseRect(arr[i]["rect"]);
                        var offset = JsonHelper.ParseVector2(arr[i]["offset"]);
                        glyphs[i] = new Glyph(rect, offset, arr[i]["width"], arr[i]["code"]);

                    }
                    var tex_file = Path.Combine(Path.GetDirectoryName(file), tex_name);
                    var tex = Texture.Load(tex_file, FilterMode.Bilinear, false);
                    var font = Font.Create(glyphs, tex, json["space"], json["height"]);
                    return font;
                }
                finally
                {
                    reader.Dispose();
                }

            }

            return App.ResourceManager.FromCacheOrFile(file, load_file) as Font;
        }

        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();
            texture?.Dispose();
        }
    }

    public class Glyph
    {
        public Rect Rect { get; private set; }
        public Vector2 Offset { get; private set; }
        public float Width { get; private set; }
        public char Code { get; private set; }

        public Glyph(Rect rect, Vector2 offset, float width, char code)
        {
            this.Rect = rect;
            this.Offset = offset;
            this.Width = width;
            this.Code = code;
        }

    }
}
