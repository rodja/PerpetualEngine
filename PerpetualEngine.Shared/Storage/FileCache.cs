using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace PerpetualEngine.Storage
{
    public class FileCache
    {
        static string directory = null;
        public static Dictionary<string, Action<string>> fetchOperations = new Dictionary<string, Action<string>>();
        public static List<string> inProgress = new List<string>();

        public static string Directory {
            get {
                if (directory == null)
                    Console.WriteLine("FileCache Error: accessing directory before it is set.");
                return directory;
            }
            set {
                System.IO.Directory.CreateDirectory(value);
                directory = value;
            }
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

            if (inProgress.Contains(id))
                await Task.Run(delegate {
                    while (inProgress.Contains(id))
                        Thread.Sleep(100);
                });

            if (File.Exists(path))
                return path;

            inProgress.Add(id);
            try {
                await Task.Run(() => {
                    Console.WriteLine("executing new fetch for " + id);
                    fetchOperations[id](path);
                });
            } catch (Exception e) {
                Console.WriteLine("FileCache Error: " + e.Message);
                if (File.Exists(path))
                    File.Delete(path);
                return null;
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

