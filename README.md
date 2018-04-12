# HttpWebServer
C# WebServer An embeddable and extensible web server that supports controllers, dynamic templates (which are recompiled in runtime if changed on disc), haml, multiple web sites, reverse proxying, http digest, http basic auth and more.

C# WebServer 
This project is not being supported anymore.You can either use my networking library Griffin.Networking with the HTTP protocol implementation to get something like the standard HttpListener. Both assemblies are available at nuget.You can also use the new Griffin.WebServer. Which is slowly taking shape. It's a lot faster than C# WebServer.
Current features in Griffin.WebServer
File handling
Caching
Partial downloads (Range support, i.e. continue downloads)
Authentication
Basic
Digest
Request body handling
multipart/form-data (to handle file uploads)
UrlEncoded (the most common body encoding)
Support any file size (large files are written to temporary files)
Response body handling
Allows streaming of large files with low memory usage. Just attach a FileStream etc.
Better module support
different types of modules which are executed in a specific order (authentication models are for instance always run first)
Regex routing module

Legacy information
Features in v1.1
HTTP Basic and Digest authentication - The authentication 
process is activated either by throwing the UnauthorizedException or 
tagging controller methods with a special attribute.
Controllers ("C" in MVC)
Template engines ("V" in MVC)
Multiple web sites module (serve multiple websites in same server).
HTTPS Support (SSL)
Multilingual applications
Validator - Validates input (got multilingual support)
Uniform input handling - Handle querystring/form/xml in the same way (you can add your own decoders too).
Helpers
Testing Simplified testing of controllers.

Features in v2.0
Version 2.0 is still a beta. Use v1.1 if you need a stable version.
MVC
External view engines (Spark, NHaml support included)
HTTPS
HttpFactory -> incject your own types to create a custom server.
Abstract logging system -> Very easy to add support for your 
favorite logging framework. Built in support for console logging.
Each header is parsed into an object -> Easier to handle header information.
Click on the documentation tab for examples.
Code documentation 
Extremely well-commented source code
C# Webserver is written mostly in C#.

Across all C# projects on Ohloh, 22% of all source code lines are comments. For C# Webserver, this figure is 37%.

This very impressive number of comments puts C# Webserver among the best 10% of all C# projects on Ohloh.

A high number of comments might indicate that the code is well-documented and organized, and could be a sign of a helpful and disciplined development team.

(the text is taken from ohloh)
