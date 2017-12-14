using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public abstract class Resource : Disposable
    {
        string file;

        /// <summary>
        /// The file which used to load this resource.
        /// </summary>
        public string FileName
        {
            get => file;
            internal set => file = value;
        }

        protected override void OnDisposeUnmanaged()
        {
            if (!string.IsNullOrEmpty(file))
            {
                App.ResourceManager.remove(file);
                file = null;
            }
        }
    }
}