using System;
using System.IO;
using System.Json;
using System.Collections.Generic;

namespace Engine
{
    public abstract partial class Resource : Disposable
    {
        static Dictionary<string, Resource> res_map;
        static string root_path = null;

        string file;

        /// <summary>
        /// The file which used to load this resource.
        /// </summary>
        public string File => file;

        protected static Resource FromCacheOrFile(string file, LoadHandler load_func)
        {
            file = file.TrimStart('/');
            Resource res;
            if (res_map.TryGetValue(file, out res))
            {
                return res;
            }
            else
            {
                var absPath = System.IO.Path.Combine(root_path, file);
                if (!System.IO.File.Exists(absPath))
                {
                    Log.Info($"Asset does not exist at path '{file}'.");
                    return null;
                }
                res = load_func(absPath);
                res.file = file;
                res_map[file] = res;
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

        internal static void init()
        {
            res_map = new Dictionary<string, Resource>(50);
            root_path = System.IO.Path.Combine(App.ExePath, "Data");
            if (Directory.Exists(root_path))
            {
                Log.Info($"Resource manager initialized, root directory set to '{root_path}'.");
            }
            else
            {
                string msg = "Invalid path for root directory.";
                Log.Error(msg);
                throw new DirectoryNotFoundException(msg);
            }
        }

        internal static void shut_down()
        {
            List<Resource> tmp_all_res = new List<Resource>(res_map.Values);
            for (int i = 0; i < tmp_all_res.Count; i++)
                tmp_all_res[i].Dispose();

            tmp_all_res.Clear();
            res_map.Clear();
        }

        protected delegate Resource LoadHandler(string abs_path);
    }
}