-- 13 May 2025 - Task
-- 1. Try two concurrent updates to same row → see lock in action.
-- 2. Write a query using SELECT...FOR UPDATE and check how it locks row.
-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.
-- 4. Use pg_locks query to monitor active locks.
-- 5. Explore about Lock Modes


SELECT * FROM tbl_bank_accounts;

INSERT INTO tbl_bank_accounts (account_name, balance)
VALUES 
('Sommu', 2000),
('Rammu', 2500)

-- 1. Try two concurrent updates to same row → see lock in action.
BEGIN;
UPDATE tbl_bank_accounts
SET balance = balance + 100
WHERE account_id = 1;

COMMIT;

---- In other session -----
BEGIN;
UPDATE tbl_bank_accounts
SET balance = balance + 200
WHERE account_id = 1;
	
COMMIT; 


SELECT * FROM tbl_bank_accounts;

-- 2. Write a query using SELECT...FOR UPDATE and check how it locks row.

BEGIN;
SELECT * FROM tbl_bank_accounts 
WHERE account_id = 1 FOR UPDATE

-- Transaction 2 in other session is waiting for the update
UPDATE tbl_bank_accounts 
SET balance = balance - 100
WHERE account_id = 1;

COMMIT;


-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.

BEGIN;
UPDATE tbl_bank_accounts
SET balance = balance + 1000 
WHERE account_id = 1;

-- After trans 2 executed in session 2
BEGIN;
UPDATE tbl_bank_accounts 
SET balance = balance - 100 
WHERE account_id = 2;

------------
UPDATE tbl_bank_accounts
SET balance = balance + 2
WHERE account_id = 2;

SELECT * FROM tbl_bank_accounts;

-- 4. Use pg_locks query to monitor active locks.

SELECT *
FROM pg_locks 

SELECT locktype, mode, transactionid, page, relation::regclass
FROM pg_locks 
WHERE relation IS NOT NULL;


-- 5. Explore about Lock Modes
BEGIN;
LOCK TABLE tbl_bank_accounts IN ACCESS SHARE MODE;

BEGIN;
LOCK TABLE tbl_bank_accounts IN ROW SHARE MODE;

BEGIN;
LOCK TABLE tbl_bank_accounts IN EXCLUSIVE MODE;
COMMIT;

BEGIN; 
LOCK TABLE tbl_bank_accounts IN ACCESS EXCLUSIVE MODE
