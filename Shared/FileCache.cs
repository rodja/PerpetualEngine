using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace PerpetualEngine
{
    public class FileCache
    {
        public static string Directory;
        public static Dictionary<string, Action<string>> fetchOperations = new Dictionary<string, Action<string>>();
        public static List<string> inProgress = new List<string>();

        public FileCache()
        {
        }

        public static void Put(string id, Action<string> fetchOperation)
        {
            fetchOperations.Add(id, fetchOperation);
        }

        public static async void Fetch(string id)
        {
            await Get(id);
        }

        public static async Task<string> Get(string id)
        {
            if (!fetchOperations.ContainsKey(id))
                return null;

            string path = Directory + "/" + id;

            await Task.Run(delegate {
                while (inProgress.Contains(id))
                    Thread.Sleep(100);
            });

            if (File.Exists(path))
                return path;

            inProgress.Add(id);
            try {
                await Task.Run(() => fetchOperations[id](path));
            } catch (Exception) {
                if (File.Exists(path))
                    File.Delete(path);
            }
            inProgress.Remove(id);

            return path;
        }

        public static bool Contains(string id)
        {
            return fetchOperations.ContainsKey(id);
        }
    }
}

