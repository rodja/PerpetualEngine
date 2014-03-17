using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace PerpetualEngine
{
    public class Server
    {
        private string Request(string method, string url)
        {
            var request = HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;
            try {
                using (var response = request.GetResponse() as HttpWebResponse) {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (var reader = new StreamReader(response.GetResponseStream())) {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content)) {
                            Console.WriteLine("Response contained empty body...");
                        } else {
                            Console.WriteLine("Response Body: \r\n {0}", content);
                            return content;
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private void RequestFile(string url, string filePath)
        {
            var request = HttpWebRequest.Create(url);
            var response = request.GetResponse();
            using (var input = response.GetResponseStream()) {
                using (var output = new FileStream(filePath, FileMode.Create)) {
                    int bytesRead;
                    byte[] buffer = new byte[32768];
                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0) {
                        output.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public string Get(string url)
        {
            return Request("GET", url);
        }

        public async Task<string> GetAsync(string url)
        {
            return await Task.Run(() => {
                return Get(url);
            });
        }

        public void GetFile(string url, string filePath)
        {
            RequestFile(url, filePath);
        }

        public async void GetFileAsync(string url, string filePath)
        {
            await Task.Run(() => {
                RequestFile(url, filePath);
            });
        }
        // TODO Get() and GetAsync() overloaded for downloading files
        // TODO Post() and PostAsync() for uploading files
    }
}

