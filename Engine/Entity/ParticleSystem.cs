using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public class ParticleSystem : EntityNode
    {
        Image color_over_life;
        Vector2 life = new Vector2(5, 10);
        float emit_rate = 6f;
        float emit_radius = 1f;
        float emit_angle = MathF.PI * .25f; // 45 degree
        int max_count = 100;

        Array<Particle> particles = new Array<Particle>();
        Array<Vertex> verts = new Array<Vertex>();

        protected override void OnUpdate(float dt)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Life -= dt;

                //particles[i].Velocity
            }
        }

        void emit()
        {
            var particle = new Particle();
            //particle. = rand_dir(-this.emit_angle, this.emit_angle);
            var vertex = new Vertex(rand_dir(-MathF.PI, MathF.PI) * emit_radius, Vector2.Zero, Color.White);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Vector2 rand_dir(float min, float max)
        {
            var angle = Random.Float(min, max);
            return new Vector2(MathF.Sin(angle), MathF.Cos(angle));
        }

        struct Particle
        {
            public float MaxLife;
            public float Life;
            public float Size;
            public float Rotation;
            public Vector2 Velocity;
        }
    }
}