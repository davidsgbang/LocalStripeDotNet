FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LocalStripeDotNet.Tester/LocalStripeDotNet.Tester.csproj", "LocalStripeDotNet.Tester/"]
RUN dotnet restore "LocalStripeDotNet.Tester/LocalStripeDotNet.Tester.csproj"
COPY . .
WORKDIR "/src/LocalStripeDotNet.Tester"
RUN dotnet build "LocalStripeDotNet.Tester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocalStripeDotNet.Tester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalStripeDotNet.Tester.dll"]
