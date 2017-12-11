using System;
using System.IO;
using System.Json;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteSheet : Resource
    {
        int pixel_per_unit = 24;
        Texture tex;
        Frame[] frames;

        SpriteSheet() { }

        public Frame this[int index] => frames[index];

        public Texture Texture => tex;

        public int PixelPerUnit => pixel_per_unit;

        public static SpriteSheet Create(Frame[] frames, Texture tex, int pixel_per_unit)
        {
            var sheet = new SpriteSheet();
            sheet.tex = tex;
            sheet.pixel_per_unit = pixel_per_unit;
            sheet.frames = frames;

            return sheet;
        }

        /// <summary>
        /// Load and cache sprite sheet and related texture from file
        /// </summary>
        public static SpriteSheet Load(string file)
        {
            Resource load_file(Stream stream)
            {
                TextReader reader = new StreamReader(stream);
                try
                {
                    var src = reader.ReadToEnd();
                    var json = JsonValue.Parse(src);
                    string tex_name = json["texture"];
                    int pixel_per_unit = json["pixel_per_unit"];
                    FilterMode filter = Enum.Parse<FilterMode>(json["filter_mode"]);
                    var tex_file = Path.Combine(Path.GetDirectoryName(file), tex_name);
                    var tex = Texture.Load(tex_file, filter, false);
                    JsonArray arr = json["frames"] as JsonArray;
                    Frame[] frames = new Frame[arr.Count];
                    for (int i = 0; i < arr.Count; i++)
                    {
                        string name = arr[i]["name"];
                        Rect rect = JsonHelper.ParseRect(arr[i]["rect"]);
                        Vector2 offset = JsonHelper.ParseVector2(arr[i]["offset"]);
                        frames[i] = new Frame(name, rect, offset, tex.Size, pixel_per_unit);
                    }

                    return SpriteSheet.Create(frames, tex, pixel_per_unit);
                }
                finally
                {
                    reader.Dispose();
                }
            }

            return FromCacheOrFile(file, load_file) as SpriteSheet;
        }

        public int GetFrameIndex(string name) => Array.FindIndex(frames, (f) => f.Name == name);

        public class Frame
        {
            string name;
            Vertex[] verts;

            internal Frame(string name, Rect rect, Vector2 offset, Vector2 tex_size, float pixel_per_unit)
            {
                this.name = name;
                var texcoord = rect / tex_size;
                rect /= pixel_per_unit;
                offset /= pixel_per_unit;
                rect.Position = -offset;
                verts = new Vertex[6];
                verts[0] = new Vertex(rect.XMinYMin, texcoord.XMinYMin, Color.White);
                verts[1] = new Vertex(rect.XMaxYMax, texcoord.XMaxYMax, Color.White);
                verts[2] = new Vertex(rect.XMinYMax, texcoord.XMinYMax, Color.White);
                verts[3] = new Vertex(rect.XMinYMin, texcoord.XMinYMin, Color.White);
                verts[4] = new Vertex(rect.XMaxYMin, texcoord.XMaxYMin, Color.White);
                verts[5] = new Vertex(rect.XMaxYMax, texcoord.XMaxYMax, Color.White);
            }

            /// <summary>
            /// Name of the frame
            /// </summary>
            public string Name => name;

            /// <summary>
            /// Local space vertex data for the frame
            /// </summary>
            public Vertex[] Vertices => verts;
        }
    }
}