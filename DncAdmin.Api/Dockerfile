FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["DncAdmin.Api/DncAdmin.Api.csproj", "DncAdmin.Api/"]
RUN dotnet restore "DncAdmin.Api/DncAdmin.Api.csproj"
COPY . .
WORKDIR "/src/DncAdmin.Api"
RUN dotnet build "DncAdmin.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DncAdmin.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DncAdmin.Api.dll"]