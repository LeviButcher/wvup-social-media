using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WVUPSM.MVC.Models;

namespace WVUPSM.MVC.Service
{
    public class RSSFeedService
    {
        XmlDocument xmlDocument = new XmlDocument();

        public RSSFeedService()
        {


        }
        /// <summary>
        ///     RSS Feed will be parsed, and a list of RSSFeedItemViewModels returned
        ///
        ///     Basic knowledge to write method taken from here: https://stackoverflow.com/questions/11097750/xml-reader-threw-object-null-exception-but-node-exists
        /// </summary>
        /// <param name="url">The url of an RSS feed to parse</param>
        /// <returns></returns>
        public List<RssFeedItemViewModel> ParseRssDoc(string url)
        {
            xmlDocument.Load(url);

            // Solution for dc prefix taken from here: https://stackoverflow.com/questions/4633127/how-to-select-xml-nodes-with-xml-namespaces-from-an-xmldocument
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            // Solution for media prefix taken from here: https://forums.asp.net/t/1256876.aspx?Reading+Enclosure+values+and+media+thumbnail+values
            nsmgr.AddNamespace("media", "http://search.yahoo.com/mrss/");

            XmlNodeList rssNodes = xmlDocument.SelectNodes("rss/channel/item");

            List<RssFeedItemViewModel> rssFeedItemViewModels = new List<RssFeedItemViewModel>();

            foreach (XmlNode rssNode in rssNodes)
            {
                RssFeedItemViewModel rssFeedItemViewModel = new RssFeedItemViewModel();

                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                rssFeedItemViewModel.Title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                rssFeedItemViewModel.Link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                rssFeedItemViewModel.Description = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("dc:creator", nsmgr);
                rssFeedItemViewModel.Author = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("pubDate");
                var date = rssSubNode != null ? rssSubNode.InnerText : "";
                rssFeedItemViewModel.PubDate = date.Remove(16);

                rssSubNode = rssNode.SelectSingleNode("media:thumbnail/@url", nsmgr);
                rssFeedItemViewModel.ThumbnailUrl = rssSubNode != null ? rssSubNode.InnerText : "";

                rssFeedItemViewModels.Add(rssFeedItemViewModel);

            }

            return rssFeedItemViewModels;
        }
    }
}
