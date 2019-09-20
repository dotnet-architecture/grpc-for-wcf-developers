FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build

WORKDIR /src

COPY ./Kubed.sln .
COPY ./src/StockData/StockData.csproj ./src/StockData/
COPY ./src/StockWeb/StockWeb.csproj ./src/StockWeb/

RUN dotnet restore

COPY . .

RUN dotnet publish --no-restore -c Release -o /published src/StockWeb/StockWeb.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0

WORKDIR /app

COPY --from=build /published .

ENTRYPOINT [ "dotnet", "StockWeb.dll" ]
