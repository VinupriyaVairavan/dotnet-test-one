FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7000

ENV ASPNETCORE_URLS=http://+:7000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["source/FunctionAppTest/FunctionAppTest.csproj", "source/FunctionAppTest/"]
RUN dotnet restore "source/FunctionAppTest/FunctionAppTest.csproj"
COPY . .
WORKDIR "/src/source/FunctionAppTest"
RUN dotnet build "FunctionAppTest.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FunctionAppTest.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FunctionAppTest.dll"]
