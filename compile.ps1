# have to do a platform specific compile because the powershell library has a dependency that can't properly work when compiled with the generic win-x64 flag

dotnet publish -c release -r win10-x64 --self-contained true -p:PublishSingleFile=true