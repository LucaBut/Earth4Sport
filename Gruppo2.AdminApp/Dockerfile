#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Gruppo2.AdminApp/Gruppo2.AdminApp.csproj", "Gruppo2.AdminApp/"]
RUN dotnet restore "Gruppo2.AdminApp/Gruppo2.AdminApp.csproj"
COPY . .
WORKDIR "/src/Gruppo2.AdminApp"
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - && apt-get install -y nodejs
RUN npm install -g npm
RUN dotnet build "Gruppo2.AdminApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gruppo2.AdminApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gruppo2.AdminApp.dll"]