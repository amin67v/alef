using System;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Base class for objects that contain unmanaged resources.
    /// </summary>
    public class DisposeBase : IDisposable
    {
        ~DisposeBase()
        {
            dispose(false);
        }

        /// <summary>
        /// Returns true if this object is disposed otherwise returns false
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Release all resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose(bool manual) { }

        void dispose(bool manual)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                OnDispose(manual);
            }
        }
    }
}