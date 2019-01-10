@echo off

set PATH=C:\Program Files (x86)\Git\cmd;%PATH%

rmdir /S /Q sciter-dport
git clone https://github.com/midiway/sciter-dport.git
rmdir /S /Q "sciter-dport/.git"