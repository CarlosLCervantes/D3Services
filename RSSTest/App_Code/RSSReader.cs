using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSTest
{
    public class RSSReader
    {
        private string _contentURI;
        public RSSReader(string contentURI)
        {
            _contentURI = contentURI;
        }


        protected SyndicationFeed GetContent()
        {
            XmlReaderSettings xrs = new XmlReaderSettings();

            XmlReader reader = XmlReader.Create(_contentURI, xrs);

            SyndicationFeed feed = SyndicationFeed.Load(reader);

            return feed;
        }

        public void JunkTestCode()
        {
            //string rssURI = @"http://diablo.incgamers.com/feed";
            //string rssURI = @"http://www.diablofans.com/rss/forums/1-diablo-fans-home-page-news/";
            //string rssURI = @"http://us.battle.net/d3/en/feed/news";

            //string testSummary = feed.Items.ToArray()[5].Summary.Text;
            //string testContent = feed.Items.ToArray()[5].Content.ToString();

            //foreach (SyndicationItem i in feed.Items)
            //{
            //    string derp = i.Content.ToString();
            //}

            //var test = 0;

        }


    }

}