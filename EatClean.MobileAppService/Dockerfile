FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["EatClean.MobileAppService.csproj", "./"]
RUN dotnet restore "./EatClean.MobileAppService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EatClean.MobileAppService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EatClean.MobileAppService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EatClean.MobileAppService.dll"]
