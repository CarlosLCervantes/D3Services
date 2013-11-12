using System;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using DAL;


public abstract class DiabloNewsRSSReader
{
    protected RSSReader reader;
    public DiabloNewsRSSReader(string contentURI)
    {
        reader = new RSSReader(contentURI);
    }

    public abstract List<News> GetContent();
}

public class DiabloNewsRSSReader_Simple : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_Simple(string uri)
        : base(uri)
    {

    }

    public override List<News> GetContent()
    {
        return new List<News>();
    }
}

public class DiabloNewsRSSReader_DialboFans : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_DialboFans()
    {

    }


    public override List<News> GetContent()
    {
        return new List<News>();
    }
}

public class DiabloNewsRSSReader_DiabloIncGamers : DiabloNewsRSSReader
{
    public DiabloNewsRSSReader_DiabloIncGamers()
    {

    }


    public override List<News> GetContent()
    {
        return new List<News>();
    }
}

