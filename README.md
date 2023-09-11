# TaskManagementSystem
# Documentation

## Decisions

- The adoption of the Clean Architecture pattern using Domain Driven Design in this project is a judicious decision. By separating the business logic from the delivery mechanism, we have improved code manageability and performance. This pattern offers several benefits, including the ability to change the database without affecting the UI and the ease of implementing UI changes without impacting the system and business rules. Additionally, the business rules are highly testable and can be tested independently of the UI or database.

- The Unit of Work design pattern is a well-suited choice for this project because it facilitates the handling of multiple database interactions in a single request. This ensures that operations are handled as a single operation and that the system's integrity is maintained by rolling back failed operations..
- Hangfire serves as the Background service and it is provides a simple and reliable way to execute background tasks, such as sending emails, processing data, or performing other asynchronous or scheduled operations..
- Serialog is the logging framework of choice in this project. Its structured logging capability and automatic capture of key attributes and message context make it an excellent fit. Moreover, it has sinks that enable logging to file and cloud databases.
- Finally, the **Fluent Assertion** library is leveraged to enhance the readability and human-friendliness of our test assertions, while the **Moq** package helps us mock dependencies for testing, thus reducing development time.

## Tools

Here are the tools required to start the application:

- **Dotnet 7 SDK**: This project was developed using .NET 7. You can download it from https://dotnet.microsoft.com/en-us/download/dotnet/7.0
- **Dotnet EF**: This project uses EF code-first approach, to run migrations. Run the following command:
```
dotnet ef migrations add migrationMessage -c AppDbContext -p "yourlocalpath\src\TaskManagementSystem.Infrastructure\TaskManagementSystem.Infrastructure.csproj" -s "yourlocalpath\TaskManagementSystem\TaskManagementSystem\Infrastructure.csproj" -o Data/Migrations
```
For further documentation, study https://learn.microsoft.com/en-us/ef/core/cli/dotnet

- **Hangfire**: The project uses Hangfire as a framework for background job processing in .NET applications

## Running the Application
To run the application, follow these steps:


1. Clone the project to your local machine.
2. Navigate to the project's root folder and run `dotnet restore` to restore dependencies.
3. Run `dotnet test` to run the tests alone.
4. If the tests run successfully, proceed to run the application.
5. To start the application, run the command: `dotnet run`
