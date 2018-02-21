# C# Demo Projects for [Active Query Builder ASP.NET Edition 2](http://www.activequerybuilder.com/product_asp.html)
#
#
---
#### This repository holds demo projects for the **OBSOLETE** version of Active Query Builder ASP.NET Edition!
#### Please proceed to the [Demo projects repository](https://github.com/ActiveDbSoft/active-query-builder-3-asp-net-samples-csharp)  for **Active Query Builder ASP.NET Edition 3**
---

#
## What is Active Query Builder?
Active Query Builder is a visual query builder and SQL parser component for ASP.NET (WebForms and MVC). 
##### Details:
- [Active Query Builder web site](http://www.activequerybuilder.com/),
- [Active Query Builder ASP.NET Edition details page](http://www.activequerybuilder.com/product_asp.html).

## How do I get Active Query Builder?
- [Download the trial version](http://www.activequerybuilder.com/trequest.html?request=asp) from the product web site
- Get it by installing the [ActiveQueryBuilder.ASPNET NuGet package](https://www.nuget.org/packages/ActiveQueryBuilder.ASPNET/).

## What's in this repository?
The demo projects in this repository illustrate various aspects of the component's functionality from basic usage scenarios to advanced features. They are also included the trial and full versions of Active Query Builder.
A brief description of each project can be found below.
##### Prerequisites:
- Visual Studio 2012 or higher,
- .NET Framework 4.0 or higher.
- [Active Query Builder ASP.NET NuGet package](https://www.nuget.org/packages/ActiveQueryBuilder.ASPNET/).

## How to get these demo projects up and running?

1. Clone this repository to your PC.
2. Open the .sln file of the demo project you want to explore in Visual Studio.
3. Select the "Tools" - "NuGet Package Manager" - "**Package Manager Console**" menu item.
4. Install **Active Query Builder ASP.NET NuGet package** by running the following command:
     ```Install-Package ActiveQueryBuilder.ASPNET```
5. Run the project.

## Have a question or want to leave feedback?

Welcome to the [Active Query Builder Help Center](https://support.activequerybuilder.com/hc/)!
There you will find:
- End-user's Guide,
- Getting Started guides,
- Knowledge Base,
- Community Forum.

## Contents

All demo projects can be arranged into three groups:
1. 'Getting started' projects for different environments.
2. Projects which contain frequently asked code samples for different environments. 
3. Projects to illustrate advanced aspects of the component's functionality.
Projects in the third group are designed for the WebForms environment only, but they don't contain any evnvironment-specific functionality, so their code can be used in any environment.

Projects which require a database connection must be configured according to the database server being used. 
- Specify the connection string in the <connectionStrings> section of the 'Web.config' file.
- Choose the syntax and metadata providers which suit your database server and connection method. Read details in this article: [What are the Syntax and Metadata Providers for?](https://support.activequerybuilder.com/hc/en-us/articles/115001063445-What-are-the-Syntax-and-Metadata-providers-for-).

### Getting Started projects

There are two basic projects for each of the environments: 
- **The Offline project** which doesn't establish a connection to database, but reads metadata from the pre-generated XML file (usually from the Northwind database). 
It is ideal for quick acquaintance with the component.
- **The Query Results project** which must be configured to connect to a database. 
It lets quickly test the component with your database. It also illustrates usage of the CriteriaBuilder control along with a result data grid.

##### WebForms
In addition to two basic demo projects there's a project to illustrate **interaction with ASP.NET controls**. A button with the UpdatePanel which triggers asyncronous postback event in this demo switches metadata loading from one preset to another.

    \WebForms\Simple Query Results Demo
    \WebForms\Simple Offine Demo
    \WebForms\Interact With ASPNET Controls

##### MVC 2.0
There is only one demo for MVC 2.0 as this environment is getting old. The principles of working with Active Query Builder in this environment is similar to the later versions.

    \MVC\MVC 2 Demo

##### MVC 4.0
MVC 4.0 demo projects are designed for both ASPX and Razor view engines.

    \MVC\MVC 4\ASPX Offline Demo
    \MVC\MVC 4\ASPX Query Results Demo
    \MVC\MVC 4\Razor Offline Demo
    \MVC\MVC 4\Razor Query Results Demo

##### MVC 5.0
MVC 5.0 demo projects are designed only for the Razor view engine, but you are free to use any engine according to your needs.

    \MVC\MVC 5\Simple Offline Demo
    \MVC MVC 5\Simple Query Results Demo

### Frequently Asked Code Samples

##### Changing DB Connection in runtime
This demo shows how to switch database connection in runtime.

    \WebForms\Change Connection Demo
    \MVC 5\Change Connection Demo

##### Working with Alternate Names
The Alternate Names feature allows for substitution of unintelligible names of database objects and fields for friendly aliases. End-users see the friendly names in the visual query building interface as well as in the query text when editing it manually. The component gets the query with real names back only when you need to execute the query against a database server. 
Read more about this feature in the Knowledge Base: [Defining friendly names for database objects and fields](https://support.activequerybuilder.com/hc/en-us/articles/115001063565-User-friendly-database-object-and-field-names).

    \WebForms\Alternate Names Demo
    \MVC 5\Alternate Names Demo

##### Toggling usage of Alternate Names
Advanced SQL writers may wish to turn displaying of friendly aliases off to see the real names in the visual query building interface. This demo project gives the sample for this operation. 

    \WebForms\Toggle UseAltNames Demo
    \MVC 5\Toggle UseAltNames Demo

### Advanced Functionality

##### Load Metadata Demo
This demo illustrates various ways of loading metadata information to Active Query Builder. 
Metadata handling is described here: [Metadata handling and filtration](https://support.activequerybuilder.com/hc/en-us/sections/115000316525-AQB-for-NET-Metadata-handling-and-filtration).

##### Metadata Structure Demo
This demo gives an idea how to build a custom structure of nodes in the Database Schema Tree: Creation of folders for particular objects (Favorites), folders which contain objects according to a mask, etc. 
Read more about this feature: [Customizing the Database Schema Tree structure](https://support.activequerybuilder.com/hc/en-us/articles/115001055289-Customizing-the-Database-Schema-Tree-structure).

##### Virtual Objects and Fields Demo
This demo illustrates the ability to add queries as new objects to the Database Schema Tree that act like ordinary database views. Users see virtual objects as ordinary database objects and can use them in their queries without the need to understand their complexity. The same is true for virtual fields: you can add new fields and put complex SQL expressions or correlated sub-queries as their expressions. The users will see them as ordinary fields even in the SQL query text. They will be expanded to corresponding SQL expressions only when you need to execute the query against a database server.
Read more about this feature in the Knowledge Base: [Adding virtual objects and calculated fields to database schema](https://support.activequerybuilder.com/hc/en-us/articles/115001055269-Virtual-objects-and-calculated-fields).

##### Query Analysis Demo
This demo contains the code example of a pass through the entire internal query object model.

##### Query Creation Demo
This demo provides code samples of programmatic creation of various SQL queries.

##### Query Modification Demo
This demo explains how one can analyze and modify user queries to correct user errors or to limit the data returned by them.

##### Custom Expression Editor Demo
This demo adds a button to the in-place editor of Expression and Criteria cells of the Query Columns List and gives the sample of creating a custom editor to edit the cell content in a popup window. 

##### SQL Syntax Highlighting Demo
This demo provides sample of integration of a third-party SQL text editor to enable SQL syntax highlighting.

##### jQueryUI Theming Demo
This demo illustrates run-time switching and appliance of jQueryUI themes.

##### User-defined Fields Demo	
This demo introduces the experimental feature of letting define virtual fields by end-users.

##### 'No Design Area' Demo
This demo implements a visual query building interface without the Design Area. Users build queries by dragging fields from the Database Schema Tree to the Query Columns List and define query column properties, such as grouping, ordering, etc. The appropriate database objects are automatically added to the query, get linked with each other and removed when they aren't needed.

##### Client-side Events Handling Demo
This demo gives an idea how to perform custom actions in response to some user actions. The full list of available JavaScript events can be found in this article: [Active Query Builder JavaScript API](https://support.activequerybuilder.com/hc/en-us/articles/115001064225-JavaScript-API-of-Active-Query-Builder-ASP-NET-edition).

##### Accessibility Demo
This demo presents Active Query Builder in a high-contrast theme. Role and ARIA-LABEL attributes are defined for the controls to allow narrative speaking.

## License
The source code of the demo projects in this repository is covered by the [MIT license](https://en.wikipedia.org/wiki/MIT_License).

