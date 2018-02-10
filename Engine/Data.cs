using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public abstract class Data : Disposable
    {
        static Dictionary<string, Data> res_map;
        static ZipArchive pak = null;

        string name;

        /// <summary>
        /// The name of the asset which used to load/add this data.
        /// </summary>
        public string Name
        {
            get => name;
            internal set => name = value;
        }

        protected override void OnDisposeUnmanaged()
        {
            if (!string.IsNullOrEmpty(name))
            {
                res_map.Remove(name);
                name = null;
            }
        }

        public static Data FromCacheOrFile(string filename, DataLoadHandler load_func)
        {
            filename = filename.ToLower().Replace('\\', '/').TrimStart('/');
            Stream stream = null;
            try
            {
                Data res;
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
                            Log.Info($"Data does not exist at path '{filename}'.");
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
                            Log.Info($"Data does not exist at path '{filename}'.");
                            return null;
                        }
                        stream = entry.Open();
                        res = load_func(stream);
                    }

                    res.Name = filename;
                    res_map[filename] = res;
                    return res;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        public static void Add(string name, Data data)
        {
            res_map.Add(name, data);
            data.Name = name;
        }

        public static T Get<T>(string name) where T : Data
        {
            Data value = null;
            res_map.TryGetValue(name, out value);
            return value as T;
        }

        internal static void init()
        {
            var pakfile = App.GetAbsolutePath("data.pak");
            if (File.Exists(pakfile))
                pak = ZipFile.OpenRead(pakfile);

            res_map = new Dictionary<string, Data>(50);
            InternalData.init();
        }

        internal static void shutdown()
        {
            List<Data> tmp_all_res = new List<Data>(res_map.Values);
            for (int i = 0; i < tmp_all_res.Count; i++)
                tmp_all_res[i].Dispose();

            tmp_all_res.Clear();
            res_map.Clear();

            pak?.Dispose();

            res_map = null;
            pak = null;
        }

        public delegate Data DataLoadHandler(Stream stream);
    }
}