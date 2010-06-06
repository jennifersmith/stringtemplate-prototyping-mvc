stringtemplate-prototyping-mvc
==============================

A very simple ASP.NET MVC application allowing you to write site prototypes using string template.

Requirements
------------

* ASP.NET MVC 2.0

Usage
-----

Open and run in Visual Studio 2005 or greater. Hit Ctrl-f5 to start an instance without the debugger.

To begin creating pages, add folders and string template files into the Views folder. The path of the folder relative to the Views folder is what you can use to browse the site.
e.g. given a site running at localhost:2020 and a view template at Views/Foo/bar.st, go to localhost:2020/Foo/bar to see it in action.

See the sample page (/sample/home/index) for a better example.

Further info
------------
Visit the [StringTemplate site](http://www.antlr.org/wiki/display/ST/StringTemplate+cheat+sheet) for more information on what you can do with StringTemplate.

Todo
----
* Figure out steps to install in IIS
* Allow different template folders? (via config)
* Generic view data?
* Better documentation

