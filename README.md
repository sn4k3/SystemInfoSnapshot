# System Info Snapshot
Collect the computer and system info at a given time in order to track issues or problems.

You can share the generated report with your friend or someone of your trust to help you detect your computer problem at a given moment.

# Included Reports
1. System Info
2. Hardware
3. Networks
4. PnP Devices
5. Processes
6. Services
7. Autoruns
8. Installed programs
9. Special files

# GUI Screenshot
![GUI][gui_screenshot]

[gui_screenshot]: https://github.com/sn4k3/SystemInfoSnapshot/raw/master/SystemInfoSnapshot/Resources/images/gui_screenshot.png "GUI Screenshot"


# Usage
1. Run program (.exe) as Administrator.
2. Wait util report gets generated.
3. Open or send the report to someone who can read it and analyse or problem.


Note: Program can be executed with '-s', '/s' or '--silent' argument to skip GUI.
Report will be generated and the html file will show on explorer after completion. 

**Example:** "SystemInfoSnapshot.exe -s"


# Arguments

## Null mode
* **Arguments:** '-n', '/n' or '--null'
* **Description:** Only generate the report without showing or doing anything. Good for scripts or tasks.
* **Default:** False.
* **Example:** "SystemInfoSnapshot.exe --null"

## Silent mode
* **Arguments:** '-s', '/s' or '--silent'
* **Description:** Run program without showing the GUI. After the report is generated that will be shown on explorer after completion.
* **Default:** False.
* **Example:** "SystemInfoSnapshot.exe -s"

## Open report on completion
* **Arguments:** '-o', '/o' or '--open-report'
* **Description:** After the report is generated that will be opened automatically in the default browser.
* **Default:** False.
* **Example:** "SystemInfoSnapshot.exe -o"

## Define the file path destination
* **Arguments:** '-f', '/f' or '--filename'
* **Description:** Set the path and/or filename for the generated report file. If the passed path is a directory the default filename will be appended to it. Absolute or relative paths are allowed.
* **Default:** Desktop/SystemInfoSnapshot_date-time.html.
* **Examples:**
1. "SystemInfoSnapshot.exe -f ." - Use a dot (.) to set the path as same as the executable path or working dir and preserve default filename.
2. "SystemInfoSnapshot.exe -f="C:\my folder"" - (See note 2)
3. "SystemInfoSnapshot.exe -f:/home/snapshots/report.html" - (See note 3 and 7)
4. "SystemInfoSnapshot.exe -f my_new_report.html" - (See note 4)
5. "SystemInfoSnapshot.exe -f D:\report1" - (See note 5)
* **Notes:**
1. Path must be writable!
2. If a path from a existing directory is passed, filename will be set as default and saved on this directory. ie: -f D:\
3. If the passed path is not a directory then it will try to save the file in this path. ie: -f "D:\myreport.html"
4. Absolute and relative paths are allowed. Relative paths will start from the working directory (from executable path). ie: -f report.html
5. The .html extension will be appended if not present in the filename. ie: -f report
6. If you give a existing file path, that file will be overwritten without warning.
7. ALWAYS use -f:path OR -f=path under a Unix system, because slash (/) is interpreted as a argument. ie: "-f /home" = ERROR | "-f:/home" = OK

## Max tasks to use on reports generation
* **Arguments:** '-t', '/t' or '--max-tasks' <value:int>
* **Description:** Sets the maximum number of concurrent tasks enabled to generate the reports. If it is -1, there is no limit on the number of concurrently running operations (Default). If it is 1 it will run in a single thread, best used with single core CPUs or for debuging.
* **Default:** -1.
* **Examples**
1. "SystemInfoSnapshot.exe -t 0" - Use 0 or lower to use no limit, best value will be choose by .NET Framework. (Default)
2. "SystemInfoSnapshot.exe -t 1" - Allow only 1 maximum task (single thread).
3. "SystemInfoSnapshot.exe -t 4" - Allow only 4 maximum tasks.
* *Note:* See ParallelOptions > MaxDegreeOfParallelism for more information about it.

## Examples
1. "SystemInfoSnapshot.exe --null -o" - Generate and open the report in the default browser without showing the GUI.
2. "SystemInfoSnapshot.exe -s -o" - Generate, show and open the report in the explorer and the default browser without showing the GUI.


# Requirements for Windows
* Windows Vista or above (Vista, Server 2008, 7, Server 2012, 8, 8.1, 10)
* [Microsoft .NET Framework 4.5](http://www.microsoft.com/en-us/download/details.aspx?id=30653) (Already pre-installed on Windows 8 and 8.1)

# Requirements for Linux
* Any capable Linux or OS X with mono installed
* The [mono-complete](http://www.mono-project.com/docs/getting-started/install/linux) package

Small and lightweight application with just one executable file.
No installation needed, this software is portable and free!


# Download and demos
http://systeminfosnapshot.com/