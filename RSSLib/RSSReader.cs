using System;
using System.Linq;
using System.Xml;
using System.ServiceModel.Syndication;


public class RSSReader
{
    private string _contentURI;
    public RSSReader(string contentURI)
    {
        _contentURI = contentURI;
    }


    public SyndicationFeed GetContent()
    {
        XmlReaderSettings xrs = new XmlReaderSettings();

        XmlReader reader = XmlReader.Create(_contentURI, xrs);

        SyndicationFeed feed = SyndicationFeed.Load(reader);

        return feed;
    }
}