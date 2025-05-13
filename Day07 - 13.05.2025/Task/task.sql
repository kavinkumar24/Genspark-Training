-- Cursors 
-- Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.
SELECT * FROM CUSTOMER

CREATE TABLE summary_of_customer_rentals (
	s_no SERIAL PRIMARY KEY,
	customer_id INT,
	customer_name TEXT,
	no_of_rentals TEXT
);

DO $$
DECLARE 
	row_data record;
	cursor_rentals CURSOR FOR
	SELECT c.customer_id , CONCAT(c.first_name, ' ', c.last_name) as Name, COUNT(r.rental_id) AS no_of_rentals
	FROM customer c 
	JOIN rental r ON c.customer_id = r.customer_id
	GROUP BY c.customer_id;

BEGIN
	OPEN cursor_rentals;
	LOOP
		FETCH cursor_rentals INTO row_data;
		EXIT WHEN NOT FOUND;

		INSERT INTO summary_of_customer_rentals (customer_id, customer_name, no_of_rentals)
		VALUES (
			row_data.customer_id,
			row_data.Name,
			row_data.no_of_rentals
		);
	END LOOP;
	CLOSE cursor_rentals;
END;
$$;

SELECT * FROM summary_of_customer_rentals

---------------------------------------------------------------------------------------------------------------------------------

-- Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.
SELECT * FROM inventory
SELECT * FROM film_category

DO $$
DECLARE 
	film_title TEXT;
	film_comedy_cursor CURSOR FOR
	SELECT f.title, COUNT(r.rental_id)
	FROM film f
	JOIN film_category fc ON f.film_id = fc.film_id
	JOIN category c ON fc.category_id = c.category_id
	JOIN inventory i ON f.film_id = i.film_id
	JOIN rental r ON i.inventory_id = r.inventory_id
	WHERE c.name = 'Comedy'
	GROUP BY f.title
	HAVING COUNT(r.rental_id) > 10;

BEGIN 
	OPEN film_comedy_cursor;
	LOOP
		FETCH film_comedy_cursor INTO film_title;
		EXIT WHEN NOT FOUND;
		RAISE NOTICE 'Title: %', film_title;
	END LOOP;
	CLOSE film_comedy_cursor;
END;
$$;

---------------------------------------------------------------------------------------------------------------------------------

-- Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
CREATE TABLE report(
	s_no SERIAL PRIMARY KEY,
	store_id INT,
	distinct_count INT
);

DO $$
DECLARE 
	row_data record;
	ditinct_film_cursor CURSOR FOR
	SELECT COUNT(DISTINCT i.film_id) as distinct_count, s.store_id
	FROM inventory i 
	JOIN store s ON i.store_id = s.store_id
	GROUP BY s.store_id;

BEGIN
	OPEN ditinct_film_cursor;
	LOOP
		FETCH ditinct_film_cursor INTO row_data;
		EXIT WHEN NOT FOUND;
		INSERT INTO report (store_id, distinct_count)
		VALUES (row_data.store_id, row_data.distinct_count);
	END LOOP;
	CLOSE ditinct_film_cursor;
END;
$$;

SELECT * FROM report

---------------------------------------------------------------------------------------------------------------------------------

-- Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.

SELECT * FROM customer
SELECT * FROM rental

CREATE TABLE inactive_customers (
s_no SERIAL PRIMARY KEY,
customer_id INT,
customer_name TEXT
);

DO $$
DECLARE 
	row_data record;
	find_inactive_cursors CURSOR FOR
	SELECT c.customer_id, CONCAT(c.first_name, ' ', c.last_name) AS customer_name
	FROM customer c 
	WHERE c.customer_id NOT IN
	(
	SELECT DISTINCT r.customer_id
	FROM rental r
	WHERE r.rental_date >= CURRENT_DATE - INTERVAL '6 months'
	);

