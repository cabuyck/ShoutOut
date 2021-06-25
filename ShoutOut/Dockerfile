#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ShoutOut/ShoutOut.csproj", "ShoutOut/"]
RUN dotnet restore "ShoutOut/ShoutOut.csproj"
COPY . .
WORKDIR "/src/ShoutOut"
RUN dotnet build "ShoutOut.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoutOut.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoutOut.dll"]