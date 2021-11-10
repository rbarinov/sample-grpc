FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Sample.Grpc/Sample.Grpc.csproj", "Sample.Grpc/"]
RUN dotnet restore "Sample.Grpc/Sample.Grpc.csproj"
COPY . .
WORKDIR "/src/Sample.Grpc"
RUN dotnet build "Sample.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Grpc.dll"]
