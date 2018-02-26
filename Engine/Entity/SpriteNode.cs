using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteNode : Entity.Node
    {
        SpriteSheetFrame frame;
        BlendMode blend;
        Shader shader;
        Color color;
        int layer;
        int order;

        public SpriteSheetFrame Frame
        {
            get => frame;
            set => frame = value;
        }

        public BlendMode BlendMode
        {
            get => blend;
            set => blend = value;
        }

        public Shader Shader
        {
            get => shader;
            set => shader = value;
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }

        public int Layer
        {
            get => layer;
            set => layer = value;
        }

        public int OrderInBuffer
        {
            get => order;
            set => order = value;
        }

        protected override void OnBegin()
        {
            Shader = Data.Get<Shader>("Mult.Shader");
            Color = Color.White;
            BlendMode = BlendMode.AlphaBlend;
            UseUpdate();
        }

        protected override void OnUpdate(float dt)
        {
            BatchManager.AddSprite(this);
        }
    }
}