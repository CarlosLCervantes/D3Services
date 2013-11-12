using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class RSSConfigSection : System.Configuration.ConfigurationSection 
{
    public static RSSConfigSection GetConfig()
    {
        return ConfigurationManager.GetSection("rssConfigGroup/rssConfig") as RSSConfigSection;
    }

    [ConfigurationProperty("sites")]
    public RSSConfigSitesCollection Sites
    {
        get
        {
            return this["sites"] as RSSConfigSitesCollection;
        }

    }
}

public class RSSConfigSite : System.Configuration.ConfigurationElement
{
    [ConfigurationProperty("contentURI", IsRequired = true)]
    public string ContentURI
    {
        get
        {
            return (string)this["contentURI"];
        }
        set
        {
            this["contentURI"] = value;
        }
    }

    [ConfigurationProperty("articleTypeID", IsRequired = true)]
    public int ArticleTypeID
    {
        get
        {
            return (int)this["articleTypeID"];
        }
        set
        {
            this["articleTypeID"] = value;
        }
    }

    [ConfigurationProperty("readerType", IsRequired = true)]
    public DiabloNewsRSSReaderTypes ReaderType
    {
        get
        {

            return (DiabloNewsRSSReaderTypes)this["readerType"];
        }
        set
        {
            this["remoteOnly"] = value;
        }
    }
}

public class RSSConfigSitesCollection : System.Configuration.ConfigurationElementCollection
{
    public RSSConfigSite this[int index]
    {
        get
        {
            return base.BaseGet(index) as RSSConfigSite;

        }
        set
        {
            if (base.BaseGet(index) != null)
            {
                base.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
        }

    }

    protected override ConfigurationElement CreateNewElement()
    {
        return new RSSConfigSite();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((RSSConfigSite)element).ContentURI.GetHashCode();
    }
}