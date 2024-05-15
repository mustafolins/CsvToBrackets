# CsvToBrackets
 A simple restful web api for converting csv input into brackets.

 Production Swagger Url: https://csvtobrackets.azurewebsites.net/swagger/index.html

 ## Usage

Request:
 ```http

POST https://csvtobrackets.azurewebsites.net/CsvToBrackets
Content-Type: application/json
Accept: text/plain

"\"Test, One\",Dude,\"Testing\"\nOne,Three,\"Test\""
 ```

Response
```
[Test, One] [Dude] [Testing]
[One] [Three] [Test]
```

 Docker repository: https://hub.docker.com/repository/docker/mustafolins/csv-to-brackets/general

[![.NET](https://github.com/mustafolins/CsvToBrackets/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mustafolins/CsvToBrackets/actions/workflows/dotnet.yml)

[![Docker Image CI](https://github.com/mustafolins/CsvToBrackets/actions/workflows/docker-image.yml/badge.svg)](https://github.com/mustafolins/CsvToBrackets/actions/workflows/docker-image.yml)

[![Build and deploy ASP.Net Core app to Azure Web App - CsvToBrackets](https://github.com/mustafolins/CsvToBrackets/actions/workflows/main_csvtobrackets.yml/badge.svg)](https://github.com/mustafolins/CsvToBrackets/actions/workflows/main_csvtobrackets.yml)