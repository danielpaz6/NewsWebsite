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

Another nice feature is that when the user scrolls across 80% of the page, more articles are opened to him, which is done via AJAX.

## Users & Login system

In order to build the user system part of the site, we used ASP.NET Identity where the system came with a basic user interface in which we added additional fields like permissions and articles of the same user and more.
Moreover, we implemented a nice popup window once you clicked on the "login" panel ( via jQuery )
and used ```Google API``` and ```Facebook API``` in order to make more ways to login to the website. Example:

<p align="center">
  <img src="/src/login.png" width="400">
</p>

