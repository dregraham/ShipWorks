@ECHO OFF
cls

set "str3="

if "%~1"=="" goto Run

set "str1=-ScriptArgs --params= "
set "str2=%1"
set "str4=%2"
set "str3=%str1%%str2% %str4%"



:Run
Powershell.exe -executionpolicy remotesigned -File  build.ps1 -Target "%~1" -Configuration "%~2" 