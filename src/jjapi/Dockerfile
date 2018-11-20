FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 58835
EXPOSE 44354
EXPOSE 80


FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["jjapi/jjapi.csproj", "jjapi/"]
RUN dotnet restore "jjapi/jjapi.csproj"
COPY . .
WORKDIR "/src/jjapi"
RUN dotnet build "jjapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "jjapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "jjapi.dll"]