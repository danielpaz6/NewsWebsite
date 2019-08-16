# Papor - RSS News Website

![Presentation Project](/src/mainview.png)

An ASP.NET project we made during the semester.

## Introduction

The idea of this site, unlike other news sites, is actually a site that groups several news sites together. And he does this through the RSS of those sites.
And with a smart statistical algorithm, he's able to channel the relevant articles at the top of the page.
In addition, this site supports Web Services, Google & Facebook API, weather API and many other technologies that will be listed below.

## Home Page

This is the main page of the site where you can see the various articles presented on the site.

As mentioned before, this site has statistical learning, that is, when a user login to the site and clicks on different articles, the system will save it and offer the same user relevant news stories of whatever he chooses.

If we go deeper, the learning is actually done in the following way:

Suppose there are 4 categories of news, for simplicity we will label them in the letters: A, B, C, D.

And suppose the total user enters the expressions (according to Group By):


| CategoryName | Count |
| :---: | :---: |
| A | 2 |
| B | 5 |
| C | 3 |

We'll mark as num1 the sum of A,B,C ( 2+5+3 )
That is, the user enters articles in categories A, B, C (we will mark this group as S)
We'll mark as num2 the sum of categories that not listed in group S.

So what we want to do in the article presentation, let's say that about 20 new articles are displayed on each page, and we label this number as ```ArticlesPerPage```.

So we want the following distribution articles to look like this:

<p align="center">
  <img src="/src/formula1.png">
</p>

It is important to note that we will multiply both 0.8 and 0.2 in ```ArticlesPerPage``` so that we get a proper ratio.

Another nice feature is that when the user scrolls across 50%-80% ( depends how many pages were already opened ) of the page, more articles are opened, which is done via AJAX.

## Users & Login system

In order to build the user system part of the site, we used ASP.NET Identity where the system came with a basic user interface in which we added additional fields like permissions and articles of the same user and more.
Moreover, we implemented a nice popup window once you clicked on the "login" panel ( via jQuery )
and used ```Google API``` and ```Facebook API``` in order to make more ways to login to the website. Example:


## Add RSS Articles Feature

The manager has 2 options for adding new articles to the site: either manually or using additional buttons to choose which RSS to add articles to and which category this belongs and how many to add, for example:


<p align="center">
  <img src="/src/rssadd.png" width="750">
</p>

And this was done by using many technologies such as ```XmlReader``` ```SyndicationFeed``` so we could read and interpret the RSS of the different news websites.
And by clicking on any of these buttons will lead to this code to active: (CNN RSS Example)

```csharp
string url = "http://rss.cnn.com/rss/edition.rss";
XmlReader reader = XmlReader.Create(url);
SyndicationFeed feeds = SyndicationFeed.Load(reader);
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
			   from content in ext.GetObject<XElement>().Elements(ns + "content")
			   where (string)content.Attribute("medium") == "image" // if that medium is an image
			   select (string)content.Attribute("url");

	if (urls.Count() < 5)
		continue;

	string img = urls.ToArray()[3];

	temp[0] = subject;
	temp[1] = summary;
	temp[2] = link;
	temp[3] = img;

	lst.Add(temp);
}
```

and in the end of the foreach, we have a list of all the articles so we could add them to our database.

## Statistics

The site also supports a page where statistics about the various users and articles are available so that the site administrators can view them.

<p align="center">
  <img src="/src/statistics.png">
</p>

And this is done by using d3js ( https://d3js.org/ ).


## Other technologies & features

* HTML5 including: Video, Canvas Aside, Footer, Header, Nav, Section and some more.
* CSS3 including: Text-shadow, Transition, Multiple-columns, Font-face, Border-radius.
* Facebook, Google API and some Weather API.
* Javascript, jQuery and Ajax.
* Advanced queries such as Group By and Join between several tables.
* Advanced Search filtering by several parameters.
* Implemented in MVC.

## Authors

**Daniel Paz** - *Part of the project program* - [Profile](https://github.com/DanielPaz6)<br />
**Opal Koren** - *Part of the project program* - [Profile](https://github.com/OpalKo93/)<br />
**Or Mizrahi** - *Part of the project program* - [Profile](https://github.com/OrMizrahi/)<br />
**Omer Nahum** - *Part of the project program* - [Profile](https://github.com/OmerNahum/)
