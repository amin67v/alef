using System;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Base class for objects that contain unmanaged resources.
    /// </summary>
    public class Disposable : IDisposable
    {
        bool is_disposed;

        ~Disposable()
        {
            dispose(false);
        }

        /// <summary>
        /// Returns true if this object is disposed otherwise returns false
        /// </summary>
        public bool IsDisposed => is_disposed;

        /// <summary>
        /// Release all resources associated to this object.
        /// </summary>
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposeManaged() { }

        protected virtual void OnDisposeUnmanaged() { }

        void dispose(bool disposing)
        {
            if (!is_disposed)
            {
                is_disposed = true;

                OnDisposeUnmanaged();

                if (disposing)
                    OnDisposeManaged();
            }
        }
    }
}