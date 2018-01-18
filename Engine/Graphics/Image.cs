using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

using static Engine.Stb;

namespace Engine
{
    public unsafe class Image : Disposable
    {
        string name;
        int width, height;
        Color* pixels;
        Action free;

        public Image(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.pixels = (Color*)Marshal.AllocHGlobal(4 * width * height).ToPointer();
            free = () => Marshal.FreeHGlobal(new IntPtr(pixels));
        }

        public Image(int width, int height, Color color) : this(width, height)
        {
            var pix_num = width * height;
            for (int i = 0; i < pix_num; i++)
                this[i] = color;
        }

        public Image(int width, int height, IntPtr data) : this(width, height)
        {
            var len = width * height * 4;
            Buffer.MemoryCopy(data.ToPointer(), pixels, len, len);
        }

        Image(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                byte[] bytes = reader.ReadBytes((int)stream.Length);
                pixels = stbi_load_from_memory(bytes, ref width, ref height);
                free = () => stbi_image_free(pixels);
            }
            finally
            {
                reader.Dispose();
            }
        }

        public ref Color this[int x, int y] => ref pixels[x + (y * width)];

        public ref Color this[int i] => ref pixels[i];

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Width => width;

        public int Height => height;

        public IntPtr PixelData => new IntPtr(pixels);

        public static Image FromFile(string path)
        {
            Image img;
            using (var stream = File.OpenRead(path))
                img = FromFile(stream);

            img.name = Path.GetFileNameWithoutExtension(path);
            return img;
        }

        public static Image FromFile(Stream stream) => new Image(stream);

        public void ToFile(string path, int quality = 100)
        {
            var ext = Path.GetExtension(path).ToLower();
            FlipVertical(); // flip to write correctly
            switch (ext)
            {
                case ".png":
                    stbi_write_png(path, width, height, 4, pixels, width * 4);
                    break;
                case ".jpg":
                case ".jpeg":
                    stbi_write_jpg(path, width, height, 4, pixels, quality);
                    break;
                case ".tga":
                    stbi_write_tga(path, width, height, 4, pixels);
                    break;
                default:
                    throw new Exception($"Image can not be saved, format '{ext}' is not supported.");
            }
            FlipVertical(); // flip back !
        }

        public void FlipVertical()
        {
            var half_h = Height / 2;

            for (int y = 0; y < half_h; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var my = Height - y - 1;
                    var tmp = this[x, y];
                    this[x, y] = this[x, my];
                    this[x, my] = tmp;
                }
            }
        }

        public void FlipHorizontal()
        {
            var half_w = Width / 2;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < half_w; x++)
                {
                    var mx = Width - y - 1;
                    var tmp = this[x, y];
                    this[x, y] = this[mx, y];
                    this[mx, y] = tmp;
                }
            }
        }

        protected override void OnDisposeUnmanaged() => free();
    }
}