using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageModeration
{
    class Secrets
    {
        //Content Moderator Key 
        public const string CONTENTMODERATOR_APIKEY = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_SUBSCRIPTION_KEY");
    }
}
