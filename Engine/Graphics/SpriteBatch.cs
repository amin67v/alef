using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class SpriteBatch : Disposable, IDrawable
    {
        Array<Vertex> verts;
        Transform xform;
        SpriteSheet spr;
        Shader shader;
        MeshBuffer mb;
        BlendMode blend;
        Rect bounds;
        int layer;
        bool mb_dirty = true;
        bool bounds_dirty = true;

        public SpriteBatch(SpriteSheet spr, int layer)
            : this(spr, DefaultShaders.ColorMult, null, BlendMode.AlphaBlend, layer, 20) { }

        public SpriteBatch(SpriteSheet spr, Shader shader, Transform xform, BlendMode blend_mode, int layer, int capacity)
        {
            verts = new Array<Vertex>(capacity * 6);
            this.spr = spr;
            this.layer = layer;
            this.shader = shader;
            this.xform = xform;
            this.blend = blend_mode;
            mb = MeshBuffer.Create();
        }

        /// <summary>
        /// Bounding rect which all vertices are inside
        /// </summary>
        public Rect Bounds
        {
            get
            {
                if (bounds_dirty)
                {
                    float xmin = float.MaxValue;
                    float xmax = float.MinValue;
                    float ymin = float.MaxValue;
                    float ymax = float.MinValue;
                    for (int i = 0; i < verts.Count; i++)
                    {
                        var pos = verts[i].Position;
                        xmin = MathF.Min(xmin, pos.X);
                        xmax = MathF.Max(xmax, pos.X);
                        ymin = MathF.Min(ymin, pos.Y);
                        ymax = MathF.Max(ymax, pos.Y);
                    }
                    bounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
                    bounds_dirty = false;
                }
                return bounds;
            }
        }

        public int Layer
        {
            get => layer;
            set => layer = value;
        }

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
            var sprite_verts = spr[frame].Vertices;
            for (int i = 0; i < sprite_verts.Length; i++)
            {
                var pos = xform.LocalToWorld(sprite_verts[i].Position);
                verts.Push(new Vertex(pos, sprite_verts[i].Texcoord, color));
            }
            mb_dirty = bounds_dirty = true;
        }

        /// <summary>
        /// Clears batch from all sprites
        /// </summary>
        public void Clear()
        {
            verts.Clear(false);
            mb_dirty = bounds_dirty = true;
        }

        public void Draw()
        {
            if (verts.Count > 0)
            {
                var gfx = App.Graphics;
                gfx.SetBlendMode(blend);
                gfx.SetShader(shader);
                OnSetUniforms();

                if (mb_dirty)
                {
                    mb.UpdateVertices(verts);
                    mb_dirty = false;
                }

                (App.ActiveState as Scene)?.DebugDraw.Rect(Bounds, Color.Red);
                mb.Draw(PrimitiveType.Triangles);
            }
        }

        /// <summary>
        /// Override to set custom uniforms
        /// </summary>
        protected virtual void OnSetUniforms()
        {
            shader.SetTexture("main_tex", 0, spr.Texture);
            shader.SetMatrix4x4("view_mat", App.Graphics.ViewMatrix);
            if (xform != null)
                shader.SetMatrix3x2("model_mat", xform.Matrix);
            else
                shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);
        }

        protected override void OnDisposeManaged()
        {
            verts.Clear(false);
            verts = null;
            xform = null;
            spr = null;
            shader = null;
            mb.Dispose();
        }
    }
}
