using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public sealed class DataCache
    {
        static DataCache instance;

        Dictionary<string, Data> res_map;
        ZipArchive pak = null;

        DataCache() { }

        public static Data FromCacheOrFile(string filename, DataLoadHandler load_func)
        {
            filename = filename.ToLower().Replace('\\', '/').TrimStart('/');
            Stream stream = null;
            try
            {
                Data res;
                if (instance.res_map.TryGetValue(filename, out res))
                {
                    return res;
                }
                else
                {
                    // use root folder instead of pak file
                    if (instance.pak == null)
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
                        var entry = instance.pak.GetEntry(filename);
                        if (entry == null)
                        {
                            Log.Info($"Data does not exist at path '{filename}'.");
                            return null;
                        }
                        stream = entry.Open();
                        res = load_func(stream);
                    }

                    res.Name = filename;
                    instance.res_map[filename] = res;
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
            instance.res_map.Add(name, data);
            data.Name = name;
        }

        public static T Get<T>(string name) where T : Data
        {
            Data value = null;
            instance.res_map.TryGetValue(name, out value);
            return value as T;
        }

        internal static void remove(string filename) => instance.res_map.Remove(filename);

        internal static void init()
        {
            instance = new DataCache();
            var pakfile = App.GetAbsolutePath("data.pak");

            if (File.Exists(pakfile))
                instance.pak = ZipFile.OpenRead(pakfile);


            instance.res_map = new Dictionary<string, Data>(50);

            InternalData.init();
        }

        internal static void shutdown()
        {
            List<Data> tmp_all_res = new List<Data>(instance.res_map.Values);
            for (int i = 0; i < tmp_all_res.Count; i++)
                tmp_all_res[i].Dispose();

            tmp_all_res.Clear();
            instance.res_map.Clear();

            DefaultShaders.dispose_all();

            instance.pak?.Dispose();
        }

        public delegate Data DataLoadHandler(Stream stream);
    }

    public abstract class Data : Disposable
    {
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
                DataCache.remove(name);
                name = null;
            }
        }
    }
}