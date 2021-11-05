FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LocalStripeDotNet.Server/LocalStripeDotNet.Server.csproj", "LocalStripeDotNet.Server/"]
RUN dotnet restore "LocalStripeDotNet.Server/LocalStripeDotNet.Server.csproj"
COPY . .
WORKDIR "/src/LocalStripeDotNet.Server"
RUN dotnet build "LocalStripeDotNet.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocalStripeDotNet.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalStripeDotNet.Server.dll"]
