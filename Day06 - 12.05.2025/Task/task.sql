-- 1. Question:
--  In a transaction, if I perform multiple updates and an error happens in the third statement,
--  but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
--  Will my first two updates persist?

- No, first two updates will not persist without savepoint
- Transaction maintains atomicity either fully succeed or full fail, without savepoint there is no intermediate pointing

BEGIN;
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_id = 1;
UPDATE tbl_bank_accounts SET balance = balance + 100 WHERE account_id = 2;
UPDATE tbl_bank_accounts SET balance = balance - 200 WHERE account_id = 1;
UPDATE tbl_bank_accounts SET balance = balance + 'error' WHERE account_id = 2;
 
ROLLBACK;

------------------------------------------------------------------------------------------

-- 2. Question: 
-- Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance 
-- if the isolation level is set to READ COMMITTED?


- NO, Transaction B cannot read the new balance updated by Transaction A
- Read Committed only sees data that has been committed by other transactions
- By using Read Committed, Dirty Read is avoided
- Read Committed is a default level of isolation

-- Example: intial balance (in alice)= 1000
BEGIN;
UPDATE tbl_bank_accounts SET balance = balance + 10   -- 1010
WHERE account_name = 'Alice'

--- Other session
BEGIN;
SELECT balance FROM tbl_bank_accounts  -- 1000 (Original Balance not updated one)
WHERE account_name = 'Alice'

COMMIT;

------------------------------------------------------------------------------------------

-- 3. Question: 
-- What will happen if two concurrent transactions both execute:
-- UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
-- at the same time? Will one overwrite the other?

- Both transactions will not overwrite each other
- Postgresql uses locking mechanisim (serialize or row-level locking) 
- Two types of concurreny (Pessimitic and Optimistic)
- pessimitic is more suitable for highly secure apps like banking 
  (it holds the other transaction until the completion of the previous transaction)
- It locks data when a transaction starts and holds it until the transaction ends
  

BEGIN;
	UPDATE tbl_bank_accounts SET balance = balance - 100 
	WHERE account_name = 'Alice';

BEGIN;
	UPDATE tbl_bank_accounts SET balance = balance - 100 
	WHERE account_name = 'Alice';

-- Transaction 2 is blocked until the transaction 1 commits or roll back

SELECT balance FROM tbl_bank_accounts  

------------------------------------------------------------------------------------------

-- 4. Question:
-- If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?

- It will only undo the changes made after the savepoint after_alice -- not everything
- Savepoint is like a bookmark which saves upto the mark point (like partial roll back)

BEGIN;
	UPDATE tbl_bank_accounts SET balance = balance - 100 
	WHERE account_name = 'Alice';
	SAVEPOINT after_alice;
	UPDATE tbl_bank_accounts SET balance = balance - 100 
	WHERE account_name = 'Bob'; 
	
	ROLLBACK TO SAVEPOINT after_alice;
COMMIT;

------------------------------------------------------------------------------------------

-- 5. Question:
-- Which isolation level in PostgreSQL prevents phantom reads?

SERIALIZABLE - which prevents the phantom reads

- Trans1 run a select query with a condition
- Trans2 insert row that match the condition
- Trans1 runs same select query, it sees different rows 

------------------------------------------------------------------------------------------

-- 6. Question: 
-- Can Postgres perform a dirty read (reading uncommitted data from another transaction)?
- No, Postgresql does not support dirty reads, its low level of isolation is READ COMMITTED only (default)
- Postgresql enforces MVCC and it guarantees no dirty reads under any level of isolation
- `SHOW transaction_isolation` - This command used to view the current session''s isolation level

SHOW transaction_isolation

BEGIN;
UPDATE tbl_bank_accounts SET balance = 100 
WHERE account_name = 'Alice';

-- Other sessions --
BEGIN;
SELECT balance FROM tbl_bank_accounts 
WHERE account_name = 'Alice';

------------------------------------------------------------------------------------------

-- 7. Question:
-- If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?

- Yes, If autocommit is on (default in psql) and when UPDATE is executed, the change is immediately committed
- In postgresaql, autocommit is enabled, each individual sql statement is executed and committed
- No need for explicit commit in the outer transaction queries

------------------------------------------------------------------------------------------

-- 8. Question:
-- If I do this:

-- BEGIN;
-- UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- -- (No COMMIT yet)
-- And from another session, I run:

-- SELECT balance FROM accounts WHERE id = 1;
-- Will the second session see the deducted balance?


- No, Session 2 will not see the updated balance
- by default transaction_isolation is READ COMMITTED which allows reading only committed data
- Session 1 has not committed, postgresql willnot expose uncommitted changes to other sessions
- This ensures data consistency and avoid dirty reads


------------------------------------------------------------------------------------------