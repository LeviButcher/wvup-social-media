#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . "WVUPSM.MVC/"
RUN dotnet restore "WVUPSM.MVC/WVUPSM.MVC.csproj"
WORKDIR "/src/WVUPSM.MVC"
RUN dotnet build "WVUPSM.MVC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WVUPSM.MVC.csproj" -c Release -o /app

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WVUPSM.MVC.dll"]
