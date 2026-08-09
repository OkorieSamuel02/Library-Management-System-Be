FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src


COPY LibraryManagementSystem.sln .

COPY LibraryManagementSystem.Api/LibraryManagementSystem.Api.csproj LibraryManagementSystem.Api/
COPY LibraryManagementSystem.Application/LibraryManagementSystem.Application.csproj LibraryManagementSystem.Application/
COPY LibraryManagementSystem.Domain/LibraryManagementSystem.Domain.csproj LibraryManagementSystem.Domain/
COPY LibraryManagementSystem.Infrastructure/LibraryManagementSystem.Infrastructure.csproj LibraryManagementSystem.Infrastructure/


RUN dotnet restore


COPY . .


WORKDIR /src/LibraryManagementSystem.Api

RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LibraryManagementSystem.Api.dll"]