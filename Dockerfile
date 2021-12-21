FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app
COPY . .
RUN dotnet restore "./Worms.Web/Worms.Web.csproj"
RUN dotnet build "./Worms.Web/Worms.Web.csproj" -c Release -o out
RUN dotnet publish "./Worms.Web/Worms.Web.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Worms.Web.dll"]

