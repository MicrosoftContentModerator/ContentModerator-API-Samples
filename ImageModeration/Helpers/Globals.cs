using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageModeration
{
    class Globals
    {
        public enum CallType { POST, GET };

        public const int CALLSTATUSOK = 200;
        public const int CALLVOLUMEEXCEEDED = 403;
        public const int CALLRATEEXCEEDED = 429;
        public const int CALLWAITTIME = 1000;

        public const string TOP_DIR = @"C:\Webinar\ContentModerator-API-Samples\ImageModeration\Sample files\";
        public const string INPUT_FILE = TOP_DIR + "input.txt";
        public const string OUTPUT_JSONFILE = TOP_DIR + "output";

        //Content Moderator Key 
        public const string CONTENTMODERATOR_APIKEY = Secrets.CONTENTMODERATOR_APIKEY;

        // All Uris
        public const string APIURI = "https://westus.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate";
    }
}
