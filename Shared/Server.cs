using System;
using System.Net;
using System.IO;

namespace PerpetualEngine
{
    public class Server
    {
        private string Request(string method, string path)
        {
            var request = HttpWebRequest.Create(path);
            request.ContentType = "application/json";
            request.Method = method;
            try {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
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

        public string DownloadString(string path)
        {
            return Request("GET", path);
        }
    }
}

