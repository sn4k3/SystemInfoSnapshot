# Changelog

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