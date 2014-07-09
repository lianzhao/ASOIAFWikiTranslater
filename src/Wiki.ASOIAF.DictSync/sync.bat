@echo off
rd /s /q .\bin\debug\log
.\bin\debug\Wiki.ASOIAF.DictSync.exe
echo PRESS ANY KEY TO CONTINUE
pause>nul