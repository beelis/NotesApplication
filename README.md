Installation
############
`npm install`
Note: There's a postinstall-script that will take care of bower dependencies, so `npm install` really is enough. If you have a global bower installation, just run `bower install` inside src/Notes.

Build
#####
`cd src/Notes && dotnet bundle && dotnet build`

Run
###
`cd src/Notes && export ASPNETCORE_ENVIRONMENT=Development && dotnet run`
