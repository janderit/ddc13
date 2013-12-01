@echo off
SET dopause=0
IF "%1"=="" (
  SET dopause=1
  SET target=Default
) ELSE (
  SET target=%1
)
cls

@IF NOT EXIST "tools\FAKE\tools\Fake.exe" (
	@echo Installing FAKE from NUGET... this may take a short while...
	@"tools\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
)

@echo Invoking build_script.fsx...
@"tools\FAKE\tools\Fake.exe" build_script.fsx %target%

IF "%dopause%"=="1" pause
