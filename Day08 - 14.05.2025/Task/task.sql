-- Objective:
-- Create a stored procedure that inserts rental data on the primary server, and verify that changes replicate to the standby server. 
-- Add a logging mechanism to track each operation.

-- Tasks to Complete:
-- Set up streaming replication (if not already done):

-- Primary on port 5432

SELECT * from rental;

-- Standby on port 5433

-- Create a table on the primary:


CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    amount NUMERIC,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
-- Ensure this table is replicated.

-- Write a stored procedure to:

-- Insert a new rental log entry

-- Accept customer_id, film_id, amount as inputs

-- Wrap logic in a transaction with error handling (BEGIN...EXCEPTION...END)


CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, amount)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_amount);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;

-- Call the procedure on the primary:


CALL sp_add_rental_log(1, 100, 4.99);


-- On the standby (port 5433):

-- Confirm that the new record appears in rental_log

Run:SELECT * FROM rental_log ORDER BY log_id DESC LIMIT 1;

-- Add a trigger to log any UPDATE to rental_log


--- Trigger if any update happens in the rental_log table

CREATE TABLE rental_log_updates (
    log_id SERIAL PRIMARY KEY,
    rental_log_id INT,
    old_rental_time TIMESTAMP,
    new_rental_time TIMESTAMP,
    old_customer_id INT,
    new_customer_id INT,
    old_film_id INT,
    new_film_id INT,
    old_amount NUMERIC,
    new_amount NUMERIC,
    updated_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE OR REPLACE FUNCTION log_rental_update()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO rental_log_updates (
        rental_log_id,
        old_rental_time, new_rental_time,
        old_customer_id, new_customer_id,
        old_film_id, new_film_id,
        old_amount, new_amount
    )
    VALUES (
        OLD.log_id,
        OLD.rental_time, NEW.rental_time,
        OLD.customer_id, NEW.customer_id,
        OLD.film_id, NEW.film_id,
        OLD.amount, NEW.amount
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER trg_log_rental_updates
AFTER UPDATE ON rental_log
FOR EACH ROW
EXECUTE FUNCTION log_rental_update();

SELECT * FROM rental_log
UPDATE rental_log SET amount = amount*2 
WHERE customer_id = 1;

SELECT * FROM rental_log_updates



-- Primary server is on port 5432 which is the default port (running by default)
-- psql -U postgres -p 5432 -d postgres -c "CREATE ROLE replicator WITH REPLICATION LOGIN PASSWORD 'primary_root';"

-- Backup the primary server data
-- pg_basebackup -D d:\sec -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433

-- Set up the standby server
-- (db is initialized already)
-- pg_ctl -D D:\sec -o "-p 5433" -l d:\sec\logfile start
-- which will start the server on port 5433
-- psql -p 5433 -d postgres 
-- which will connect to the standby server 
-- and interactive mode is enabled


-- Status check in primary server
select * from pg_stat_replication;

-- which will show the status of the primary server like pid, usename, application_name, client_addr, client_port, backend_start, state, sent_lsn, write_lsn, flush_lsn, replay_lsn, sync_priority, sync_state

-- Status check in standby server
select pg_is_in_recovery();

-- which will show that recovery is on or off (by t or f)

-- Commands 
-- \dt
-- List tables in the current schema
-- \dt *.*
-- List tables in all schemas
-- \d tablename
-- Show the structure (columns, types) of a table
-- \l
-- List all databases (you already know this one!)
-- \c dbname
-- Connect to another database

-------------------------------------------------------------------------
-- Steps need to do
-- 1. Create a replicator role on the primary server
-- 2. Create a base backup of the primary server
-- 3. Set up the standby server
-- 4. Start the standby server
-- 5. Check the status of the replication
-- 6. Create a table on the primary server (rental_log)
-- 7. Create a stored procedure to insert rental log data
-- 8. Call the stored procedure on the primary server
-- 9. Check the rental_log table on the standby server
-- 10. Create a trigger to log any update on the rental_log table
-- 11. Update the rental_log table
-- 12. Check the rental_log_updates table on the standby server
-- 13. Check the status of the replication
-- 14. Check the status of the standby server

