using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check for folder and file existence
            if (Helpers.FolderAndFilesExist())
            {
                // Iterate through all moderated JSONs...
                for (int i = 1; i <= Globals.REVIEWS; i++)
                {
                    // Start moderation job with workflowname = "showallreviews" or "showonlyracy"
                    StartImageJob(i, "showonlyracy");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        static int StartImageJob(int Index, string WorkflowName)
        {
            string ImageUrl = Globals.IMAGE_URL + Index.ToString() + ".jpg";
            string ImageBody = "{ \"ContentValue\": \"" + ImageUrl + "\"" + " }";

            string Parameter = "ContentType=Image&ContentId=" + Index.ToString() + "&WorkflowName=" + WorkflowName;

            Console.WriteLine("Creating moderation job for image: " + ImageUrl);

            HttpResponseMessage response = Helpers.CallAPI(Globals.APIURI, Globals.CONTENTMODERATOR_APIKEY,
                                            Globals.CallType.POST, "Ocp-Apim-Subscription-Key",
                                            "application/json", Parameter, ImageBody);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Job created. Saving job request body.");
                File.WriteAllText(Globals.JOB_CREATION_REQUEST_JSONFILE, ImageBody);
            }

            return (int)response.StatusCode;
        }


    }
}
