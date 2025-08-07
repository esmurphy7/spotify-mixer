# Use the official .NET 8.0 runtime image for Linux
# This provides a Linux base with .NET 8.0 runtime pre-installed
# OPTIMIZED: Using runtime-only image for final stage (~200MB vs ~4GB for Windows)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory inside the container
# This is where the application will be installed and run from
WORKDIR /app

# Expose port 80 for HTTP traffic
# The application will listen on this port inside the container
EXPOSE 80

# Expose port 443 for HTTPS traffic (if needed)
# This allows for secure connections to the application
EXPOSE 443

# Use the official .NET 8.0 SDK image for building the application
# This image contains the full .NET SDK needed to compile the application
# OPTIMIZED: SDK image only used for building, not in final runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory for the build process
WORKDIR /src

# Copy the project file first to leverage Docker layer caching
# This allows Docker to cache the restore step if only source code changes
COPY ["SpotifyMixerApi/SpotifyMixerApi.csproj", "SpotifyMixerApi/"]

# Restore NuGet packages
# This downloads all the dependencies specified in the project file
RUN dotnet restore "SpotifyMixerApi/SpotifyMixerApi.csproj"

# Copy the rest of the source code
# This includes all the C# files, configuration files, and other project assets
COPY . .

# Set the working directory to the project folder
WORKDIR "/src/SpotifyMixerApi"

# Build the application in Release mode
# This compiles the C# code into an executable assembly
RUN dotnet build "SpotifyMixerApi.csproj" -c Release -o /app/build

# Publish the application for production
# This creates a framework-dependent deployment optimized for the runtime image
FROM build AS publish
RUN dotnet publish "SpotifyMixerApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: Create the optimized runtime image
# OPTIMIZED: Using minimal runtime image instead of full SDK image
# This significantly reduces the final image size from ~8GB to ~200MB
FROM base AS final

# Set the working directory for the final image
WORKDIR /app

# Copy the published application from the build stage
# This includes the compiled application and all its dependencies
COPY --from=publish /app/publish .

# CONTAINER FIX: Set environment variables for proper container configuration
# CONTAINER FIX: Configure ASP.NET Core to bind to all interfaces (0.0.0.0)
ENV ASPNETCORE_URLS=http://0.0.0.0:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Set the entry point for the container
# This tells Docker what command to run when the container starts
ENTRYPOINT ["dotnet", "SpotifyMixerApi.dll"] 