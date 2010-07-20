--------------------------------------------
-- Rhino Security Administration
--------------------------------------------

This is a simple implementation of an administrative user interface for the Rhino Security framework. 
It currently only supports:
 * managing users, groups and their associations
 * managing operations (add and search)
 * managing permissions for users\groups on given operations

It's built using ExtJS for the UI and ASP.NET MVC v2 for the server side. 
Json is used for the communication between client and server.

Please read the following sections before running the application.

--------------------------------------------
-- Rhino Security
--------------------------------------------

This software uses a custom version of Rhino Security, which has been built with NHibernate 2.1.2. 
At the time of this writing (19 july 2010) Rhino.Security on github is still referencing NH 2.1.1.


--------------------------------------------
-- Ext JS
--------------------------------------------

The UI of this software has been built using ExtJS (see http://www.sencha.com/products/js/). For licencing reasons (ExtJS is distributed under LGPL v3) the source code of ExtJS cannot be included here. 
So, in order to correctly run the application, you must:
 a) download ExtJS from http://www.sencha.com/products/js/
 b) copy it under '.\src\Rhino.Security.Mgmt\ext'.
 
After having done that, you should have the following folder\file structure under '.\src\Rhino.Security.Mgmt\ext':
	
	.\src\Rhino.Security.Mgmt\ext\adapter
	.\src\Rhino.Security.Mgmt\ext\resources
	.\src\Rhino.Security.Mgmt\ext\ext-all.js
	.\src\Rhino.Security.Mgmt\ext\ext-all-debug.js
	.\src\Rhino.Security.Mgmt\ext\ext-all-debug-w-comments.js
	
For a deployed live sample see http://codegen.nexida.com/samples/rhinosecurityadmin/#

--------------------------------------------
-- Database
--------------------------------------------

The application uses a sample database (called [RhinoSecurityAdmin]) containing the Rhino Security tables and a custom User table. 
A utility script is provided to generate the database (see SecurityTablesScript).

--------------------------------------------
-- Build
--------------------------------------------

The project should be built using the provided Powershell scripts, that are ported from Rhino Security by Ayende (http://github.com/ayende/rhino-security).
For more details please read 'How to build.txt'.

--------------------------------------------
-- Licences
--------------------------------------------

This software is distributed under the terms of the new BSD Licence (see licence.txt).

It uses software developed under different licences. See the '\libs' folder. 

In particular, make sure you read the 'ExtJs' section above.

