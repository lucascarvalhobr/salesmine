FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SalesMine.Identity.API/SalesMine.Identity.API.csproj", "SalesMine.Identity.API/"]
RUN dotnet restore "SalesMine.Identity.API/SalesMine.Identity.API.csproj"
COPY . .
WORKDIR "/src/SalesMine.Identity.API"
RUN dotnet build "SalesMine.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesMine.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesMine.Identity.API.dll"]