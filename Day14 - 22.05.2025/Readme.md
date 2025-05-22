# Day 14

## Topics

- Delegate and its working
- Predefined delegate (Action, Func, Predicate)
- Linq expression with a example project
    - https://github.com/tigerbluejay/FullLINQ101Implementations/tree/main/FullLINQ101ImplementationsSolution
- Extension Methods in c#
- Dependency Inversion and Dependency Injection
- Solid Principles
    - https://learn.microsoft.com/en-us/archive/msdn-magazine/2014/may/csharp-best-practices-dangers-of-violating-solid-principles-in-csharp
    
    - https://www.geeksforgeeks.org/solid-principle-in-programming-understand-with-real-life-examples/

## Delegate
 
 - It is pointer to method for holding its reference
 - Method signature must be same with the delegate type

## Predefined delegate

- `Action` - Does not return any return type (basically it should be `void`)
- `Func` - Returns a value
- `Predicate` - return a true/false based on the expression passed to it

## Extension function

- It is a special static function, which allows add a new methods to existing types (like string, int , etc)

- For this it should in `static` and `this` is lined with the first parameter
