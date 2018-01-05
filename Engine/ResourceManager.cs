using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public sealed class ResourceManager
    {
        Dictionary<string, Resource> res_map;
        ZipArchive pak = null;

        internal ResourceManager()
        {
            var pakfile = App.GetAbsolutePath("data.pak");

            if (File.Exists(pakfile))
                pak = ZipFile.OpenRead(pakfile);


            res_map = new Dictionary<string, Resource>(50);
        }

        public Resource FromCacheOrFile(string filename, ResourceLoadHandler load_func)
        {
            Stream stream = null;
            try
            {
                filename = filename.TrimStart('/');
                Resource res;
                if (res_map.TryGetValue(filename, out res))
                {
                    return res;
                }
                else
                {
                    // use root folder instead of pak file
                    if (pak == null)
                    {
                        var path = App.GetAbsolutePath(filename);
                        if (!File.Exists(path))
                        {
                            App.Log.Info($"Resource does not exist at path '{filename}'.");
                            return null;
                        }
                        stream = File.OpenRead(path);
                        res = load_func(stream);
                    }
                    else
                    {
                        var entry = pak.GetEntry(filename);
                        if (entry == null)
                        {
                            App.Log.Info($"Resource does not exist at path '{filename}'.");
                            return null;
                        }
                        stream = entry.Open();
                        res = load_func(stream);
                    }

                    res.FileName = filename;
                    res_map[filename] = res;
                    return res;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        internal void remove(string filename) => res_map.Remove(filename);

        internal void shutdown()
        {
            List<Resource> tmp_all_res = new List<Resource>(res_map.Values);
            for (int i = 0; i < tmp_all_res.Count; i++)
                tmp_all_res[i].Dispose();

            tmp_all_res.Clear();
            res_map.Clear();

            DefaultShaders.dispose_all();

            pak?.Dispose();
        }

        public delegate Resource ResourceLoadHandler(Stream stream);
    }
}