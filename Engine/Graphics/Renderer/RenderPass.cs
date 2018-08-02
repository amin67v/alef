using System;

namespace Engine
{
    public abstract class RenderPass : ObjectBase
    {
        public RenderPass()
        {
            Renderer.OnReset += OnReset;
            OnBegin();
        }

        protected GBuffer GBuffer => Renderer.GBuffer;

        public abstract void Draw(Scene scene);

        protected void DrawScreenEffect(RenderTarget target, Shader shader, object obj = null)
        {
            Graphics.RenderTarget = target;
            Graphics.Viewport = new Rect(0, 0, target?.Width ?? Renderer.RenderWidth, target?.Height ?? Renderer.RenderHeight);
            Graphics.SetClearValue(0, Color.Black);
            Graphics.Clear(BufferMask.All);

            Graphics.SetDepthTest(DepthTest.Disable);
            Graphics.SetBlendMode(BlendMode.Disable);
            Graphics.SetFaceCull(FaceCull.Back);
            Graphics.SetDepthWrite(false);

            Graphics.SetShader(shader, obj);
            Graphics.Draw(MeshBuffer.ScreenQuad);
        }

        protected virtual void OnReset(int w, int h) { }

        internal override void DestroyImp(bool destroying)
        {
            if (!IsDestroyed)
            {
                OnDestroy();
                Renderer.OnReset -= OnReset;
                IsDestroyed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}