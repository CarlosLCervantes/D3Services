using System;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using DAL;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using System.Net;


public abstract class DiabloNewsRSSReader
{
    protected RSSReader reader;
    protected int articleTypeID;
    public DiabloNewsRSSReader(string contentURI, int rssArticleTypeID)
    {
        reader = new RSSReader(contentURI);
        articleTypeID = rssArticleTypeID;
    }

    public abstract List<News> GetContent(int cutOffDateInHours);

    protected void RemoveAllImageInlineStyles(ref HtmlDocument doc)
    {
        //Remove all inline styles from images in the rss feed.
        if (doc.DocumentNode.InnerHtml.Contains("img"))
        {
            foreach (HtmlNode img in doc.DocumentNode.Descendants(@"img"))
            {
                HtmlAttribute attr = img.Attributes["style"];
                if (attr != null)
                {
                    attr.Remove();
                }
            }
        }
    }

    protected void FixRelativeImages(ref HtmlDocument doc, string hostURIPart)
    {
        if (doc.DocumentNode.InnerHtml.Contains("img"))
        {
            foreach (HtmlNode img in doc.DocumentNode.Descendants(@"img"))
            {
                HtmlAttribute attr = img.Attributes["src"];
                if (attr != null && !attr.Value.Contains("http://"))
                {
                    attr.Value = string.Format(@"http://{0}/{1}", hostURIPart, attr.Value);
                }
            }
        }
    }

    protected DateTime GetArticleDT(SyndicationItem i)
    {
        DateTime articleDT = i.PublishDate.DateTime;
        if (articleDT.Hour == 0 && DateTime.Now.Hour != 0)
        {
            articleDT.AddHours(DateTime.Now.Hour);
            articleDT.AddMinutes(DateTime.Now.Minute);
            articleDT.AddSeconds(DateTime.Now.Second);
        }
        return articleDT;
    }
}

public class DiabloNewsRSSReader_Simple : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_Simple(string uri, int rssArticleTypeID)
        : base(uri, rssArticleTypeID)
    {

    }

    public override List<News> GetContent(int cutOffDateInHours)
    {
        SyndicationFeed feed = reader.GetContent();

        List<News> newsItems = new List<News>();
        foreach (SyndicationItem i in feed.Items)
        {
            if(i.PublishDate.Date > DateTime.Now.Date.Subtract(new TimeSpan(cutOffDateInHours, 0, 0)))
            {
                News newsItem = new News();
                newsItem.ArticleTypeID = articleTypeID;
                newsItem.ImageType = articleTypeID;
                newsItem.Subject = i.Title.Text;
                StringBuilder contentString = new StringBuilder(((System.ServiceModel.Syndication.TextSyndicationContent)(i.Content)).Text);
            
                //Insert wrapper that marks all content as from an RSS and not internal generated
                contentString.Insert(0, String.Format("<div class=\"rssContent readerType_{0}\">", articleTypeID));
                contentString.Append(String.Format("<p><em>Originally posted on the Diablo 3 Community Site</em></p>"));
                contentString.Append("</div>");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contentString.ToString());
                base.RemoveAllImageInlineStyles(ref doc);

                string fullyFormarttedContentString = doc.DocumentNode.OuterHtml;
                newsItem.Content = fullyFormarttedContentString;
                newsItem.DateTime = GetArticleDT(i);
                newsItems.Add(newsItem);
            }
        }

        return newsItems;
    }
}

public class DiabloNewsRSSReader_DialboFans : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_DialboFans(string uri, int rssArticleTypeID)
        : base(uri, rssArticleTypeID)
    {
    }


    public override List<News> GetContent(int cutOffDateInHours)
    {
        SyndicationFeed feed = reader.GetContent();

        List<News> newsItems = new List<News>();
        foreach (SyndicationItem i in feed.Items)
        {
            if (i.PublishDate.Date > DateTime.Now.Date.Subtract(new TimeSpan(cutOffDateInHours, 0, 0)))
            {
                News newsItem = new News();
                newsItem.ArticleTypeID = articleTypeID;
                newsItem.ImageType = articleTypeID;
                newsItem.Subject = i.Title.Text;
                StringBuilder contentString = new StringBuilder(i.Summary.Text);

                //Insert wrapper that marks all content as from an RSS and not internal generated
                contentString.Insert(0, String.Format("<div class=\"rssContent readerType_{0}\">", articleTypeID));
                contentString.Append(String.Format("<p><em><a href=\"{0}\">Originally posted on DiabloFans.com</a></em></p>", i.Id));
                contentString.Append("</div>");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contentString.ToString());
                base.RemoveAllImageInlineStyles(ref doc);
                base.FixRelativeImages(ref doc, "www.diablofans.com");
           

                string fullyFormarttedContentString = doc.DocumentNode.OuterHtml;
                newsItem.Content = fullyFormarttedContentString;

                newsItem.DateTime = GetArticleDT(i);
                newsItems.Add(newsItem);
            }
        }

        return newsItems;
    }
}

