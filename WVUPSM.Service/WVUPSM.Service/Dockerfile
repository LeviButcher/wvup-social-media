#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . "WVUPSM.Service/"
COPY Nuget.Config "/root/.nuget/NuGet/"
RUN dotnet restore "WVUPSM.Service/WVUPSM.Service.csproj"
WORKDIR "/src/WVUPSM.Service"
RUN dotnet build "WVUPSM.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WVUPSM.Service.csproj"  -c Release -o /app

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WVUPSM.Service.dll"]
