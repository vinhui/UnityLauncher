mkdir bin
dotnet publish -r linux-x64 -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true -p:DebugType=None -o bin/
