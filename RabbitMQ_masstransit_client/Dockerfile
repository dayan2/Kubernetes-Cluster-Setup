#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitMQ_masstransit_client/RabbitMQ_masstransit_client.csproj", "RabbitMQ_masstransit_client/"]
RUN dotnet restore "RabbitMQ_masstransit_client/RabbitMQ_masstransit_client.csproj"
COPY . .
WORKDIR "/src/RabbitMQ_masstransit_client"
RUN dotnet build "RabbitMQ_masstransit_client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQ_masstransit_client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQ_masstransit_client.dll"]



#docker build -f .\RabbitMQ_masstransit_client\Dockerfile -t dayanmendis/rabbit_masstransit_client .
#docker push dayanmendis/rabbit_masstransit_client:latest
#
#
#docker build -f .\RabbitMQ_masstransit\Dockerfile -t dayanmendis/rabbit_masstransit_server .
#docker push dayanmendis/rabbit_masstransit_server:lates.