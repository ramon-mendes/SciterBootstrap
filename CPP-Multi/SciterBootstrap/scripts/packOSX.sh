#!/bin/sh

# IN OSX, COMPRESS FILES IN /res FOLDER TO ArchiveResource.cpp FILE
cd "$(dirname "$0")"
chmod +x packfolder 
./packfolder ../res ../ArchiveResource.cpp
