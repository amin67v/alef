using System;
using System.Collections.Generic;

namespace Engine
{
    public class StaticBatch : Entity
    {
        static StaticBatch instance;

        Dictionary<BatchKey, SpriteBatch> batch_map;
        float batch_area_size = 100;

        public static void AddSprite(Transform xform, SpriteSheet spr, int frame)
        {
            AddSprite(xform, spr, frame, 0, DefaultShaders.ColorMult, BlendMode.AlphaBlend, Color.White);
        }

        public static void AddSprite(Transform xform, SpriteSheet spr, int frame, int layer, Color color)
        {
            AddSprite(xform, spr, frame, layer, DefaultShaders.ColorMult, BlendMode.AlphaBlend, color);
        }

        public static void AddSprite(Transform xform, SpriteSheet spr, int frame, int layer, Shader shader, BlendMode blend_mode, Color color)
        {
            if (instance == null)
                instance = Scene.Spawn<StaticBatch>("StaticBatch");

            var key = new BatchKey()
            {
                Sprite = spr,
                Shader = shader,
                BlendMode = blend_mode,
                Layer = layer,
                X = (int)(xform.Position.X / instance.batch_area_size),
                Y = (int)(xform.Position.Y / instance.batch_area_size),
            };

            SpriteBatch target;
            if (!instance.batch_map.TryGetValue(key, out target))
            {
                target = new SpriteBatch(spr, shader, null, blend_mode, layer, 20);
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
            foreach (var item in batch_map.Values)
                item.Dispose();

            batch_map.Clear();
            Scene.OnPreRender -= on_pre_render;
            instance = null;
        }

        void on_pre_render()
        {
            foreach (var item in batch_map.Values)
                Scene.RegisterForDraw(item);
        }

        struct BatchKey : IEquatable<BatchKey>
        {
            public SpriteSheet Sprite;
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
                other.Sprite == Sprite;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    const int prime = 486187739;
                    int hash = prime;
                    hash = (hash + Sprite.GetHashCode()) * prime;
                    hash = (hash + (int)BlendMode) * prime;
                    hash = (hash + Shader.GetHashCode()) * prime;
                    hash = (hash + Layer) * prime;
                    hash = (hash + X) * prime;
                    hash = (hash + Y) * prime;
                    return hash;
                }
            }
        }
    }
}