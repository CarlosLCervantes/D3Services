using System;
using System.Linq;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections;
using System.Collections.Generic;
using DAL;


public class DiabloNewsRSSConsumer
{
    public delegate void ConsumeDone(List<News> news);
    public event ConsumeDone OnConsumeDone;

    private DialoNewsDBDataContext db;

    public List<RSSContentSite> RSSContentSites { get; set; }
    private int _cutOffDateInHours;

    public DiabloNewsRSSConsumer()
    {
        _cutOffDateInHours = System.Configuration.ConfigurationManager.AppSettings["CutOffDateInHours"].ToIntegerOrDefault(24);
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conStr"].ToString();
        db = new DialoNewsDBDataContext(connectionString);
        RSSContentSites = GetRSSContentSites();
    }

    public void Consume()
    {
        foreach (RSSContentSite site in RSSContentSites)
        {
            DiabloNewsRSSReader reader = GetReader(site.ReaderType, site.ContentURI, site.ArticleTypeID);

            if (reader != null)
            {
                List<News> news = reader.GetContent(_cutOffDateInHours);

                FilterNews(ref news);

                db.News.InsertAllOnSubmit(news);
                db.SubmitChanges();

                if(OnConsumeDone != null)
                    OnConsumeDone(news);
            }
        }
    }

    /// <summary>
    /// This method is used to filter out only news that is new and does not already exist in the DB.
    /// </summary>
    /// <param name="news"></param>
    private void FilterNews(ref List<News> news)
    {
        //All items from DB or RSS will not be loaded if they are older than this. This is to create smaller data sets to process.
        
        DateTime cutOffDate = DateTime.Now.Subtract(new TimeSpan(_cutOffDateInHours, 0, 0));

        var dbNewsSet = from n in db.News
                        where n.DateTime > cutOffDate
                     select n;

        news = (from rssNews in news
                where !dbNewsSet.Any(d => d.Subject.ToLower().Trim() == rssNews.Subject.ToLower().Trim())
                && rssNews.DateTime > cutOffDate
                select rssNews).ToList();

        //var newsThatAlreadyExists = (from rssNews in news
        //                            join dbNews in dbNewsSet on rssNews.Subject equals dbNews.Subject into newsJoin
        //                            from derp in newsJoin.DefaultIfEmpty()
        //                            select derp).ToList();

    }

    private DiabloNewsRSSReader GetReader(DiabloNewsRSSReaderTypes readerType, string contentURI, int articleType)
    {
        DiabloNewsRSSReader reader = null;

        if (readerType == DiabloNewsRSSReaderTypes.SIMPLE)
        {
            reader = new DiabloNewsRSSReader_Simple(contentURI, articleType);
        }
        else if (readerType == DiabloNewsRSSReaderTypes.DIABLO_FANS)
        {
            reader = new DiabloNewsRSSReader_DialboFans(contentURI, articleType);
        }
        else if (readerType == DiabloNewsRSSReaderTypes.INC_GAMERS)
        {
            reader = new DiabloNewsRSSReader_DiabloIncGamers(contentURI, articleType);
        }

        return reader;
    }

    private List<RSSContentSite> GetRSSContentSites()
    {
        List<RSSContentSite> contentSites = new List<RSSContentSite>();
        //contentSites.Add(new RSSContentSite() { ReaderType = DiabloNewsRSSReaderTypes.SIMPLE, ContentURI = @"http://us.battle.net/d3/en/feed/news" });
        //RSSConfigSection configSection = (RSSConfigSection)System.Configuration.ConfigurationManager.GetSection(@"rssConfigGroup/ConfigGroup");
        //System.Configuration.ConfigurationElementCollection rssConfig = (System.Configuration.ConfigurationElementCollection)configSection;
        RSSConfigSection rssConfig = RSSConfigSection.GetConfig();
        foreach (RSSConfigSite s in rssConfig.Sites)
        {
            RSSContentSite site = new RSSContentSite(s.ReaderType, s.ContentURI, s.ArticleTypeID);
            contentSites.Add(site);
        }
        

        return contentSites;
    }
}

public class RSSContentSite
{
    public DiabloNewsRSSReaderTypes ReaderType { get; set; }
    public string ContentURI { get; set; }
    public int ArticleTypeID { get; set; }

    public RSSContentSite()
    {

    }

    public RSSContentSite(DiabloNewsRSSReaderTypes readerType, string contentURI, int articleTypeID)
    {
        ReaderType = readerType;
        ContentURI = contentURI;
        ArticleTypeID = articleTypeID;
    }

    //private DiabloNewsRSSReaderTypes GetReaderType(string readerTypeString)
    //{
    //    return DiabloNewsRSSReaderTypes.SIMPLE;
    //}
}

public enum DiabloNewsRSSReaderTypes {SIMPLE, INC_GAMERS, DIABLO_FANS}