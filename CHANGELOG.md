# Changelog

# v1.6.1.0
## 22/04/2015

* Added the FluentCommandLineParser.dll library to the project to easier get the passed parameters. https://github.com/fclp/fluent-command-line-parser
* Added a new argument 'Filename' (-f, /f, --filename): Set the path and/or filename for the generated report file. If the passed path is a directory the default filename will be appended to it. Absolute or relative paths are allowed
* Added a new argument 'Max Tasks' (-t, /t, --max-tasks <value>): Sets the maximum number of concurrent tasks enabled to generate the reports. If it is -1, there is no limit on the number of concurrently running operations (Default). If it is 1 it will run in a single thread, best used with single core CPUs or for debuging.
* Added a new argument 'Help' (-?, /?, --help): Display a help message with all possible arguments and its usages
* Added support for combined (grouped) options when passing three or more boolean arguments. ex: -sno (same as: -s -n -o)
* Changed the way arguments can be passed. Now it support the following syntax: [-|--|/][switch_name][=|:| ][value]
* Removed the empty columns for installed programs table under Mac OSX
* Removed 'Single Thread' argument in favor of the new 'Max Tasks' argument, use -t 1 to produce the same effect


# v1.6.0.0
## 21/04/2015

* Added a new report 'SpecialFiles' it shows some special files from the system and its contents, ie: hosts
* Added support for installed programs under Mac OSX and most linux distributions (Debian, Ubuntu, Mint, Centos, Fedora, RedHat, Arch, etc)
* Fixed a critical bug under linux and OSX that cause program crash if not possible to obtain the hardware
* Fixed a critical bug under linux and OSX that cause the processes report to broke all the html when any process name is null
* Changed the InstalledProgram class was been rewrited and moved into Core


# v1.5.0.0
## 18/04/2015

* Added a disk manager holding all devices informations such as SMART
* Added more information to disks under Hadware > HDDs including SMART, problems will be reported too
* Added the program version to the GUI title
* Added a search box to every table to be able to filter the results
* Added DataTable js script and style
* Added floatThead js script
* Changed the html tables code to use DataTables that gives much more customization features
* Changed the background color to lightgrey of the tables head
* Changed the tables head to be fixed and always show on scroll while a table is visible
* Changed the tables are now wrapped inside a '<div class="responsive"></div>'
* Improved tables sort are now more realible
* Fixed the tables big font-size aren't the default from theme
* Moved Malware folder into Core
* Moved Libraries into Resources
* Removed table-responsive class from tables (wrong usage)


# 1.4.0.0
## 16/04/2015

* Added a malware database to detect bad programs on the system (Windows only)
* Added 204 toolbars to malware list (Windows only)
* Added support to Linux and others mono compatible systems
* Added a 'build.bat' file to compile project under Windows (Visual Studio required)
* Added a 'Makefile' file to compile project under Linux systems using Mono (mono-complete required)
* Changed program will now be executed using 64bit process under 64bit systems, otherwise 32bit will be used
* Changed GUI update timer interval to 1s rather than 500ms
* Changed uptime values are now represented by: days.hours:minutes:seconds
* Changed controls postion to bottom left instead of top right
* Changed page title build date to use CurrentCulture instead of InvariantCulture
* Changed rewrite whole reports using HtmlTextWriter wich use a StringBuilder instead a single string with concatenation
* Changed program will now run with less privileges (from full to admin)
* Improved report generation performance
* Removed Service type from report


# 1.3.0.0
## 11/04/2015

* Added a new dashboard with information about the reports
* Added a new section with controls to navigate between items and display them
* Added the items with warning and danger classes can now be easily browsed and followed with the navigation controls in order to quickly find the problems
* Added comments on javascript code
* Fixed Html navbar overlaps to content on small resolutions
* Changed some javascript code to improve page performance
* Updated bootstrap version from 3.3.2 to 3.3.4


# 1.2.20.2
## 10/04/2015

* Added a new argument 'single thread': Generate all the reports one by one without use parallel tasks. Best used with single core CPUs or for debuging.
* Fixed application always crash on autoruns collection when tab icons are null
* Fixed auto accept the eula dialog from autorunsc, this was blocking the application
* Removed unused namespaces


# 1.2.20.0
## 10/04/2015

* Added more information under Autoruns about almost everything that loads with the system
* Added autorunsc.exe from Sysinternals into projected [Embedded]
* Changed reports are now generated with parallel threads, 100% boost over single thread on most cases - More cores more benefits


# 1.2.0.0
## 09/04/2015

* Added a new report about system info
* Changed old system info report to hardware
* Fixed PnP Devices not showing


# 1.1.10.0
## 08/04/2015

* Added a timer on GUI to inform the elapsed time in seconds
* Changed the whole reports system [Internal]


# 1.1.0.0
## 07/04/2015

* Added PnP devices
* Added Network devices
* Fixed a crash while trying to get info from installed programs with bad keys
* Fixed sensor units on progress bar was always the % sign
* Improved template responsive styles
* Internal improvements


# 1.0.0.0
## 06/04/2015

* First Release