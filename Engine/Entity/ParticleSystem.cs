using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;

using static System.MathF;

namespace Engine
{
    using static MathF;

    public class ParticleSystem : Entity.Node, IDrawable
    {
        static Comparer<ParticleVertex> younger_comparer = Comparer<ParticleVertex>.Create((x, y) => x.Life < y.Life ? -1 : 1);
        static Comparer<ParticleVertex> older_comparer = Comparer<ParticleVertex>.Create((x, y) => x.Life > y.Life ? -1 : 1);

        MeshBuffer<ParticleVertex> buffer;
        Array<ParticleVertex> verts;
        Array<Vector2> forces;
        ParticleSortMode sort_mode;
        Gradient color;
        float emit_life = 2.5f;
        float emit_rate = .1f;
        float emit_radius = .5f;
        float emit_angle = PI * .1f;
        float emit_timer = 0;
        float emit_vel_min = 4;
        float emit_vel_max = 6;
        float rot1 = 0, rot2 = 0;
        float size1 = 1, size2 = 1;
        BlendMode blend = BlendMode.AlphaBlend;
        Texture tex;
        Shader shader;
        int layer = 0;
        TweenType rot_twn;
        TweenType size_twn;
        Rect bounds = new Rect(0, 0, 1, 1);
        bool rand_rot = false;
        bool auto_update_bounds = true;

        public int AllocatedCount => verts.Items.Length;

        public int AliveCount => verts.Count;

        public Gradient ColorGradient => color;

        public Rect Bounds => bounds;

        public int Layer
        {
            get => layer;
            set => layer = value;
        }

        public int EmitPerSecond
        {
            get => (int)(1f / emit_rate);
            set
            {
                emit_rate = 1f / value;
            }
        }

        public ref float EmitRadius => ref emit_radius;

        public float EmitAngle
        {
            get => emit_angle * RadToDeg;
            set => emit_angle = value * DegToRad;
        }

        public ref float Life => ref emit_life;

        public ref ParticleSortMode SortMode => ref sort_mode;

        public ref float RotateStart => ref rot1;

        public ref float RotateEnd => ref rot2;

        public ref TweenType RotateTween => ref rot_twn;

        public ref float SizeStart => ref size1;

        public ref float SizeEnd => ref size2;

        public ref TweenType SizeTween => ref size_twn;

        public ref bool AutoUpdateBounds => ref auto_update_bounds;

        public void UpdateBounds()
        {
            float xmin = float.MaxValue;
            float xmax = float.MinValue;
            float ymin = float.MaxValue;
            float ymax = float.MinValue;
            for (int i = 0; i < verts.Count; i++)
            {
                var pos = verts[i].Position;
                xmin = Min(xmin, pos.X);
                xmax = Max(xmax, pos.X);
                ymin = Min(ymin, pos.Y);
                ymax = Max(ymax, pos.Y);
            }
            bounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            bounds.Inflate(Max(size1, size2) * 0.7071065f);
        }

        public void Draw()
        {
            var gfx = App.Graphics;
            gfx.SetBlendMode(blend);
            gfx.SetShader(shader);
            shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);
            //shader.SetMatrix3x2("model_mat", Matrix);
            shader.SetTexture("main_tex", 0, tex);

            buffer.UpdateVertices(verts);

            (App.ActiveState as Scene)?.DebugDraw.Rect(Bounds, Color.Red);
            buffer.Draw(PrimitiveType.Points);
        }

        protected override void OnBegin() 
        {
            buffer = MeshBuffer<ParticleVertex>.Create();
            verts = new Array<ParticleVertex>(50);
            forces = new Array<Vector2>();
            shader = Data.Get<Shader>("Particle.Shader");
            tex = Data.Get<Texture>("White.Texture");
            color = new Gradient();
            UseUpdate();
        }

        protected override void OnUpdate(float dt)
        {
            emit_timer += dt;
            while (emit_timer >= emit_rate)
            {
                emit();
                emit_timer -= emit_rate;
            }

            Vector2 force = Vector2.Zero;
            if (forces.Count > 0)
            {
                for (int i = 0; i < forces.Count; i++)
                    force += forces[i];

                force /= forces.Count;
            }

            var life_norm = 1f / emit_life;
            for (int i = 0; i < verts.Count; ++i)
            {
                var t = 1 - verts[i].Life * life_norm;
                verts[i].Life -= dt;
                verts[i].Velocity += force * dt;
                verts[i].Position += verts[i].Velocity * dt;
                verts[i].Color = color.Sample(t);
                verts[i].Rotation += Lerp(RotateStart, RotateEnd, Tween.Invoke(rot_twn, t)) * dt;
                verts[i].Size = Lerp(SizeStart, SizeEnd, Tween.Invoke(size_twn, t));
            }

            for (int i = verts.Count - 1; i >= 0; --i)
            {
                if (verts[i].Life < 0)
                {
                    verts.Swap(i, verts.Count - 1);
                    verts.Pop();
                }
            }

            if (auto_update_bounds)
                UpdateBounds();

            sort();

            (App.ActiveState as Scene)?.AddDrawable(this);
        }

        void emit()
        {
            verts.Push(new ParticleVertex()
            {
                Life = emit_life,
                Velocity = LocalToWorldNormal(rand_dir(-emit_angle, emit_angle)) * Random.Float(emit_vel_min, emit_vel_max),
                Position = LocalToWorld(rand_dir(-PI, PI) * emit_radius * Random.Float()),
                Texcoord = Vector2.Zero,
                Rotation = rand_rot ? Random.Float(-PI, PI) : 0,
                Size = .1f,
                Color = Random.Color()
            });
        }

        void sort()
        {
            if (sort_mode == ParticleSortMode.None)
                return;
            else if (sort_mode == ParticleSortMode.Younger)
                verts.Sort(younger_comparer);
            else if (sort_mode == ParticleSortMode.Older)
                verts.Sort(older_comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Vector2 rand_dir(float min, float max)
        {
            var angle = Random.Float(min, max);
            return new Vector2(Sin(angle), Cos(angle));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ParticleVertex : IVertex
        {
            public int SizeInBytes
            {
                get
                {
                    unsafe { return sizeof(ParticleVertex); }
                }
            }

            public string Format => "float.vec2.vec2.vec2.float.float.color";

            public float Life;
            public Vector2 Velocity;
            public Vector2 Position;
            public Vector2 Texcoord;
            public float Rotation;
            public float Size;
            public Color Color;
        }
    }

    public enum ParticleSortMode { None, Younger, Older }


}