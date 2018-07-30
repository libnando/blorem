FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . . 
RUN dotnet restore 
RUN dotnet publish ./Blorem.Presentation.Main/Blorem.Presentation.Main.csproj -o /publish/
 
WORKDIR /publish
ENTRYPOINT ["dotnet", "Blorem.Presentation.Main.dll"]