@echo off
set /p Path=Path to parsing repository: 

cd ..\build\Toolkit
start /d "." dotnet Noskito.Toolkit.dll parse -p %Path%