FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Services/Department/Department.API/Department.API.csproj", "Services/Department/Department.API/"]
COPY ["BuildingBlocks/WebHost.Customization/WebHost.Customization.csproj", "BuildingBlocks/WebHost.Customization/"]
RUN dotnet restore "Services/Department/Department.API/Department.API.csproj"
COPY . .
WORKDIR "/src/Services/Department/Department.API"
RUN dotnet build "Department.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Department.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Department.API.dll"]