BEGIN
	OPEN find_inactive_cursors;
	
	LOOP
		FETCH find_inactive_cursors INTO row_data;
		EXIT WHEN NOT FOUND;
	
		INSERT INTO inactive_customers (customer_id,customer_name)
		VALUES (row_data.customer_id, row_data.customer_name);
	END LOOP;
	CLOSE find_inactive_cursors;
END;
$$;

SELECT * FROM inactive_customers

---------------------------------------------------------------------------------------------------------------------------------

-- Transactions 
-- Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.
SELECT * FROM customer
SELECT * FROM inventory

BEGIN;
WITH new_customer AS (
	INSERT INTO customer(store_id, first_name, last_name, email, address_id, activebool, create_date)
	VALUES (1, 'Alice','Bob','alicebob@customer.org',5, TRUE, NOW())
	RETURNING customer_id
),
new_rental AS (
INSERT INTO rental(rental_date, inventory_id, customer_id, staff_id)
SELECT NOW(), 200, customer_id, 2 FROM new_customer
RETURNING rental_id, customer_id
)
INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
SELECT customer_id, 2, rental_id, 4.66, NOW()
FROM new_rental;

COMMIT;

ABORT;

SELECT * FROM customer 
WHERE customer_id = 601
ORDER BY first_name 

SELECT * FROM payment 
WHERE customer_id = 601

---------------------------------------------------------------------------------------------------------------------------------

-- Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.
SELECT * FROM customer

DO $$
BEGIN
	BEGIN
		UPDATE customer
		SET email = 'bobalice@customer.org'
		WHERE customer_id = 601;

		UPDATE customer
		SET store_id = 100001
		WHERE customer_id = 601;

		COMMIT;
	EXCEPTION
		WHEN OTHERS THEN
			ROLLBACK;
			RAISE NOTICE 'Transaction failed, all changes are roll back';
		END;
END $$;

---------------------------------------------------------------------------------------------------------------------------------

-- Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.
SELECT * FROM payment

BEGIN;
	UPDATE payment
	SET amount = amount + 10
	WHERE payment_id = 18568;

	UPDATE payment
	SET amount = amount + 2
	WHERE payment_id = 18570;

	SAVEPOINT two_updated_happened;

	UPDATE payment
	SET amount = amount + 100
	WHERE payment_id = 22873;

	ROLLBACK TO SAVEPOINT two_updated_happened;

	UPDATE payment 
	SET amount = amount *2
	WHERE payment_id = 22873;

	COMMIT;
END;

SELECT * FROM payment 
WHERE payment_id = 22873;

---------------------------------------------------------------------------------------------------------------------------------

-- Perform a transaction that transfers inventory from one store to another (delete + insert) safely.
SELECT * FROM inventory 
WHERE inventory_id = 150;

DO $$
DECLARE
	i_inventory_id INT := 150;
	i_product_id INT;
	i_quantity INT;
BEGIN

	SELECT product_id, quantity
	INTO i_product_id, i_quantity
	FROM inventory 
	WHERE inventory_id = i_inventory_id AND store_id = 2;
	 
	DELETE FROM inventory
	WHERE inventory_id = i_inventory_id
	AND store_id = 2;
	
	SAVEPOINT before_insert_to_other_store;
	BEGIN
		INSERT INTO inventory (inventory_id, store_id, product_id, quantity, last_update)
		VALUES (i_inventory_id, 1, i_product_id,i_quantity, NOW());
	EXCEPTION
		WHEN OTHERS THEN
			RAISE NOTICE 'Insert failed so rolling back upto the savepoint';
			ROLLBACK To before_insert_to_other_store;
	END;
END $$;

SELECT * FROM inventory WHERE inventory_id = 150

ALTER TABLE rental
DROP CONSTRAINT rental_inventory_id_fkey;

ALTER TABLE rental ADD CONSTRAINT rental_inventory_id_fkey
FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id) ON DELETE CASCADE;

CREATE OR REPLACE PROCEDURE transfer_inventory(i_inventory_id INT)
LANGUAGE plpgsql
AS $$
DECLARE
    i_film_id INT;
