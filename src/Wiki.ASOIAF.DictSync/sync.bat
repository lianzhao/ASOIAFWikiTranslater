@echo off
rd /s /q .\bin\debug\log
cd /d %~dp0\bin\debug
.\Wiki.ASOIAF.DictSync.exe
echo PRESS ANY KEY TO CONTINUE
pause>nul