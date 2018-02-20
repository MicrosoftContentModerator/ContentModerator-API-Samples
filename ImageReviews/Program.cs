using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace ImageReviews
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check for folder and file existence
            if (Helpers.FolderAndFilesExist())
            {
                // Initialize a review creation request object
                ReviewCreationRequest rcr = new ReviewCreationRequest();

                // Iterate through all moderated JSONs...
                for (int i = 1; i <= Globals.REVIEWS; i++)
                {
                    // Read pre-generated moderation response into the corresponding object
                    ImageModerationResponse imr = ReadImageModerationResponse(i);

                    // Prepare review creation request for this item
                    CreateImageReviewItem(i, imr, ref rcr);

                    Console.WriteLine();
                }

                // Convert to the review creation request JSON and submit to the API
                CreateAllReviews(rcr);
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        static ImageModerationResponse ReadImageModerationResponse(int Index)
        {
            string ModeratedJSONFile = Globals.MODERATED_JSONFILE + Index.ToString() + ".json";

            Console.WriteLine("Reading moderation insights from: " + ModeratedJSONFile);

            string ModerationResponse = File.ReadAllText(ModeratedJSONFile);

            ImageModerationResponse imr = JsonConvert.DeserializeObject<ImageModerationResponse>(ModerationResponse);
            return imr;
        }

        static void CreateImageReviewItem (int Index, ImageModerationResponse imr, ref ReviewCreationRequest rcr)
        {
            string ModeratedImageUrl = Globals.IMAGE_URL + Index.ToString() + ".jpg";

            Console.WriteLine("Creating review creation request for: " + ModeratedImageUrl);
 
            rcr.Item[Index - 1].Content = ModeratedImageUrl;
            rcr.Item[Index - 1].ContentId = Index.ToString();
            rcr.Item[Index - 1].SetKeyValue(0, "a", imr.IsImageAdultClassified.ToString().ToLower());
            rcr.Item[Index - 1].SetKeyValue(1, "adultScore", imr.AdultClassificationScore.ToString());
            rcr.Item[Index - 1].SetKeyValue(2, "r", imr.IsImageRacyClassified.ToString().ToLower());
            rcr.Item[Index - 1].SetKeyValue(3, "racyScore", imr.RacyClassificationScore.ToString());
        }

        static int CreateAllReviews(ReviewCreationRequest rcr)
        {
            Console.WriteLine("Submitting review creation request for all images.");

            string ReviewCreationRequestJSON = JsonConvert.SerializeObject(rcr.Item);
            HttpResponseMessage response = Helpers.CallAPI(Globals.APIURI, Globals.CONTENTMODERATOR_APIKEY,
                                            Globals.CallType.POST, "Ocp-Apim-Subscription-Key",
                                            "application/json", "", ReviewCreationRequestJSON);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Writing the review creation request JSON to file.");
                File.WriteAllText(Globals.REVIEW_REQUEST_CREATION_JSONFILE, ReviewCreationRequestJSON);
            }

            return (int)response.StatusCode;
        }
    }

    
}
