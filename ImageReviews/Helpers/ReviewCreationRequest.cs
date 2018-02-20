using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageReviews
{
    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Content Moderator Image Review Creation Request Object
    /// </summary>
    public class ReviewCreationRequest
    {
        public ReviewItem[] Item { get; set; }

        public ReviewCreationRequest()
        {
            Item = new ReviewItem[Globals.REVIEWS];
            for (int i=0; i< Globals.REVIEWS; i++)
            {
                Item[i] = new ReviewItem();
                Item[i].Metadata = new KeyValuePair[Globals.REVIEW_TAGS_KEYVALUE_PAIRS];

                for (int j=0; j<Globals.REVIEW_TAGS_KEYVALUE_PAIRS; j++)
                {
                    Item[i].Metadata[j] = new KeyValuePair();
                }
            }
        }
    }

    public class ReviewItem
    {
        public string Content { get; set; }
        public string ContentId { get; set; }
        public KeyValuePair[] Metadata { get; set; }
        public string Type { get; set; }
        public string CallbackEndpoint { get; set; }

        public ReviewItem()
        {
            ContentId = "1";
            Type = "Image";
            CallbackEndpoint = "";
        }

        public void SetKeyValue(int Index, string Label, string Value)
        {
            Metadata[Index].Key = Label;
            Metadata[Index].Value = Value;
        }
    }

    public class KeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
