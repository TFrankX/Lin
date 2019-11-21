#Download base image ubuntu 16.04
#FROM ubuntu:16.04
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY *.sln .
COPY Lin/*.csproj ./Lin/
COPY LinClient/*.csproj ./LinClient/
RUN dotnet restore

# copy everything else and build app
COPY Lin/. ./Lin/
WORKDIR /app/Lin
RUN dotnet publish -c Release -o out

WORKDIR /app
# copy everything else and build app
COPY LinClient/. ./LinClient/
WORKDIR /app/LinClient
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Lin/out ./
COPY --from=build /app/LinClient/out ./

EXPOSE 13000
EXPOSE 13001
#ENTRYPOINT ["dotnet", "LinServ.dll"]