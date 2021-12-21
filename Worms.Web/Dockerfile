#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Worms.Web.csproj", "Worms.Web/"]
#RUN dotnet restore "Worms.Web/Worms.Web.csproj"
#COPY . .
#WORKDIR "/src/Worms.Web"
#RUN dotnet build "Worms.Web.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Worms.Web.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Worms.Web.dll"]
FROM mcr.microsoft.com/dotnet/core/sdk AS build-env
WORKDIR /app

COPY ./../Worms.Domain ./
COPY Worms.Web.csproj ./Worms.Web/
WORKDIR /app/Worms.Web
RUN dotnet restore