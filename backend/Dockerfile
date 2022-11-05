FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7135

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tictactoe.csproj", "./"]
RUN dotnet restore "Tictactoe.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Tictactoe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tictactoe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:7135
ENTRYPOINT ["dotnet", "Tictactoe.dll"]
