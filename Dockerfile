FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
EXPOSE 9091
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/*.csproj ./src/
COPY test/car-pooling.UnitTest/*.csproj ./test/car-pooling.UnitTest/
COPY test/car-pooling.ComponentTest/*.csproj ./test/car-pooling.ComponentTest/
RUN dotnet restore
COPY . .
RUN dotnet build

FROM build AS testrunner
WORKDIR /source/test/car-pooling.UnitTest/
CMD ["dotnet", "test", "--logger:trx"]


FROM build AS publish
WORKDIR /source/src
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "car-pooling-api.dll"]