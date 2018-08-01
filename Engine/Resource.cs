using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public abstract class Resource : ObjectBase
    {
        /// <summary>
        /// Resource key (path to file or unique name) used to load this resource
        /// </summary>
        public string Key { get; internal set; }

        internal override void DestroyImp(bool destroying)
        {
            if (!IsDestroyed)
            {
                OnDestroy();
                if (!string.IsNullOrEmpty(Key))
                {
                    Game.Current.ResourceManager.Remove(Key);
                    Key = null;
                }
                IsDestroyed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}