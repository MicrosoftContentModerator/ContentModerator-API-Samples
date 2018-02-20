using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageReviews
{
    public class ImageModerationResponse
    {
        public float AdultClassificationScore { get; set; }
        public bool IsImageAdultClassified { get; set; }
        public float RacyClassificationScore { get; set; }
        public bool IsImageRacyClassified { get; set; }
        public Advancedinfo[] AdvancedInfo { get; set; }
        public bool Result { get; set; }
        public Status Status { get; set; }
        public string TrackingId { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public object Exception { get; set; }
    }

    public class Advancedinfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
