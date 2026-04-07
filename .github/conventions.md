## Endpoint design
- Use plural nouns for resource names (e.g., `/users`, `/products`).
- Use lowercase letters and hyphens to separate words (e.g., `/user-profiles`).
- Use HTTP methods to indicate the action (e.g., `GET /users` to retrieve users, `POST /users` to create a new user).
- Include versioning in the URL (e.g., `/v1/users`) to allow for future changes without breaking existing clients.
- The method should have structure like:
    try {
        var serviceResponce = service.MethodName(request if needed);
        logger.LogInformation($"MethodName: {serviceResponce}", message about what happened);
        return Ok(dto model);
    }
    catch (Exception ex) {
        logger.LogError(ex, $"MethodName: {ex.Message}", message about what happened);
        return StatusCode(500, "An error occurred while processing your request.");
    }

## Service design
- Services should be designed to handle specific business logic and should be reusable across different controllers.
- Each service method should have a clear and specific purpose, and should not be responsible for handling HTTP requests or responses.
- Services should return data in a format that can be easily consumed by the controllers, such as DTOs (Data Transfer Objects) or domain models.
- Services should handle exceptions internally and should not throw exceptions to the controllers. Instead, they should return error information in a structured format that the controllers can use to generate appropriate HTTP responses.

-Service methods should return at least a boolean indicating success or failure in current implementations of the software. 

- In future version of software service methods should return at least a status code and a message, and optionally a data object if the operation was successful. For example:
    public ServiceResponse<YourDto> MethodName(YourRequest request) {
        try {
            // Business logic here
            var data = // result of business logic
            return new ServiceResponse<YourDto> {
                StatusCode = 200,
                Message = "Operation successful",
                Data = data
            };
        }
        catch (Exception ex) {
            // Log the exception
            logger.LogError(ex, $"MethodName: {ex.Message}", message about what happened);
            return new ServiceResponse<YourDto> {
                StatusCode = 500,
                Message = "An error occurred while processing your request."
            };
        }
    }

## Repository design
- Repositories should be designed to handle data access, basic CRUD operations and should be reusable across different services.
- Map to exactly one dataset (e.g., a database table or a collection in a NoSQL database).
- Return data in a format that can be easily consumed by the services, such as domain models or DTOs.
- Inherit from a common interface (e.g., `IRepository<T>`) that defines basic CRUD operations (e.g., `GetById`, `GetAll`, `Add`, `Update`, `Delete`).
- Repositories should throw exceptions to the services when data access errors occur, allowing the services to handle them appropriately and return structured error information to the controllers.