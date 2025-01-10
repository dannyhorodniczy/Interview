FROM mcr.microsoft.com/dotnet/sdk:9.0.101-alpine3.20-amd64 AS build
WORKDIR /build

RUN apk add --no-cache icu-libs

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore -r linux-musl-x64
# Build and publish a release
RUN dotnet publish -c Release -o out -r linux-musl-x64 --no-restore --no-self-contained

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0.0-alpine3.20-amd64
WORKDIR /App
COPY --from=build /build/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]


# building an API that runs on a docker container
#
# 1. build the web api image
#C:\Dev\Interview\WebApi> docker build -t webapi-image -f Dockerfile .
#
# 2. run the webapi-iamge
#C:\Dev\Interview\WebApi> docker container run -d --name mynewapi -p 8080:8080 webapi-image
#
# Other stuff:
# - still not connecting to the database :/
# - still need to run on Linux containers!
# - still need to... I forget :/
# - ideally would run in https


## WINDOWS DOCKERFILE
#FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
#WORKDIR /App
#
## Copy everything
#COPY . ./
## Restore as distinct layers
#RUN dotnet restore
## Build and publish a release
#RUN dotnet publish -o out
#
## Build runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
#WORKDIR /App
#COPY --from=build /App/out .
#ENTRYPOINT ["dotnet", "WebApi.dll"]
