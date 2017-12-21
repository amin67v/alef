using System;
using System.Collections.Generic;

namespace Engine
{
    public class StaticBatch : Entity
    {
        static StaticBatch instance;

        Dictionary<BatchKey, SpriteBatch> batch_map;
        float batch_area_size = 100;

        public static void AddSprite(Transform xform, SpriteSheet sheet, int frame)
        {
            AddSprite(xform, sheet, frame, 0, DefaultShaders.ColorMult, BlendMode.AlphaBlend, Color.White);
        }

        public static void AddSprite(Transform xform, SpriteSheet sheet, int frame, int layer, Color color)
        {
            AddSprite(xform, sheet, frame, layer, DefaultShaders.ColorMult, BlendMode.AlphaBlend, color);
        }

        public static void AddSprite(Transform xform, SpriteSheet sheet, int frame, int layer, Shader shader, BlendMode blend_mode, Color color)
        {
            if (instance == null)
                instance = Scene.Spawn<StaticBatch>("StaticBatch");

            var key = new BatchKey()
            {
                Sheet = sheet,
                Shader = shader,
                BlendMode = blend_mode,
                Layer = layer,
                X = (int)(xform.Position.X / instance.batch_area_size),
                Y = (int)(xform.Position.Y / instance.batch_area_size),
            };

            SpriteBatch target;
            if (!instance.batch_map.TryGetValue(key, out target))
            {
                target = new SpriteBatch(sheet, shader, null, blend_mode, layer, 20);
                instance.batch_map.Add(key, target);
            }

            target.AddSprite(xform, frame, color);
        }

        public void SetBatchAreaSize(float value) => batch_area_size = value;

        public override void OnBegin()
        {
            batch_map = new Dictionary<BatchKey, SpriteBatch>();
            Scene.OnPreRender += on_pre_render;
        }

        public override void OnDestroy()
        {
            instance = null;
            foreach (var item in batch_map.Values)
                item.Dispose();

            batch_map.Clear();
            Scene.OnPreRender -= on_pre_render;
        }

                void on_pre_render()
        {
            foreach (var item in batch_map.Values)
                Scene.RegisterForDraw(item);
        }

        struct BatchKey : IEquatable<BatchKey>
        {
            public SpriteSheet Sheet;
            public BlendMode BlendMode;
            public Shader Shader;
            public int Layer;
            public int X;
            public int Y;

            public bool Equals(BatchKey other)
            {
                return other.Layer == Layer &&
                other.BlendMode == BlendMode &&
                other.Shader == Shader &&
                other.Sheet == Sheet;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    const int prime = 486187739;
                    int h = prime;
                    h = (h + Sheet.GetHashCode()) * prime;
                    h = (h + (int)BlendMode) * prime;
                    h = (h + Shader.GetHashCode()) * prime;
                    h = (h + Layer) * prime;
                    h = (h + X) * prime;
                    h = (h + Y) * prime;
                    return h;
                }
            }
        }
    }
}