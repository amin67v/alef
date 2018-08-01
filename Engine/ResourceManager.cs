using System;
using System.Json;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;


namespace Engine
{
    public sealed class ResourceManager : ObjectBase
    {
        Dictionary<string, Resource> resourceMap;
        ZipArchive pak = null;

        internal ResourceManager()
        {
            resourceMap = new Dictionary<string, Resource>();
            var pakfile = Game.Current.GetAbsolutePath("Data.pak");
            if (File.Exists(pakfile))
                pak = ZipFile.OpenRead(pakfile);
        }

        public Resource Load(string file, ResourceLoadHandler loadFunc)
        {
            file = file.ToLower().Replace('\\', '/').TrimStart('/');
            Stream stream = null;
            try
            {
                Resource res;
                if (resourceMap.TryGetValue(file, out res))
                {
                    return res;
                }
                else
                {
                    stream = GetFileStream(file);
                    res = loadFunc(stream);
                    res.Key = file;
                    resourceMap[file] = res;
                    return res;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        public Stream GetFileStream(string file)
        {
            Stream stream = null;
            if (pak == null)
            {
                var path = Game.Current.GetAbsolutePath($"Data/{file}");
                if (!File.Exists(path))
                {
                    Game.Current.Log.Info($"Resource does not exist at path '{file}'.");
                    return null;
                }
                stream = File.OpenRead(path);
            }
            else
            {
                var entry = pak.GetEntry(file);
                if (entry == null)
                {
                    Game.Current.Log.Info($"Resource does not exist at path '{file}'.");
                    return null;
                }
                stream = entry.Open();
            }
            return stream;
        }

        public T Get<T>(string key) where T : Resource
        {
            Resource res;
            if (resourceMap.TryGetValue(key, out res))
                return res as T;

            return null;
        }

        public void Add(string key, Resource res)
        {
            resourceMap.Add(key, res);
            res.Key = key;
        }

        public void Remove(string key)
        {
            resourceMap.Remove(key);
        }

        protected override void OnDestroy()
        {
            List<Resource> tmpAllRes = new List<Resource>(resourceMap.Values);
            for (int i = 0; i < tmpAllRes.Count; i++)
                Destroy(tmpAllRes[i]);

            tmpAllRes.Clear();
            resourceMap.Clear();

            pak?.Dispose();

            resourceMap = null;
            pak = null;
        }
    }

    public delegate Resource ResourceLoadHandler(Stream stream);
}
