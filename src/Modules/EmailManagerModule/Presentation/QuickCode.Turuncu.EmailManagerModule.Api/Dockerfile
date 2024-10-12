FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PIPELINE_ID=1 

WORKDIR /src
COPY . .

WORKDIR "/src/src/Modules/EmailManagerModule/Presentation/QuickCode.Turuncu.EmailManagerModule.Api"

RUN dotnet restore "QuickCode.Turuncu.EmailManagerModule.Api.csproj"

FROM build AS publish
RUN dotnet publish "QuickCode.Turuncu.EmailManagerModule.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:AssemblyVersion=1.0.0.${PIPELINE_ID} /p:GenerateCode=NO 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "QuickCode.Turuncu.EmailManagerModule.Api.dll"]