BEGIN
    SELECT film_id
    INTO i_film_id
    FROM inventory
    WHERE inventory_id = i_inventory_id AND store_id = 2;

    DELETE FROM inventory
    WHERE inventory_id = i_inventory_id AND store_id = 2;

    BEGIN
        INSERT INTO inventory (inventory_id, store_id, film_id, last_update)
        VALUES (i_inventory_id, 1, i_film_id, NOW());

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE NOTICE 'Insert failed. Rolling back to savepoint.';
    END;
END;
$$;


CALL transfer_inventory(153)

---------------------------------------------------------------------------------------------------------------------------------

-- Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.
SELECT * FROM customer WHERE customer_id = 101;

BEGIN;
DELETE FROM payment
WHERE rental_id IN (SELECT rental_id FROM rental WHERE customer_id = 101);

DELETE FROM rental WHERE customer_id = 101;

DELETE FROM customer WHERE customer_id = 101;

COMMIT;

SELECT * FROM customer WHERE customer_id = 101;

---------------------------------------------------------------------------------------------------------------------------------

-- Triggers
-- Create a trigger to prevent inserting payments of zero or negative amount.

CREATE OR REPLACE FUNCTION prevent_undefined_level_payments ()
RETURNS TRIGGER
AS $$
BEGIN
	IF NEW.amount <= 0 THEN
		RAISE EXCEPTION 'Amount cannot be zero or negative';
	END IF;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER trg_tbl_payment_before_update
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_undefined_level_payments();


INSERT INTO payment (payment_id, customer_id, staff_id, rental_id, amount, payment_date)
VALUES (10004, 600, 2, 1778, 0, CURRENT_TIMESTAMP)

SELECT * FROM payment where payment_id = 10004

---------------------------------------------------------------------------------------------------------------------------------

-- Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

SELECT * FROM film WHERE film_id = 11

CREATE OR REPLACE FUNCTION last_update_film()
RETURNS TRIGGER 
AS $$
BEGIN
	IF NEW.title IS DISTINCT FROM OLD.title OR NEW.rental_rate IS DISTINCT FROM OLD.rental_rate THEN
		NEW.last_update := NOW();
	END IF;

	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_film_update_last_update
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION last_update_film();

UPDATE film SET
title = 'Aladdin Calendar Part 1' WHERE film_id = 10;

UPDATE film SET
rental_rate = rental_rate * 0.1 WHERE film_id = 11;

---------------------------------------------------------------------------------------------------------------------------------

-- Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.


CREATE TABLE rental_log (
log_id SERIAL PRIMARY KEY, 
film_id INT,
rental_count INT,
log_time TIMESTAMP DEFAULT NOW()
);


SELECT * FROM rental;

CREATE OR REPLACE FUNCTION log_film_rental()
RETURNS TRIGGER AS $$
DECLARE 
	i_film_id INT;
	i_count INT;
BEGIN
	SELECT film_id INTO i_film_id
	FROM inventory 
	WHERE inventory_id = NEW.inventory_id;

	SELECT COUNT(*) INTO i_count
	FROM rental r
	JOIN inventory i ON r.inventory_id = i.inventory_id
	WHERE i.film_id = i_film_id AND
	r.rental_date >= NOW() - INTERVAL '7 days';

	IF i_count >3 THEN
		INSERT INTO rental_log (film_id, rental_count)
		VALUES (i_film_id, i_count);
	END IF;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_film_rental_logging
AFTER INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION log_film_rental();

DROP TRIGGER trg_film_rental_logging on rental
SELECT * FROM rental where customer_id = 3
INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
VALUES 
(NOW(), 101, 3,1),
(NOW(), 102, 1,1),
(NOW(), 103, 3,1),
(NOW(), 104, 3,1),
(NOW(), 105, 3,1)

SELECT * FROM inventory WHERE inventory_id IN (101,102,103,104,105)
SELECT * FROM rental_log
-- ------------------------------------------------------------------------------