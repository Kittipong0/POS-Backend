FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["POS.API/POS.API.csproj", "POS.API/"]
COPY ["POS.Application/POS.Application.csproj", "POS.Application/"]
COPY ["POS.Infrastructure/POS.Infrastructure.csproj", "POS.Infrastructure/"]
COPY ["POS.Domain/POS.Domain.csproj", "POS.Domain/"]
COPY ["POS.Common/POS.Common.csproj", "POS.Common/"]
RUN dotnet restore "POS.API/POS.API.csproj"
COPY . .
WORKDIR "/src/POS.API"
RUN dotnet build "POS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "POS.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POS.API.dll"]
