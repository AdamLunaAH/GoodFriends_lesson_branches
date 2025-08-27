#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./dotnet-run-production.sh

# Build the entire solution in Release configuration
dotnet build GoodFriends.sln --configuration Release

# Run directly with Production environment
dotnet run --project AppWebApi/AppWebApi.csproj --configuration Release --environment Production