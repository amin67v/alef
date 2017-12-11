using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Engine
{
    public abstract partial class Resource : Disposable
    {
        static Dictionary<string, Resource> res_map;
        static string root_path = null;
        static ZipArchive archive = null;

        string file;

        /// <summary>
        /// The file which used to load this resource.
        /// </summary>
        public string FileName => file;

        protected static Resource FromCacheOrFile(string file_name, LoadHandler load_func)
        {
            file_name = file_name.TrimStart('/');
            Resource res;
            if (res_map.TryGetValue(file_name, out res))
            {
                return res;
            }
            else
            {
                if (archive == null)
                {
                    var abs_path = Path.Combine(root_path, file_name);
                    if (!File.Exists(abs_path))
                    {
                        Log.Info($"Asset does not exist at path '{file_name}'.");
                        return null;
                    }
                    var stream = File.OpenRead(abs_path);
                    res = load_func(stream);
                }
                else
                {
                    var entry = archive.GetEntry(file_name);
                    var stream = entry.Open();
                    res = load_func(stream);
                }

                res.file = file_name;
                res_map[file_name] = res;
                return res;
            }
        }

        protected override void OnDisposeUnmanaged()
        {
            if (!string.IsNullOrEmpty(file))
            {
                res_map.Remove(file);
                file = null;
            }
        }

        internal static void init(bool use_archive)
        {
            if (use_archive)
            {
                var archive_path = Path.Combine(App.ExePath, "data.pak");;
                if (!File.Exists(archive_path))
                {
                    string msg = "Invalid path for data pak.";
                    Log.Error(msg);
                    throw new FileNotFoundException(msg);
                }
                archive = ZipFile.OpenRead(archive_path);
            }
            else
            {
                root_path = Path.Combine(App.ExePath, "data");;
                if (!Directory.Exists(root_path))
                {
                    string msg = "Invalid path for root directory.";
                    Log.Error(msg);
                    throw new DirectoryNotFoundException(msg);
                }
            }

            res_map = new Dictionary<string, Resource>(50);
        }

        internal static void shut_down()
        {
            List<Resource> tmp_all_res = new List<Resource>(res_map.Values);
            for (int i = 0; i < tmp_all_res.Count; i++)
                tmp_all_res[i].Dispose();

            tmp_all_res.Clear();
            res_map.Clear();
        }

        protected delegate Resource LoadHandler(Stream stream);
    }
}