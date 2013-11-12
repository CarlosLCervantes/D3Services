using System;
using System.Linq;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections;
using System.Collections.Generic;


public class DiabloNewsRSSConsumer
{
    public List<RSSContentSite> RSSContentSites { get; set; }

    public DiabloNewsRSSConsumer()
    {
        RSSContentSites = GetRSSContentSites();
    }

    public void Consume()
    {
        int d = "";
    }

    private List<RSSContentSite> GetRSSContentSites()
    {
        List<RSSContentSite> contentSites = new List<RSSContentSite>();
        return contentSites;
    }
}

public class RSSContentSite
{
    public DiabloNewsRSSReaderTypes ReaderType { get; set; }
    public string ContentURI { get; set; }

    public RSSContentSite()
    {

    }

    private DiabloNewsRSSReaderTypes GetReaderType(string readerTypeString)
    {

        return DiabloNewsRSSReaderTypes.SIMPLE;
    }
}

public enum DiabloNewsRSSReaderTypes {SIMPLE, INC_GAMERS, DIABLO_FANS}