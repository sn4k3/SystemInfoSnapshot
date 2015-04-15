#
# SystemInfoSnapshot
# Author: Tiago Conceição
# http://systeminfosnapshot.com
#
# Makefile - build and compile project
#

# Project file
PROJECTFILE=SystemInfoSnapshot.sln

# Configuration to use: Release or Debug
CONFIGURATION="Release"
#CONFIGURATION="Debug"

# The compiler to use.
CC=xbuild

# Properties will be the options pass to the compiler.
PROPERTIES = \
	/property:Configuration=$(CONFIGURATION) 

all: app

app: 
	$(CC) $(PROPERTIES) $(PROJFILE)
	
rebuild: clean app

clean:
	$(CC) $(PROPERTIES) /target:clean $(PROJECTFILE)

