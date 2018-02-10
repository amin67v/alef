using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class BatchManager : Entity
    {
        static BatchManager instance;

        Dictionary<BatchKey, SpriteBatch> batch_map;
        Shader default_shader;
        Vector2 batch_size = Vector2.One * 100;

        public Vector2 BatchSize
        {
            get => batch_size;
            set => batch_size = value;
        }

        public static void AddSprite(SpriteNode node)
        {
            if (instance == null)
                instance = Spawn<BatchManager>("BatchManager");

            var key = new BatchKey()
            {
                Sprite = node.Frame.SpriteSheet,
                Shader = node.Shader,
                BlendMode = node.BlendMode,
                Layer = node.Layer,
                X = (int)(node.Position.X / instance.BatchSize.X),
                Y = (int)(node.Position.Y / instance.BatchSize.Y),
            };

            SpriteBatch target;
            if (!instance.batch_map.TryGetValue(key, out target))
            {
                target = new SpriteBatch();
                target.MainTexture = node.Frame.SpriteSheet.Texture;
                target.Shader = node.Shader;
                target.BlendMode = node.BlendMode;
                target.Layer = node.Layer;
                instance.batch_map.Add(key, target);
            }

            target.AddSprite(node);
        }

        protected override void OnBegin()
        {
            batch_map = new Dictionary<BatchKey, SpriteBatch>();
            Scene.Active.OnPreRender += on_pre_render;
            default_shader = Data.Get<Shader>("Mult.Shader");
            CreateRootNode<EntityNode>();
        }

        protected override void OnDestroy()
        {
            foreach (var item in batch_map.Values)
                item.Dispose();
            
            batch_map.Clear();
            Scene.Active.OnPreRender -= on_pre_render;
            instance = null;
        }

        void on_pre_render()
        {
            foreach (var item in batch_map.Values)
            {
                item.Rebuild();
                Scene.Active.AddDrawable(item);
                item.Clear();
            }
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