# Day 15

## Topics

- Solid Principles recap
- Memory blockage (does not follow SRP)
- Design Patterns
- Type of Design Patters
- Practical Implementation of some patterns
- Task based on Proxy and Singleton Design Pattern


## Memory Blockage

- There will three types of objects in heap 
    - `0th` block - storing latest block
    - `1st` block - moved when there is no reference
    - `2nd` block

- So When SRP is not properly implemented there many be memory issues that all the objects be in the same level which makes the process slow

## Design Patterns

- Design patterns are typical solutions to commonly occurring problems in software design. They are like pre-made blueprints that you can customize to solve a recurring design problem in your code.

### Types

- Creational patterns
- Structural patterns 
- Behavioral patterns

## Singleton pattern

- Ensures a class has only one instance and provides a global point of access to it.

## Factory Method

- Defines an interface for creating objects, but lets subclasses decide which class to instantiate.

## Abstract Factory

- Provides an interface to create families of related objects without specifying their concrete classes.

## Adapter

- Allows incompatible interfaces to work together by wrapping one interface with another the client expects.

## Flyweight

- Minimizes memory use by sharing common object data instead of creating many similar objects.

## Proxy

- Provides a surrogate or placeholder for another object to control access, add security, or manage resources.
