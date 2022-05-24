# PaymentGateway
A simple API allowing merchants to process payments and retrieve payment details.

## Running the solution
### Visual Studio
- Set PaymentGateway.Api as the startup project
- Run the profile you want (Project or Docker)

### dotnet CLI 
- Run `dotnet run --project PaymentGateway.Api` from the root folder
- Access the API at https://localhost:7256

### Docker 
- Run the following commands from the root folder.
```
docker build -f PaymentGateway.Api/Dockerfile -t payment-gateway-api .
docker run -p 0.0.0.0:9888:80/tcp payment-gateway-api
```
- Access the API at http://localhost:9888

## Architecture
The solution is divided into 4 projects, following the [Clean Architecture template](https://github.com/jasontaylordev/CleanArchitecture).

## Assumptions
- Acquiring Bank component communicates over HTTP (as well as our API does). Therefore, its interface is asynchronous with return types of `HttpResponseMessage`. Since we are only simulating it, we are hosting it together with our API.
- Due to time constraints and simplicity, there is no authorization. Furthermore, it was not part of the requirements and we could have different complexity implementations for it.
- Due to time constraints and simplicity, the state is only persisted in memory. It would be trivial to upgrade it by using Microsoft.EntityFrameworkCore.SqlServer or similar package and then configure it in the startup.

## Areas for improvement
- More thorough input validation
- Caching

## Minor technical details
(https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#composite-value-objects)
(https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#non-nullable-properties-and-initialization)