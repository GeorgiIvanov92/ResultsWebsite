using System;
using System.Collections.Generic;
using System.Text;
using static RabbitMQ.TrackerEssentials.Communication.Sports;

namespace RabbitMQ.TrackerEssentials
{
    public class Link
    {
        public SportEnum Sport;
        public Uri Uri;
        public string AdditionalData;
        public string LeagueName;
        public int MapNumber;
        public int BestOf;
        public int ScoreHome;
        public int ScoreAway;
        public Link(SportEnum sport, Uri uri, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
        }
        public Link(SportEnum sport, Uri uri, string leagueName, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
            LeagueName = leagueName;
        }
        public Link(SportEnum sport, Uri uri, string leagueName, int mapNumber, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
            LeagueName = leagueName;
            MapNumber = mapNumber;
        }
        public Link(SportEnum sport, Uri uri, string leagueName, int mapNumber,int bestOf, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
            LeagueName = leagueName;
            MapNumber = mapNumber;
            BestOf = bestOf;
        }
        public Link(SportEnum sport, Uri uri, string leagueName, int mapNumber, int bestOf,int scoreHome, int scoreAway, string additionaldata = null)
        {
            Sport = sport;
            Uri = uri;
            AdditionalData = additionaldata;
            LeagueName = leagueName;
            MapNumber = mapNumber;
            BestOf = bestOf;
            ScoreHome = scoreHome;
            ScoreAway = scoreAway;
        }
    }
}
