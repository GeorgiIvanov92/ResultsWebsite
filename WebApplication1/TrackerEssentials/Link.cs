using System;
using System.Collections.Generic;
using System.Text;
using static Tracker.TrackerEssentials.Communication.Sports;

namespace Tracker.TrackerEssentials
{
    public class Link
    {
        public SportEnum Sport;
        public Uri Uri;
        public string AdditionalData;
        public Link(SportEnum sport, Uri uri, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
        }
    }
}
