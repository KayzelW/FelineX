FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlazorServer/BlazorServer/BlazorServer.csproj", "BlazorServer/BlazorServer/"]
COPY ["BlazorServer/BlazorServer.Client/BlazorServer.Client.csproj", "BlazorServer/BlazorServer.Client/"]
RUN dotnet restore "BlazorServer/BlazorServer/BlazorServer.csproj"
COPY . .
WORKDIR "/src/BlazorServer/BlazorServer"
RUN dotnet build "BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServer.dll"]
