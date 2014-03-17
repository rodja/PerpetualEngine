using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace PerpetualEngine
{
    public class Server
    {
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
            RequestDownload(url, filePath);
        }

        public async void GetFileAsync(string url, string filePath)
        {
            await Task.Run(() => {
                RequestDownload(url, filePath);
            });
        }

        public void PostFile(string url, string filePath)
        {
            RequestUpload(url, filePath);
        }

        public async void PostFileAsync(string url, string filePath)
        {
            await Task.Run(() => {
                RequestUpload(url, filePath);
            });
        }

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

        private void RequestDownload(string url, string filePath)
        {
            var request = HttpWebRequest.Create(url);
            var response = request.GetResponse();
            using (var input = response.GetResponseStream()) {
                using (var output = new FileStream(filePath, FileMode.Create))
                    StreamCopy(input, output);
            }
        }

        private void RequestUpload(string url, string filePath)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); // TODO casting?
            request.Method = "POST";
            request.AllowWriteStreamBuffering = true; // TODO isn't it true by default?
            using (var input = new FileStream(filePath, FileMode.Open)) {
                request.ContentLength = input.Length; // TODO needs to be set?
                using (var output = request.GetRequestStream())
                    StreamCopy(input, output); // TODO manual buffer necessary?
            }
            // TODO unit test
        }

        private void StreamCopy(Stream input, Stream output)
        {
            int bytesRead;
            byte[] buffer = new byte[32768];
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0) {
                output.Write(buffer, 0, bytesRead);
            }
            // TODO unit test
        }
    }
}

