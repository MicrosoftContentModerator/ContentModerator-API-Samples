using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoModeration
{
    class Globals
    {
        // declare constants and globals
        public static CloudMediaContext _context = null;

        // Azure Media Services authentication
        public const string AZURE_AD_TENANT_NAME = "microsoft.onmicrosoft.com";
        public const string CLIENT_ID = Secrets.CLIENT_ID;
        public const string CLIENT_SECRET = Secrets.CLIENT_SECRET;

        // REST API endpoint, for example "https://accountname.restv2.westcentralus.media.azure.net/API".      
        public const string REST_API_ENDPOINT = Secrets.REST_API_ENDPOINT;

        // Content Moderator Media Processor Name
        public const string MEDIA_PROCESSOR = "Azure Media Content Moderator";

        public const string TOP_DIR = @"C:\Webinar\ContentModerator-API-Samples\VideoModeration\Sample files\";
        public const string OUTPUT_FOLDER = TOP_DIR;
        public const string INPUT_FILE = TOP_DIR + "windows10.mp4";

        //a configuration file in the json format with the version number, also in the current directory
        public static readonly string CONTENT_MODERATOR_PRESET_FILE = "preset.json";
        //Example file:
        //        {
        //             "version": "2.0"
        //        }

    }
}
