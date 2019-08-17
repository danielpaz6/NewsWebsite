using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace NewsWebsite.Models
{
    public class GetNews
    {
        public List<string[]> Add_CNN_News()
        {
            string url = "http://rss.cnn.com/rss/edition.rss";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feeds = SyndicationFeed.Load(reader); // References -> Right Click -> Add Reference -> System.ServiceModel
            reader.Close();
            List<String[]> lst = new System.Collections.Generic.List<string[]>();
            var ns = (XNamespace)"http://search.yahoo.com/mrss/";

            bool flag = false;
            foreach (SyndicationItem item in feeds.Items)
            {
                String[] temp = new String[4];
                if (item.Summary == null)
                    continue;

                string subject = item.Title.Text;
                string summary = item.Summary.Text;
                string link = item.Links[0].Uri.ToString();

                var urls = from ext in item.ElementExtensions  // all extensions to ext
                           where ext.OuterName == "group" &&    // find the ones called group
                                 ext.OuterNamespace == ns       // in the right namespace
                           from content in ext.GetObject<XElement>().Elements(ns + "content") // get content elements
                           where (string)content.Attribute("medium") == "image"  // if that medium is an image
                           select (string)content.Attribute("url");

                if (urls.Count() < 5)
                    continue;

                Random rnd = new Random();

                string img = "";
                if (rnd.Next(1, 5) != 3)
                {
                    img = urls.ToArray()[3];
                    flag = false;
                }
                else
                {
                    if (flag == false)
                    {
                        img = urls.ToArray()[6];
                        flag = true;
                    }
                    else
                    {
                        img = urls.ToArray()[3];
                        flag = false;
                    }
                }

                temp[0] = subject;
                temp[1] = summary;
                temp[2] = link;
                temp[3] = img;

                lst.Add(temp);
            }
            return lst;
        }

        public List<string[]> Add_Ynet_News()
        {
            string url = "http://www.ynet.co.il/Integration/StoryRss3082.xml";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feeds = SyndicationFeed.Load(reader); // References -> Right Click -> Add Reference -> System.ServiceModel
            reader.Close();
            var ns = (XNamespace)"http://search.yahoo.com/mrss/";
            List<String[]> lst = new System.Collections.Generic.List<string[]>();

            foreach (SyndicationItem item in feeds.Items)
            {
                String[] temp = new String[4];

                if (item.Summary == null)
                    continue;

                string subject = item.Title.Text;
                string summary = item.Summary.Text;
                string link = item.Links[0].Uri.ToString();
                string img = null;
                string desc = null;
                string pattern1 = "src='([^']*)'";

                desc = summary.Split(new string[] { "</div>" }, StringSplitOptions.None)[1];
                RegexOptions options = RegexOptions.Multiline;

                foreach (Match m in Regex.Matches(summary, pattern1, options))
                {
                    img = m.Groups[1].ToString();
                }

                temp[0] = subject;
                temp[1] = desc;
                temp[2] = link;
                temp[3] = img;

                lst.Add(temp);
            }
            return lst;
        }

        public List<string[]> Add_FOX_News()
        {
            string url = "http://feeds.foxnews.com/foxnews/latest";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feeds = SyndicationFeed.Load(reader); // References -> Right Click -> Add Reference -> System.ServiceModel
            reader.Close();
            List<String[]> lst = new System.Collections.Generic.List<string[]>();
            var ns = (XNamespace)"http://search.yahoo.com/mrss/";
            foreach (SyndicationItem item in feeds.Items)
            {
                String[] temp = new String[4];
                if (item.Summary == null)
                    continue;

                string subject = item.Title.Text;
                string summary = item.Summary.Text;
                string link = item.Links[0].Uri.ToString();

                var urls = from ext in item.ElementExtensions  // all extensions to ext
                           where ext.OuterName == "group" &&    // find the ones called group
                                 ext.OuterNamespace == ns       // in the right namespace
                           from content in ext.GetObject<XElement>().Elements(ns + "content") // get content elements
                           where (string)content.Attribute("medium") == "image"  // if that medium is an image
                           select (string)content.Attribute("url");

                string img = urls.ToArray()[0];

                temp[0] = subject;
                temp[1] = summary;
                temp[2] = link;
                temp[3] = img;

                lst.Add(temp);
            }
            return lst;
        }
    }
}