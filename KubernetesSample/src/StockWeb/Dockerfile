FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /src

COPY ./StockKube.sln .
COPY ./src/StockData/StockData.csproj ./src/StockData/
COPY ./src/StockWeb/StockWeb.csproj ./src/StockWeb/

RUN dotnet restore

COPY . .

RUN dotnet publish --no-restore -c Release -o /published src/StockWeb/StockWeb.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

WORKDIR /app

COPY --from=build /published .

ENTRYPOINT [ "dotnet", "StockWeb.dll" ]
