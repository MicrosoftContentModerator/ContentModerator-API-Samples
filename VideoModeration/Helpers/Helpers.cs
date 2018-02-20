using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace VideoModeration
{
    class Helpers
    {

        /// <summary>
        /// Creates a media context from azure credentials
        /// </summary>
        public static void CreateMediaContext()
        {
            // Get Azure AD credentials
            var tokenCredentials = new AzureAdTokenCredentials(Globals.AZURE_AD_TENANT_NAME,
                       new AzureAdClientSymmetricKey(Globals.CLIENT_ID, Globals.CLIENT_SECRET),
                       AzureEnvironments.AzureCloudEnvironment);

            // Initialize an Azure AD token
            var tokenProvider = new AzureAdTokenProvider(tokenCredentials);

            // Create a media context
            Globals._context = new CloudMediaContext(new Uri(Globals.REST_API_ENDPOINT), tokenProvider);
        }

        /// <summary>
        /// Creates an Azure Media Services Asset from the video file
        /// </summary>
        /// <returns>Asset</returns>
        public static IAsset CreateAssetfromFile()
        {
            return Globals._context.Assets.CreateFromFile(Globals.INPUT_FILE, AssetCreationOptions.None); ;
        }
    }
}
