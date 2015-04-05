# System Info Snapshot
Collect the computer and system info at a given time in order to track issues or problems.

You can share the generated report with your friend or someone of your trust to help you detect your computer problem at a given moment.

# Included Reports
1. System Info
2. Processes
3. Services
4. Startup Applications
5. Installed programs

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
* **Example:** "SystemInfoSnapshot.exe --null"

## Silent mode
* **Arguments:** '-s', '/s' or '--silent'
* **Description:** Run program without showing the GUI. After the report is generated that will be shown on explorer after completion.
* **Example:** "SystemInfoSnapshot.exe -s"

##Open report on completion
* **Arguments:** '-o', '/o' or '--open-report'
* **Description:** After the report is generated that will be opened automatically in the default browser.
* **Example:** "SystemInfoSnapshot.exe -o"

## Examples
1. "SystemInfoSnapshot.exe --null -o" - Generate and open the report in the default browser without showing the GUI.
2. "SystemInfoSnapshot.exe -s -o" - Generate, show and open the report in the explorer and the default browser without showing the GUI.


# Requirements
* Windows Vista or above (Vista, Server 2008, 7, Server 2012, 8, 8.1, 10)
* [Microsoft .NET Framework 4.5](http://www.microsoft.com/en-us/download/details.aspx?id=30653) (Already pre-installed on Windows 8 and 8.1)

Small and lightweight application with just one executable file.
No installation needed, this software is portable and free!


# Download and demos
http://systeminfosnapshot.com/