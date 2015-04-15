@echo off
title "SystemInfoSnapshot Compiler"
set PROJECTFILE=SystemInfoSnapshot.sln
set CONFIGURATION=Release
set PROPERTIES = /property:Configuration=%CONFIGURATION%

echo #
echo # SystemInfoSnapshot
echo # Author: Tiago Conceição
echo # http://systeminfosnapshot.com
echo #
echo # Build and compile project
echo #

ping 1.1.1.1 -n 1 -w 3000 > nul

if defined VS120COMNTOOLS (
   call "%VS120COMNTOOLS%\vsvars32.bat"
   goto compile
) else (    
	if defined VS110COMNTOOLS (
	   call "%VS110COMNTOOLS%\vsvars32.bat"
	   goto compile
	) else ( 
		if defined VS100COMNTOOLS (
			 call "%VS110COMNTOOLS%\vsvars32.bat"
			 goto compile
		) else ( 
			if defined VS90COMNTOOLS (
				call "%VS90COMNTOOLS%\vsvars32.bat"
				goto compile
			) else ( 
				echo ERROR: msbuild require a valid instalation of Visual Studio 9, 10, 11 or 12
				pause
			)
		)
	)
)


:compile 
msbuild %PROPERTIES% %PROJECTFILE%
pause

:end