using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TextModeration
{
    class Helpers
    {
        /// <summary>
        /// Check if folder and file exists
        /// </summary>
        /// <returns></returns>
        public static bool FolderAndFileExist()
        {
            if (!Directory.Exists(Globals.TOP_DIR))
            {
                Console.WriteLine("Missing folder!");
                return false;
            }

            if (!File.Exists(Globals.INPUT_FILE))
            {
                Console.WriteLine("Missing " + Globals.INPUT_FILE + "!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read all messages into an iterator
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ReadItems()
        {
            // Read all messages in the file
            return File.ReadLines(Globals.INPUT_FILE);
        }

        /// <summary>
        /// The HTTP API call method that's used to call all REST APIs.
        /// </summary>
        /// <param name="Uri"></param>
        /// <param name="Key"></param>
        /// <param name="Type"></param>
        /// <param name="AuthenticationLabel"></param>
        /// <param name="ContentType"></param>
        /// <param name="UrlParameter"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public static HttpResponseMessage CallAPI(string Uri, string Key, Globals.CallType Type,
                                                    string AuthenticationLabel, string ContentType,
                                                    string UrlParameter, string Body)
        {

            if (!String.IsNullOrEmpty(UrlParameter))
            {
                Uri += "?" + UrlParameter;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Uri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(ContentType));

            client.DefaultRequestHeaders.Add(AuthenticationLabel, Key);

            HttpResponseMessage response = null;

            if (Type == Globals.CallType.POST)
            {
                response = client.PostAsync(Uri, new StringContent(
                                   Body, System.Text.Encoding.UTF8, ContentType)).Result;
            }
            else if (Type == Globals.CallType.GET)
            {
                response = client.GetAsync(Uri).Result;
            }

            return response;
        }
    }
}
