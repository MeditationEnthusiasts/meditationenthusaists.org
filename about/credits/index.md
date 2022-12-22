---
layout: page
title: Credits
description: "Third party libraries that make this site possible."
comments: false
tags: [cake, credits, csharp, dotnet, foss, open, source, third, party]
---

@using Pretzel.Logic.Templating.Context

This website was created with the help of several third-party libraries and services.  They are listed below.

_None_ of these libraries or services are affiliated with @(Model.Site.Config["title"]).

## Binary Theme

CSS Theme

* Website: [https://binarytheme.com/](https://binarytheme.com/)
* Theme: [https://binarytheme.com/bloggo-clean-personal-blog-html5-template-2/](https://binarytheme.com/bloggo-clean-personal-blog-html5-template-2/)
* Terms of Service: [https://binarytheme.com/terms-of-service/](https://binarytheme.com/terms-of-service/)

@Include( "credits.md", Model, typeof( PageContext ) )
@Include( "actpub_credits.md", Model, typeof( PageContext ) )
