FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ApiGateways/Mobile.Bff/Employee.HttpAggregator/Employee.HttpAggregator.csproj", "ApiGateways/Mobile.Bff/Employee.HttpAggregator/"]
RUN dotnet restore "ApiGateways/Mobile.Bff/Employee.HttpAggregator/Employee.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Mobile.Bff/Employee.HttpAggregator"
RUN dotnet build "Employee.HttpAggregator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Employee.HttpAggregator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Employee.HttpAggregator.dll"]