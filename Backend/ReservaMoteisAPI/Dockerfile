FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ReservaMoteisDomain/BookMotelsDomain.csproj ReservaMoteisDomain/
COPY ReservaMoteisAPI/BookMotelsAPI.csproj ReservaMoteisAPI/
COPY ReservaMoteisApplication/BookMotelsApplication.csproj ReservaMoteisApplication/
COPY ReservaMoteisInfra/BookMotelsInfra.csproj ReservaMoteisInfra/

WORKDIR /src/ReservaMoteisAPI
RUN dotnet restore BookMotelsAPI.csproj

WORKDIR /src
COPY . .

WORKDIR /src/ReservaMoteisAPI
RUN dotnet build BookMotelsAPI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BookMotelsAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookMotelsAPI.dll"]