public class DiabloNewsRSSReader_DiabloIncGamers : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_DiabloIncGamers(string uri, int rssArticleTypeID)
        : base(uri, rssArticleTypeID)
    {

    }

    public override List<News> GetContent(int cutOffDateInHours)
    {
        SyndicationFeed feed = reader.GetContent();

        List<News> newsItems = new List<News>();
        foreach (SyndicationItem i in feed.Items)
        {
            if (i.PublishDate.Date > DateTime.Now.Date.Subtract(new TimeSpan(cutOffDateInHours, 0, 0)))
            {
                News newsItem = new News();
                newsItem.ArticleTypeID = articleTypeID;
                newsItem.ImageType = articleTypeID;
                newsItem.Subject = i.Title.Text;
                StringBuilder contentString = GetHTMLString(i.Id);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contentString.ToString());
                HtmlNode contentNode = null;
                try
                {
                    //contentNode = doc.DocumentNode.Descendants(@"div[@class='post_container']").FirstOrDefault();
                    contentNode = (from node in doc.DocumentNode.Descendants("div")
                                   where node.Attributes["class"] != null && node.Attributes["class"].Value.ToLower() == "post_container"
                                   select node).FirstOrDefault();

                    contentNode = (from node in contentNode.Descendants("div")
                                   where node.Attributes["class"] != null && node.Attributes["class"].Value.ToLower().StartsWith("post-") && node.Attributes["id"] == null
                                   select node).FirstOrDefault();

                    //Kill all header elements, up until the first p tag
                    //TODO fix collection modified
                    int positionOfFirstPTag = 0;
                    foreach (HtmlNode node in contentNode.Descendants())
                    {
                        if (node.Name == "p")
                        {
                            positionOfFirstPTag++;
                            break;
                        }
                    }

                    for (int b = positionOfFirstPTag; b > 0; b--)
                    {
                        contentNode.Descendants().ElementAt(b).Remove();
                    }
                }
                catch (Exception ex)
                {
                    contentNode = null;
                    System.Diagnostics.EventLog.WriteEntry("D3RssPollingSrvc", "Could Not Parse News Article at " + i.Id + " Probably the format changed.",
                        System.Diagnostics.EventLogEntryType.Error);
                }

                if (contentNode != null)
                {

                    doc.LoadHtml(contentNode.OuterHtml);
                    //Insert wrapper that marks all content as from an RSS and not internal generated
                    contentString.Insert(0, String.Format("<div class=\"rssContent readerType_{0}\">", articleTypeID));
                    contentString.Append(String.Format("<p><em><a href=\"{0}\">Originally posted on Diablo.IncGamers.com</a></em></p>", i.Id));
                    contentString.Append("</div>");

                    base.RemoveAllImageInlineStyles(ref doc);
                    base.FixRelativeImages(ref doc, "diablo.incgamers.com");

                    string fullyFormarttedContentString = doc.DocumentNode.OuterHtml;
                    newsItem.Content = fullyFormarttedContentString;

                    newsItem.DateTime = GetArticleDT(i);
                    newsItems.Add(newsItem);
                }
            }
        }

        return newsItems;
    }

    public StringBuilder GetHTMLString(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        
        StringBuilder sb = new StringBuilder();
        byte[] buf = new byte[8192];
        Stream resStream = response.GetResponseStream();
        string tempString = null;
        int count = 0;
        do
        {
            // fill the buffer with data
            count = resStream.Read(buf, 0, buf.Length);

            // make sure we read some data
            if (count != 0)
            {
                // translate from bytes to ASCII text
                tempString = Encoding.ASCII.GetString(buf, 0, count);

                // continue building the string
                sb.Append(tempString);
            }
        }
        while (count > 0); // any more data to read?

        return sb;
    }
}

