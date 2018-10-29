cd .\WVUPSM.DAL\WVUPSM.Models\
echo "Packing Models to all Projects"
dotnet pack -o ../NugetPackages
dotnet pack -o ../../WVUPSM.Service/WVUPSM.Service/NugetPackages
dotnet pack -o ../../WVUPSM.MVC/WVUPSM.MVC/NugetPackages
cd ..\WVUPSM.DAL\
echo "Packing DAL to all Projects"
dotnet pack -o ../NugetPackages
dotnet pack -o ../../WVUPSM.Service/WVUPSM.Service/NugetPackages
dotnet pack -o ../../WVUPSM.MVC/WVUPSM.MVC/NugetPackages
cd ..\..\WVUPSM.Service\WVUPSM.Service\
dotnet add package WVUPSM.DAL
dotnet add package WVUPSM.Models
cd ..\..\WVUPSM.MVC\WVUPSM.MVC\
dotnet add package WVUPSM.DAL
dotnet add package WVUPSM.Models
