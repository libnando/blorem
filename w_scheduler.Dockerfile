FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . . 
RUN dotnet restore 
RUN dotnet publish ./Blorem.Application.Scheduler/Blorem.Application.Scheduler.csproj -o /publish/
 
WORKDIR /publish
ENTRYPOINT ["dotnet", "Blorem.Application.Scheduler.dll"]