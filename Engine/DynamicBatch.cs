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

        public static void Draw(Transform xform, SpriteSheet sheet, int frame, int sort_key, Color color)
        {
            Draw(xform, sheet, frame, sort_key, DefaultShaders.ColorMult, BlendMode.AlphaBlend, color);
        }

        public static void Draw(Transform xform, SpriteSheet sheet, int frame, int sort_key, Shader shader, BlendMode blend_mode, Color color)
        {
            if (instance == null)
                instance = Scene.Spawn<DynamicBatch>("DynamicBatch");

            var key = new BatchKey()
            {
                Sheet = sheet,
                Shader = shader,
                BlendMode = blend_mode,
                SortKey = sort_key
            };

            SpriteBatch target;
            if (!instance.batch_map.TryGetValue(key, out target))
            {
                target = new SpriteBatch(sheet, shader, null, blend_mode, sort_key);
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

        struct BatchKey
        {
            public SpriteSheet Sheet;
            public BlendMode BlendMode;
            public Shader Shader;
            public int SortKey;
        }
    }
}