using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageModeration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check for folder and file existence
            if (Helpers.FolderAndFileExist())
            {
                // Moderate...
               ModerateImages(Helpers.ReadItems());
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Go moderate all images
        /// </summary>
        /// <param name="ImageUrls"></param>
        public static void ModerateImages(IEnumerable<string> ImageUrls)
        {
            int Index = 1;
            int CallStatus = 0;
            Console.WriteLine("Moderating...");

            // for each item in the file...
            foreach (string ImageUrl in ImageUrls)
            {
                CallStatus = ModerateImage(ImageUrl, Index);

                // Screen text 
                if (CallStatus != Globals.CALLSTATUSOK)
                {
                    if (CallStatus == Globals.CALLRATEEXCEEDED)
                    {
                        // Slow down
                        Thread.Sleep(Globals.CALLWAITTIME);
                    }
                    if (CallStatus == Globals.CALLVOLUMEEXCEEDED)
                    {
                        // Stop!
                        break;
                    }
                }
                Index++;
            }
            Console.WriteLine("Done.");
        }

        /// <summary>
        /// Moderate image using the Image moderation API
        /// </summary>
        /// <param name="Image URL"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static int ModerateImage(string ImageUrl, int Index)
        {
            string ResponseJSON = string.Empty;
            string Body = $"{{\"DataRepresentation\":\"URL\",\"Value\":\"{ImageUrl}\"}}";

            Console.WriteLine("Moderating image: " + ImageUrl);

            HttpResponseMessage response =
                Helpers.CallAPI(Globals.APIURI, Globals.CONTENTMODERATOR_APIKEY, Globals.CallType.POST,
                                                   "Ocp-Apim-Subscription-Key", "application/json",
                                                   string.Empty,Body);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Saving the moderation response.");

                ResponseJSON = response.Content.ReadAsStringAsync().Result;

                File.WriteAllText(Globals.OUTPUT_JSONFILE + Index.ToString() + ".json", ResponseJSON);
            }

            return (int)response.StatusCode;
        }


    }
}

