FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled-composite AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ARG PIPELINE_ID=1 

WORKDIR /src
COPY . .

WORKDIR "/src/src/Modules/SmsManagerModule/Presentation/QuickCode.Turuncu.SmsManagerModule.Api"

RUN dotnet restore "QuickCode.Turuncu.SmsManagerModule.Api.csproj"

FROM build AS publish
RUN dotnet publish "QuickCode.Turuncu.SmsManagerModule.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:AssemblyVersion=1.0.0.${PIPELINE_ID} /p:GenerateCode=NO 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LD_LIBRARY_PATH=/usr/lib
ENTRYPOINT ["dotnet", "QuickCode.Turuncu.SmsManagerModule.Api.dll"]

