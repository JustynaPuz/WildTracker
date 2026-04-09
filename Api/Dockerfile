# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for layer caching
COPY ["Api/WildTracker.Api.csproj",             "Api/"]
COPY ["Application/WildTracker.Application.csproj", "Application/"]
COPY ["Domain/WildTracker.Domain.csproj",       "Domain/"]
COPY ["Infrastructure/WildTracker.Infrastructure.csproj", "Infrastructure/"]
COPY ["Contracts/WildTracker.Contracts.csproj", "Contracts/"]

RUN dotnet restore "Api/WildTracker.Api.csproj"

# Copy all source code and publish
COPY . .
RUN dotnet publish "Api/WildTracker.Api.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "WildTracker.Api.dll"]
