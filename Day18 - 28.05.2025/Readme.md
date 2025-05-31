# Day 18

## Topics

- Thread Pooling
- Multi tasking
- RollBack Migration
- AddTransient, AddScoped, AddSingleton
- aync and await functions
- Fluent API


## Thread Pooling
- Thread pooling is a technique that manages a collection of pre-initialized, reusable threads to execute tasks. This reduces the overhead of creating and destroying threads for each new task, improving performance and resource utilization

## Roll back migrations
- During the migration anything went wrong we can go back using

```
dotnet ef database update <TargetMigrationName>
```

## Dependency Injection Lifecycles

### AddTransient
- A new instance of the service is created every time it is requested

### AddScoped
- A single instance of the service is created per scope. In ASP.NET Core web applications, a scope typically corresponds to a single HTTP request.

### AddSingleton
- A single instance of the service is created the first time it is requested (or when the DI container is built if eagerly loaded). 

## Difference between lazy and eager loading

### Lazy loading
- When you query for an entity (e.g., a User), its related entities (e.g., a List<Post> for that user) are not loaded from the database immediately. Instead, they are loaded only when you explicitly access the navigation property

### Eager Loading
- tell the ORM to also load its related entities 

## Fluent API
- Fluent API is a programming style that makes code more readable and expressive by allowing you to "chain" multiple method calls together, where each method returns the object itself (or a related object) so you can immediately call another method on the result.