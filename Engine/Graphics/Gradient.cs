using System;
using System.Collections.Generic;

namespace Engine
{
    public class Gradient
    {
        Array<ColorKey> keys = new Array<ColorKey>();
        Color[] baked = new Color[256];
        int id = 0;


        public Gradient()
        {
            AddKey(0, Color.White);
            AddKey(1, Color.Black);
        }

        public int KeyCount => keys.Count;

        public Color Sample(float t) => baked[Math.Min((uint)(t * 255), 255)];

        public int AddKey(float pos) => AddKey(pos, evaluate(pos));

        public int AddKey(float pos, Color color)
        {
            keys.Push(new ColorKey(id, pos, color));
            keys.Sort();
            bake();
            return id++;
        }

        public void RemoveKey(int index)
        {
            if (index == 0 || index == keys.Count - 1)
                return;

            keys.RemoveAt(index);
            bake();
        }

        public Color GetColor(int index) => keys[index].Color;

        public void SetColor(int index, Color value)
        {
            keys[index].Color = value;
            bake();
        }

        public float GetPosition(int index) => keys[index].Position;

        public void SetPosition(int index, float value)
        {
            keys[index].Position = value;
            keys.Sort();
            bake();
        }

        public int GetId(int index) => keys[index].Id;

        public int IndexOf(int id) => keys.FindIndex((item) => item.Id == id);

        Color evaluate(float t)
        {
            int a = 0;
            int b = 1;
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Position > t)
                {
                    b = i;
                    break;
                }
                else
                {
                    a = i;
                }
            }

            return Color.Blend(keys[a].Color, keys[b].Color, t.Normalize(keys[a].Position, keys[b].Position));
        }

        void bake()
        {
            for (int i = 0; i < 256; i++)
                baked[i] = evaluate(i / 255f);
        }

        struct ColorKey : IComparable<ColorKey>
        {
            public int Id;
            public float Position;
            public Color Color;

            public ColorKey(int id, float pos, Color color)
            {
                Id = id;
                Position = pos;
                Color = color;
            }

            public int CompareTo(ColorKey other) => Position > other.Position ? 1 : -1;
        }
    }
}