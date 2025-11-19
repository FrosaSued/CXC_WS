# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app

# Copy the project files and restore dependencies
COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
# Install ICU for globalization support
RUN apk add --no-cache icu-libs

# Set the environment variable to use ICU
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port
EXPOSE 9515

ENV ASPNETCORE_URLS=http://+:9515

# Command to run the application
ENTRYPOINT ["dotnet", "CXC_WS.dll"]
