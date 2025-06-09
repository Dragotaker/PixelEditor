FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# Copy the project files
COPY . .

# Build the application
RUN msbuild PixelEditor.sln /p:Configuration=Release

# Create the runtime image
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/bin/Release .

# Set the entry point
ENTRYPOINT ["PixelEditor.exe"] 