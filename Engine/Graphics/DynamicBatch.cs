using System;
using System.Collections.Generic;

namespace Engine
{
    public class DynamicBatch : Entity
    {
        static DynamicBatch instance;

        Dictionary<BatchKey, SpriteBatch> batch_map;

        public static void Draw(Transform xform, SpriteSheet sheet, int frame)
        {
            Draw(xform, sheet, frame, 0, DefaultShaders.ColorMult, BlendMode.AlphaBlend, Color.White);
        }

        public static void Draw(Transform xform, SpriteSheet sheet, int frame, int layer, Color color)
        {
            Draw(xform, sheet, frame, layer, DefaultShaders.ColorMult, BlendMode.AlphaBlend, color);
        }

        public static void Draw(Transform xform, SpriteSheet sheet, int frame, int layer, Shader shader, BlendMode blend_mode, Color color)
        {
            if (instance == null)
                instance = Scene.Spawn<DynamicBatch>("DynamicBatch");

            var key = new BatchKey()
            {
                Sheet = sheet,
                Shader = shader,
                BlendMode = blend_mode,
                Layer = layer
            };

            SpriteBatch target;
            if (!instance.batch_map.TryGetValue(key, out target))
            {
                target = new SpriteBatch(sheet, shader, null, blend_mode, layer);
                instance.batch_map.Add(key, target);
            }

            target.AddSprite(xform, frame, color);
        }

        public override void OnBegin()
        {
            batch_map = new Dictionary<BatchKey, SpriteBatch>();
            Scene.OnPostRender += on_post_render;
            Scene.OnPreRender += on_pre_render;
        }

        void on_pre_render()
        {
            foreach (var item in batch_map.Values)
                Scene.RegisterForDraw(item);
        }

        void on_post_render()
        {
            foreach (var item in batch_map.Values)
                item.Clear();
        }

        public override void OnDestroy()
        {
            instance = null;
            Scene.OnPostRender -= on_post_render;
            Scene.OnPreRender -= on_pre_render;
        }

        struct BatchKey : IEquatable<BatchKey>
        {
            public SpriteSheet Sheet;
            public BlendMode BlendMode;
            public Shader Shader;
            public int Layer;

            public bool Equals(BatchKey other)
            {
                return other.Layer == Layer &&
                other.BlendMode == BlendMode &&
                other.Shader == Shader &&
                other.Sheet == Sheet;
            }

            public override int GetHashCode()
            {
                return Sheet.GetHashCode() * (int)BlendMode * Shader.GetHashCode() * Layer;
            }
        }
    }
}