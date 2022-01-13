# CLEVAR Web Project Readme

## Development Environment and Platform
The website is developed with ASP.net Core 3.1 using Razor Pages Framework.

Razor pages makes it easy to understand and easily change, debug, edit, and deploy ASP.net websites.

It is running in microsofts most recent version of .net Core 3.1. 
This framework has matured well and the .net framework has been around for many many years so you know you are running your website on not only a trusted and well respected platform, but also one which will be relevant for many years to come with easy updates available through the Visual Studio nuget Package Manager.

.Net Core 3 allows deployment of websites as a self contained host, this means you do not need to install .net core on the server to deploy. .Net core also allows the ability for Asp.net web applications to be hosted on linux servers or directly to an azure web client, so .net is no longer restricted to windows servers.

### Summary of Features
- Microsoft SQL Server Database
- Configuration Options (Different SQL Database for Production or Development)
- CSS Bundling and Minification
- Javascript Bundling and Minification
- Server Side Rendered Pages
- Live Reloading for Debug mode

## Developer Notes

### Required Visual Studio Extensions
- Bundler & Minifier - by Mads Kristensen
- Web Compiler - by Mads Kristensen

### Non Essential VS Extensions
- Web Essentials Pack - by Mads Kristensen

### Links
- https://www.learnrazorpages.com/

### EF Core Tools - Updating The Database
- You can create a database migration file by typing ```Add-Migration Your_Migration_Name_Here``` in the package manager console
- You can update the database by running ```Update-Database``` in the package manager console

![Screenshot of CLEVAR Web Homepage](https://github.com/tomarks/ClevarWebProject/blob/main/ClevarWebHomepage.png "Home Page Screenshot")

