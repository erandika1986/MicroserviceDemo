FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ApiGateways/Mobile.Bff/Mobile.HttpAggregator/Mobile.HttpAggregator.csproj ApiGateways/Mobile.Bff/Mobile.HttpAggregator/
RUN dotnet restore ApiGateways/Mobile.Bff/Mobile.HttpAggregator/Mobile.HttpAggregator.csproj
COPY . .
WORKDIR /src/ApiGateways/Mobile.Bff/Mobile.HttpAggregator
RUN dotnet build Mobile.HttpAggregator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Mobile.HttpAggregator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mobile.HttpAggregator.dll"]
