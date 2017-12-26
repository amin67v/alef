using System;
using System.IO;
using System.Runtime.InteropServices;
using static Engine.Stb;

namespace Engine
{
    public unsafe class Image : Disposable
    {
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

        public int Width => width;

        public int Height => height;

        public IntPtr PixelData => new IntPtr(pixels);

        public static Image FromFile(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return FromFile(stream);
            }
        }

        public static Image FromFile(Stream stream) => new Image(stream);

        public void ToFile(string path, int quality = 100)
        {
            var ext = Path.GetExtension(path).ToLower();
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
        }

        public void ToFile(string path, FileFormat format, int quality = 100)
        {
            switch (format)
            {
                case FileFormat.Png:
                    stbi_write_png(path, width, height, 4, pixels, width * 4);
                    break;
                case FileFormat.Tga:
                    stbi_write_tga(path, width, height, 4, pixels);
                    break;
                case FileFormat.Jpg:
                    stbi_write_jpg(path, width, height, 4, pixels, quality);
                    break;
            }
        }

        protected override void OnDisposeUnmanaged() => free();

        public enum FileFormat { Png, Jpg, Tga }
    }
}