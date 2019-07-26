FROM microsoft/dotnet:2.1-sdk as sdk-img
WORKDIR /app

COPY . .

RUN dotnet restore

WORKDIR app
COPY . .
RUN dotnet publish -c Release -o /bin_out

ENTRYPOINT ["dotnet", "Api.dll"]