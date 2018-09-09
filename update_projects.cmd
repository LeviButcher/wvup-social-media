cd .\WVUPSM.DAL\WVUPSM.Models\
dotnet pack -o ../NugetPackages
cd ..\WVUPSM.DAL\
dotnet pack -o ../NugetPackages
cd ..\..\WVUPSM.Service\WVUPSM.Service\
dotnet add package WVUPSM.DAL
dotnet add package WVUPSM.Models
cd ..\..\WVUPSM.MVC\WVUPSM.MVC\
dotnet add package WVUPSM.DAL
dotnet add package WVUPSM.Models
