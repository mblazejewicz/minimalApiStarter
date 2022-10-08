# syntax=docker/dockerfile:1

# Build dotnet project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# node for forntend build
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash - \
 && apt-get install -y --no-install-recommends nodejs \
 && echo "node version: $(node --version)" \
 && echo "npm version: $(npm --version)" \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
    
# Copy csproj and restore as distinct layers
COPY MinimalApiStarter.csproj ./
RUN dotnet restore
   
# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "MinimalApiStarter.dll"]