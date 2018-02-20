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

namespace TextModeration
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Check for folder and file existence
            if (Helpers.FolderAndFileExist())
            {
                // Moderate...
                ModerateTexts(Helpers.ReadItems());
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Go moderate all text
        /// </summary>
        /// <param name="Texts"></param>
        public static void ModerateTexts(IEnumerable<string> Texts)
        {
            int Index = 1;
            int CallStatus = 0;
            Console.WriteLine("Moderating...");

            // for each item in the file...
            foreach (string TextPart in Texts)
            {
                ModerateText(TextPart, Index);

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
        /// Moderate text by using the API
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static int ModerateText(string Text, int Index)
        {
            string ResponseJSON = string.Empty;
            string ParamsEng = "&classify=true&PII=true&autocorrect=true";

            Console.WriteLine("Moderating text: " + Text);

            HttpResponseMessage response =
                Helpers.CallAPI(Globals.APIURI, Globals.CONTENTMODERATOR_APIKEY, Globals.CallType.POST,
                                                   "Ocp-Apim-Subscription-Key", "text/plain",
                                                   ParamsEng, Text);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Saving moderation response.");

                ResponseJSON = response.Content.ReadAsStringAsync().Result;

                File.WriteAllText(Globals.OUTPUT_JSONFILE+Index.ToString()+".json", ResponseJSON, Encoding.UTF8);
            }

            return (int)response.StatusCode;
        }
    }
}

