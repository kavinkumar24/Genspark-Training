# Day 6

## Sessions

### Morning 

- Transaction
- ACID Properties
- Auto Commit
- Isolation and its levels
- Concurrency and its types
- Lock to prevent lost updates
- Dirty reads
- Best Practice for Transaction

### Afternoon

- Task Based on the above topics 
- Scenario based queries for partial rollbacks


## Transaction

It is a sequence of one or more sql operations bundled together into a single unit.

## ACID property 

- `Atomicity`
    - Either Fully succeed or failed not a intermediate part until savepoint declared.
- `Consistency`
    - Remain one valid state to another valid state even after transactions.
- `Isolation` 
    - independently without interface (concurrency)
- `Durability` 
    - Changes are permanent, even if the system crashes

## Auto Commit

- Auto commit is a default one for a normally executing queries like DML, DDL
- Inside `Begin` or `Start Transaction`, nothing is auto - committed until the explicit `COMMIT`
- In sql `SET AUTOCOMMIT OFF/ON` (pgadmin)

## Isolation

- `Read Uncommitted` - Transactions can read data that has been modified but not yet committed by other transactions (allows dirty reads). **(Postgresql does not allow this)**
- `Read Committed` -Each statement sees only data committed before it begins; `avoids dirty reads`, but allows non-repeatable reads.
- `Repeatable Read` -All reads within a transaction see a consistent snapshot from the start of the transaction; prevents `non-repeatable reads.`
- `Serializable` - Transactions are executed with `full isolation` as if they were run one after another; prevents all concurrency anomalies.

## Concurrency 


- `MVCC (Multi-Version Concurrency Control)` - A concurrency control method where each transaction sees a consistent snapshot of the data without interfering with others, using multiple versions of rows.

- `Optimistic Locking`	- A locking strategy that assumes no conflict, validating data integrity at commit time using a version or timestamp.

- `Pessimistic Locking` -	A locking strategy that prevents conflicts by locking data immediately when accessed, blocking other transactions until the lock is released.

_For lost updates these above locks helps to get rid of that._

## Dirty Read

- Reading uncommitted data from another transaction (in other session) which might disappear later.