using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public sealed class ResourceManager
    {
        Dictionary<string, Resource> res_map;
        string root_path = null;
        ZipArchive archive = null;

        internal ResourceManager()
        {
            var pakfile = Path.Combine(App.ExePath, "data.pak");
            // use pak file if exist otherwise use data folder
            if (File.Exists(pakfile))
            {
                if (!File.Exists(pakfile))
                {
                    string msg = "Invalid path for data pak.";
                    App.Log.Error(msg);
                    throw new FileNotFoundException(msg);
                }
                archive = ZipFile.OpenRead(pakfile);
            }
            else
            {
                root_path = Path.Combine(App.ExePath, "data"); ;
                if (!Directory.Exists(root_path))
                {
                    string msg = "Invalid path for root directory.";
                    App.Log.Error(msg);
                    throw new DirectoryNotFoundException(msg);
                }
            }

            res_map = new Dictionary<string, Resource>(50);
        }

        public Resource FromCacheOrFile(string filename, ResourceLoadHandler load_func)
        {
            filename = filename.TrimStart('/');
            Resource res;
            if (res_map.TryGetValue(filename, out res))
            {
                return res;
            }
            else
            {
                if (archive == null)
                {
                    var abs_path = Path.Combine(root_path, filename);
                    if (!File.Exists(abs_path))
                    {
                        App.Log.Info($"Asset does not exist at path '{filename}'.");
                        return null;
                    }
                    var stream = File.OpenRead(abs_path);
                    res = load_func(stream);
                }
                else
                {
                    var entry = archive.GetEntry(filename);
                    var stream = entry.Open();
                    res = load_func(stream);
                }

                res.FileName = filename;
                res_map[filename] = res;
                return res;
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
        }

        public delegate Resource ResourceLoadHandler(Stream stream);
    }
}