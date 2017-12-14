using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteBatch : Disposable, IDrawable
    {
        Array<Vertex> verts;
        Transform xform;
        SpriteSheet sheet;
        Shader shader;
        MeshBuffer mb;
        BlendMode blend;
        int layer;
        bool dirty;

        public SpriteBatch(SpriteSheet sheet, int layer)
            : this(sheet, DefaultShaders.ColorMult, null, BlendMode.AlphaBlend, layer) { }

        public SpriteBatch(SpriteSheet sheet, Shader shader, Transform xform, BlendMode blend_mode, int layer)
        {
            verts = new Array<Vertex>(20 * 6); // <-- make enough room for 20 sprites by default!
            this.sheet = sheet;
            this.layer = layer;
            this.shader = shader;
            this.xform = xform;
            this.blend = blend_mode;
            mb = MeshBuffer.Create();
        }

        public int Layer
        {
            get => layer;
            set => layer = value;
        }

        /// <summary>
        /// Gets or set the main texture used to draw sprites
        /// </summary>
        public Texture MainTex => sheet.Texture;

        /// <summary>
        /// BlendMode used to draw this batch
        /// </summary>
        public BlendMode BlendMode
        {
            get => blend;
            set => blend = value;
        }

        /// <summary>
        /// Shader used to draw this batch
        /// </summary>
        public Shader Shader
        {
            get => shader;
            set => shader = value;
        }

        /// <summary>
        /// Transform this batch is attached to
        /// </summary>
        public Transform Transform
        {
            get => xform;
            set => xform = value;
        }

        /// <summary>
        /// The number of sprite this batch contains
        /// </summary>
        public int Count => verts.Count / 6;

        /// <summary>
        /// Adds sprite to the batch with the given transform and color
        /// </summary>
        public void AddSprite(Transform xform, int frame, Color color)
        {
            var sprite_verts = sheet[frame].Vertices;
            for (int i = 0; i < sprite_verts.Length; i++)
            {
                var pos = xform.LocalToWorld(sprite_verts[i].Position);
                verts.Push(new Vertex(pos, sprite_verts[i].Texcoord, color));
            }
            dirty = true;
        }

        /// <summary>
        /// Clears batch from all sprites
        /// </summary>
        public void Clear()
        {
            verts.Clear();
            dirty = true;
        }

        public void Draw()
        {
            if (verts.Count > 0)
            {
                App.Graphics.BlendMode = blend;
                Shader.Active = shader;
                OnSetUniforms();

                if (dirty)
                    mb.UpdateVertices(verts);

                mb.Draw(PrimitiveType.Triangles);
            }
        }

        /// <summary>
        /// Override to set custom uniforms
        /// </summary>
        protected virtual void OnSetUniforms()
        {
            shader.SetTexture("main_tex", 0, sheet.Texture);
            shader.SetMatrix4x4("view_mat", App.Graphics.ViewMatrix);
            if (xform != null)
                shader.SetMatrix3x2("model_mat", xform.Matrix);
            else
                shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
        }

        protected override void OnDisposeManaged()
        {
            verts.Clear();
            verts = null;
            xform = null;
            sheet = null;
            shader = null;
            mb.Dispose();
        }
    }
}
