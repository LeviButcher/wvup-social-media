# Created by: Levi Butcher
# Sets up our DB to be used for the Service and MVC



From microsoft/dotnet:2.1-sdk-nanoserver-1803 as models
WORKDIR /src
Copy ["WVUPSM.Models/WVUPSM.Models.csproj", "WVUPSM.Models/"]
RUN dotnet restore "WVUPSM.Models/WVUPSM.Models.csproj"
Copy "WVUPSM.Models/" "WVUPSM.Models/"


From models as DAL
WORKDIR /src
COPY ["WVUPSM.DAL/WVUPSM.DAL.csproj", "WVUPSM.DAL/"]
RUN dotnet restore "WVUPSM.DAL/WVUPSM.DAL.csproj"
Copy 'WVUPSM.DAL/' "WVUPSM.DAL/"
WORKDIR "/src/WVUPSM.DAL"
RUN dotnet ef migrations add Init -c SMContextProduction -o ProMigrations

From microsoft/mssql-server-windows-developer as final
WORKDIR app
Expose 1433
ENV sa_password=Develop@90
RUN DIR
ENV ACCEPT_EULA=Y
COPY --from=DAL /src src
WORKDIR /
