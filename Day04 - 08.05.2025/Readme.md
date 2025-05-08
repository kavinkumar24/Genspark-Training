# Day 4

## Sessions

### Morning 

Learned about
- OUT parameter in stored procedure
- Bulk Insertion of CSV file
- Structured hading
- CTE (Common Table Expression)
- Row Number and Offset
- Function
    - Types of functions
- Postgresql Intermediate questions practice

### Afternoon

Learned  about
- Cursors 
- Transactions, 
- Triggers

- Assignments in JOIN, Aggregate function, Stored Procedure, CTE, RowNumber.

### Out Parameter 

- Give a value output for later usage

### Bulk Insertion through csv

- created a stored procedure and used `FIRSTROW`, `FIELDTERMINATOR`, `ROWTERMINATOR` to map the values into table

### Structured Handling
 
- Used `BEGIN TRY` and `BEGIN CATCH` for handling an exception during modifications
- Content goes in-between

### CTE - Common Table Expression 

- It is a `temporary result set` and can can `reference` within a SELECT, INSERT, UPDATE, or DELETE statement.
- Mainly used in `RECURSIVE` functions

### Row Number

- It assigns a unique sequential number to each row
- Older method with many custom logic

### Offset

- It skips the specified number of rows before starting to return results in a query
- Newer version - faster

### Functions

- Declaring a function and use that function in a current program to avoid the redundancy

#### Types

- Scalar valued
      - It returns a single value.

- Table valued
      - It returns a table.

