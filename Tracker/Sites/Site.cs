using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tracker.TrackerEssentials;

namespace Tracker.Sites
{
    public abstract class Site
    {
        public List<Link> links = new List<Link>();
        public HttpClient client = new HttpClient();
        public abstract void GetLinks();
    }
}
