FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled-composite AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG PIPELINE_ID=1 

WORKDIR /src
COPY . .

WORKDIR "/src/src/Services/QuickCode.Turuncu.EventListenerService"

RUN dotnet restore "QuickCode.Turuncu.EventListenerService.csproj"

FROM build AS publish
RUN dotnet publish "QuickCode.Turuncu.EventListenerService.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:AssemblyVersion=1.0.0.${PIPELINE_ID} /p:GenerateCode=NO 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "QuickCode.Turuncu.EventListenerService.dll"]