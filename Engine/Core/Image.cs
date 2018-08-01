using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using static StbImage;

namespace Engine
{
    public unsafe class Image : DisposeBase
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
            var numTexel = width * height;
            for (int i = 0; i < numTexel; i++)
                this[i] = color;
        }

        public Image(int width, int height, IntPtr data) : this(width, height)
        {
            var len = width * height * 4;
            Buffer.MemoryCopy(data.ToPointer(), pixels, len, len);
        }

        public Image(Image other) : this(other.width, other.height)
        {
            var len = width * height * 4;
            Buffer.MemoryCopy(other.pixels, pixels, len, len);
        }

        Image(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                byte[] bytes = reader.ReadBytes((int)stream.Length);
                pixels = (Color*)stbi_load_from_memory(bytes, ref width, ref height);
                free = () => stbi_image_free(pixels);
            }
            finally
            {
                reader.Dispose();
            }
        }

        public ref Color this[int x, int y]
        {
            get
            {
                x = Math.Repeat(x, width);
                y = Math.Repeat(y, height);
                return ref pixels[x + (y * width)];
            }
        }

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

        public void Save(string path, int quality = 100)
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
            FlipVertical(); // flip back !
        }

        public void FlipVertical()
        {
            var halfHeight = Height / 2;

            for (int y = 0; y < halfHeight; y++)
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
            var halfWidth = Width / 2;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < halfWidth; x++)
                {
                    var mx = Width - y - 1;
                    var tmp = this[x, y];
                    this[x, y] = this[mx, y];
                    this[mx, y] = tmp;
                }
            }
        }

        public void BlurVertical(float amount) => blur((int)(amount * 0.5f * Height), 0, 1);

        public void BlurHorizontal(float amount) => blur((int)(amount * 0.5f * Width), 1, 0);

        public void BlurVertical(int pixels) => blur(pixels, 0, 1);

        public void BlurHorizontal(int pixels) => blur(pixels, 1, 0);

        void blur(int pixels, int h, int v)
        {
            var tmp = new Image(this);
            Parallel.For(0, height, (y) =>
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 rgb = Vector3.Zero;
                    float totalW = 0;
                    for (int i = -pixels; i <= pixels; i++)
                    {
                        var w = System.MathF.Abs(i / (float)pixels);
                        int xOffset = x + i * h;
                        int yOffset = y + i * v;
                        rgb += tmp[xOffset, yOffset].ToVector3() * w;
                        totalW += w;
                    }
                    rgb /= totalW;
                    this[x, y] = new Color(rgb);
                }
            });
            tmp.Dispose();
        }

        protected override void OnDispose(bool manual) => free();
    }
}