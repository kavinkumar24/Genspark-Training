
-- 1. Create a stored procedure to encrypt a given text
-- Task: Write a stored procedure sp_encrypt_text that takes a plain text input 
-- (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE OR REPLACE PROCEDURE sp_encrypt_text(IN p_plain_text TEXT, OUT p_encrypted_text BYTEA)
LANGUAGE plpgsql
AS $$
DECLARE 
	secret_key TEXT := 'secret_info';
BEGIN
	p_encrypted_text := pgp_sym_encrypt(p_plain_text, secret_key);
END;
$$;

DO $$
DECLARE 
	encrypted_text BYTEA;
BEGIN
	CALL sp_encrypt_text('sample', encrypted_text);
	RAISE NOTICE 'Encrypted %', encrypted_text;
END $$;

----------------------------------------------------------------------------------------------------------------------------------

-- 2. Create a stored procedure to compare two encrypted texts
-- Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.
-- 

CREATE OR REPLACE PROCEDURE sp_compare_encrypted(IN p_encrypted_text1 BYTEA, IN p_encrypted_text2 BYTEA, OUT p_is_equal BOOLEAN)
LANGUAGE plpgsql
AS $$
DECLARE 
	secret_key TEXT := 'secret_data_password';
	p_decrypted_text1 TEXT;
	p_decrypted_text2 TEXT;
BEGIN
	BEGIN
		p_decrypted_text1 := pgp_sym_decrypt(p_encrypted_text1, secret_key);
		p_decrypted_text2 := pgp_sym_decrypt(p_encrypted_text2, secret_key);
		p_is_equal := (p_decrypted_text1 = p_decrypted_text2);
		EXCEPTION
			WHEN OTHERS THEN
				p_is_equal := FALSE;
	END;
END $$;

DO 
$$
DECLARE
	encrypted_data1 BYTEA;
    encrypted_data2 BYTEA;
	secret_key TEXT := 'secret_data_password';
	is_equal BOOLEAN;
BEGIN
	encrypted_data1 = pgp_sym_encrypt('Sample1', secret_key);
	encrypted_data2 = pgp_sym_encrypt('Sample1', secret_key);
	
	CALL sp_compare_encrypted (encrypted_data1, encrypted_data2, is_equal);

	IF is_equal THEN 
		RAISE NOTICE 'Two texts are equal %(bool)', is_equal;
	ELSE
		RAISE NOTICE 'Two texts are not equal %(bool)', is_equal;
	END IF;
END;
$$;

----------------------------------------------------------------------------------------------------------------------------------

--  3. Create a stored procedure to partially mask a given text
-- Task: Write a procedure sp_mask_text that:

-- Shows only the first 2 and last 2 characters of the input string

-- Masks the rest with *

-- E.g., input: 'john.doe@example.com' → output: 'jo***************om'

CREATE PROCEDURE sp_mask_text (IN p_text TEXT, OUT p_masked_text TEXT)
AS $$
DECLARE 
	text_length INT := LENGTH (p_text);
BEGIN
	IF text_length>=4 THEN
		p_masked_text := 
			SUBSTRING(p_text FROM 1 FOR 2) || 
			REPEAT('*',text_length-4) ||
			SUBSTRING(p_text FROM text_length-1 FOR 2);
	ELSE 
		p_masked_text:= p_text;
	END IF;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE 
	masked_text_out TEXT;
BEGIN
	CALL sp_mask_text('sam', masked_text_out);
	RAISE NOTICE 'masked text - %', masked_text_out;
END;
$$;

----------------------------------------------------------------------------------------------------------------------------------

-- 4. Create a procedure to insert into customer with encrypted email and masked name
-- Task:

-- Call sp_encrypt_text for email

-- Call sp_mask_text for first_name

-- Insert masked and encrypted values into the customer table

-- Use any valid address_id and store_id to satisfy FK constraints.

SELECT * FROM customer

ALTER TABLE customer
ALTER COLUMN email TYPE TEXT;

CREATE OR REPLACE PROCEDURE sp_insert_new_customer 
(IN p_store_id INT, IN p_first_name TEXT, IN p_last_name TEXT, IN p_email TEXT, 
IN p_address_id INT, IN p_activebool BOOLEAN, IN p_create_date DATE)
LANGUAGE plpgsql
AS $$
DECLARE 
	encrypted_email BYTEA;
	masked_first_name TEXT;
BEGIN
	BEGIN
		CALL sp_encrypt_text(p_email, encrypted_email); -- encrypt email
		CALL sp_mask_text(p_first_name, masked_first_name); -- mask the first name
		INSERT INTO customer (store_id, first_name, last_name, email, address_id, activebool, create_date)
		VALUES (p_store_id, masked_first_name, p_last_name, encode(encrypted_email, 'base64'), p_address_id, p_activebool, p_create_date);
		RAISE NOTICE 'New customer added';
	EXCEPTION
		WHEN OTHERS THEN
			RAISE NOTICE 'Error %', sqlerrm;
	END;
END;
$$;

CALL sp_insert_new_customer (2, 'Kavin', 'P', 'kavinsample@gmail.com', 5, true, CURRENT_DATE)
CALL sp_insert_new_customer (2, 'Kumar', 'P', 'kumarsample@gmail.com', 5, true, CURRENT_DATE)
CALL sp_insert_new_customer (2, 'Johnbob', 'KL', 'wickbob@gmail.com', 5, true, CURRENT_DATE)


SELECT * FROM customer ORDER BY create_date DESC;

----------------------------------------------------------------------------------------------------------------------------------

-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task:
-- Write sp_read_customer_masked() that:

-- Loops through all rows

-- Decrypts email

-- Displays customer_id, masked first name, and decrypted email
DROP PROCEDURE sp_read_customer_masked

CREATE PROCEDURE sp_read_customer_masked()
LANGUAGE plpgsql
AS $$
DECLARE 
	secret_key TEXT := 'secret_info';  -- use the same secret key used while encryption
	customer_cursor CURSOR FOR SELECT customer_id, first_name, email FROM customer WHERE create_date = CURRENT_DATE;
	row_data RECORD;
	decrypted_email TEXT;
	masked_first_name TEXT;
BEGIN
	OPEN customer_cursor;

	LOOP
		FETCH customer_cursor INTO row_data;
		EXIT WHEN NOT FOUND;

		BEGIN
			decrypted_email := pgp_sym_decrypt(decode(row_data.email,'base64'),secret_key);
		EXCEPTION 
			WHEN OTHERS THEN
				decrypted_email := 'decryption failed';
		END;
		RAISE NOTICE 'Customer_id - %, First_name - %, email - %', row_data.customer_id, row_data.first_name, decrypted_email;
	END LOOP;
	CLOSE customer_cursor;
END;
$$;

CALL sp_read_customer_masked()


		