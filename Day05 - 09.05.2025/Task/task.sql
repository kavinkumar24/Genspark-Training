-- Cursor-Based Questions (5)
-- 1) Write a cursor that loops through all films and prints titles longer than 120 minutes.

DO $$
DECLARE 
	longer_title_cursor CURSOR FOR SELECT film_id, title, length FROM film;
	c_film_id INT;
	c_title TEXT;
	c_length INT;
BEGIN
	OPEN longer_title_cursor;
	LOOP 
		FETCH longer_title_cursor INTO c_film_id, c_title, c_length;
		EXIT WHEN NOT FOUND;
		IF c_length >120 THEN
			RAISE NOTICE 'Film ID: %, Title: %, Length: %', c_film_id, c_title, c_length;
		END IF;
	END LOOP;
	CLOSE longer_title_cursor;
	END;
$$ LANGUAGE plpgsql

SELECT * FROM customer

-- 2) Create a cursor that iterates through all customers and counts how many rentals each made.

SELECT * FROM rental
DO $$
DECLARE
	customers_rental CURSOR FOR 
	SELECT c.customer_id, CONCAT(c.first_name, ' ', c.last_name) AS Name, COUNT(r.rental_id) 
	AS Total_rentals 
	FROM customer c
	JOIN rental r ON c.customer_id = r.customer_id
	GROUP BY c.customer_id;

	c_customer_id INT;
	c_name TEXT; 
	c_rental_count INT;
BEGIN
	OPEN customers_rental;
	LOOP
		FETCH customers_rental INTO c_customer_id, c_name, c_rental_count;
		EXIT WHEN NOT FOUND;
		RAISE NOTICE 'CustomerID: %, Name: %, Rental Count: %',c_customer_id INT, c_name, c_rental_count;
    END LOOP;
	CLOSE customers_rental;
END;
$$ LANGUAGE plpgsql;

	
-- 3) Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

DO $$
DECLARE
	film_rental_rate_increase_cursor CURSOR FOR
	SELECT f.film_id FROM film f
	JOIN inventory i ON f.film_id = i.film_id
	LEFT JOIN rental r ON i.inventory_id = r.inventory_id
	GROUP BY f.film_id
	HAVING COUNT(r.rental_id)<5;

	flim_record RECORD;
	
BEGIN
	OPEN film_rental_rate_increase_cursor;
	LOOP 
		FETCH film_rental_rate_increase_cursor INTO flim_record;
		EXIT WHEN NOT FOUND;
		UPDATE film SET rental_rate = rental_rate+1
		WHERE film_id = flim_record.film_id;
	END LOOP;

	CLOSE film_rental_rate_increase_cursor;
END;
$$ LANGUAGE plpgsql;

-- 4) Create a function using a cursor that collects titles of all films FROM a particular category.

CREATE OR REPLACE FUNCTION get_flims_categories(category_name_input TEXT)
RETURNS TABLE(title TEXT, category TEXT) 
AS $$
DECLARE 
	film_category_cursor CURSOR FOR
	SELECT f.title, c.name FROM film f
	JOIN film_category fc ON f.film_id = fc.film_id
	JOIN category c ON fc.category_id = c.category_id
	WHERE c.name = category_name_input
	ORDER BY f.title;

	film_record RECORD;
BEGIN
	OPEN film_category_cursor;
	LOOP
		FETCH film_category_cursor INTO film_record;
		EXIT WHEN NOT FOUND;
		title:=film_record.title;
		category:=film_record.name;
		RETURN NEXT;
	END LOOP;

	CLOSE film_category_cursor;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_flims_categories('Action')


-- 5) Loop through all stores and count how many distinct films are available in each store using a cursor.

SELECT * FROM store
SELECT * FROM inventory

CREATE OR REPLACE FUNCTION get_distinct_films_stores()
RETURNS TABLE (store_id_film INT, film_count INT) AS $$
DECLARE
    film_distinct_cursor CURSOR FOR SELECT store_id FROM store;
    store_record RECORD;
    film_distinct INT;
BEGIN
    OPEN film_distinct_cursor;

    LOOP
        FETCH film_distinct_cursor INTO store_record;
        EXIT WHEN NOT FOUND;
        SELECT COUNT(DISTINCT film_id) INTO film_distinct
        FROM inventory i
        WHERE i.store_id = store_record.store_id;

        store_id_film := store_record.store_id;
        film_count := film_distinct;

        RETURN NEXT;

    END LOOP;

    CLOSE film_distinct_cursor;
    RETURN;

END;
$$ LANGUAGE plpgsql;


SELECT * FROM get_distinct_films_stores()




-- Trigger-Based Questions (5)
-- 6) Write a trigger that logs whenever a new customer is inserted.

SELECT * FROM customer

CREATE TABLE new_customer_logs (
log_id SERIAL CONSTRAINT pk_Cust_logId PRIMARY KEY,
customer_id INT,
log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_new_customer()
RETURNS TRIGGER 
AS $$
BEGIN
	INSERT INTO new_customer_logs(customer_id)
	VALUES (new.customer_id);
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER tr_tbl_customer_after_insert
AFTER INSERT ON customer
FOR EACH ROW
EXECUTE FUNCTION log_new_customer();


INSERT INTO customer(customer_id, store_id, first_name, last_name,email, address_id, activebool,
active)
VALUES (702, 2, 'vemu1', 'P', 'vemu1@customer.org', 118, true,1)

SELECT * FROM customer
SELECT * FROM new_customer_logs

-- 7) Create a trigger that prevents inserting a payment of amount 0.

CREATE OR REPLACE FUNCTION prevent_zero_level_payment ()
RETURNS TRIGGER
AS $$
BEGIN
	IF NEW.amount <= 0 THEN
		RAISE EXCEPTION 'Amount cannot be zero or negative';
	END IF;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER tr_tbl_payment_before_insert
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_zero_level_payment();


INSERT INTO payment (payment_id, customer_id, staff_id, rental_id, amount, payment_date)
VALUES (10003, 600, 2, 1778, 1, '2007-02-15 22:25:46.996577')

SELECT * FROM payment where payment_id = 10003


-- 8) Set up a trigger to automatically set last_update on the film table before update.

SELECT * FROM film

CREATE OR REPLACE FUNCTION set_latest_update()
RETURNS TRIGGER 
AS $$
BEGIN
	NEW.last_update:=CURRENT_TIMESTAMP;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_tbl_film_before_update
BEFORE UPDATE ON film
FOR EACH ROW 
EXECUTE FUNCTION set_latest_update();

UPDATE film SET release_year = 2025 WHERE film_id = 2
SELECT * FROM film WHERE film_id = 2;


-- 9) Create a trigger to log changes in the inventory table (insert/delete)

SELECT * FROM inventory

CREATE TABLE inventory_logs(
log_id SERIAL CONSTRAINT pk_logId PRIMARY KEY,
inventory_id INT,
film_id INT,
store_id INT,
action_message TEXT,
log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
)

CREATE OR REPLACE FUNCTION log_change_inventory_table()
RETURNS TRIGGER
AS $$
DECLARE 
	inventory_id INT;
	film_id INT;
	store_id INT;
	action TEXT;
BEGIN 
	action:=TG_OP;
	IF TG_OP = 'INSERT' THEN
		inventory_id := NEW.inventory_id;
		film_id := NEW.film_id;
		store_id := NEW.store_id;
	ELSIF TG_OP = 'DELETE' THEN 
		inventory_id := OLD.inventory_id;
		film_id := OLD.film_id;
		store_id := OLD.store_id;
	END IF;
	INSERT INTO inventory_logs(inventory_id,film_id, store_id, action_message)
	VALUES (inventory_id, film_id,store_id, action);
	RETURN NULL;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trigger_log_inventory_changes ON inventory

CREATE TRIGGER tr_tbl_inventory_after_insert
AFTER INSERT OR DELETE ON inventory
FOR EACH ROW 
EXECUTE FUNCTION log_change_inventory_table();

INSERT INTO inventory (inventory_id, film_id, store_id, last_update)
VALUES (10011, 223, 2, '2006-02-15 10:09:17' )

DELETE FROM inventory WHERE inventory_id = 10011

SELECT * FROM inventory_logs

-- 10) Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

SELECT * FROM customer


CREATE OR REPLACE FUNCTION movie_rental_prevention()
RETURNS TRIGGER
AS $$
DECLARE
  total_amount_paid NUMERIC;
BEGIN
  SELECT SUM(amount)
  INTO total_amount_paid
  FROM payment
  WHERE customer_id = NEW.customer_id;

  IF total_amount_paid > 50 THEN
    RAISE EXCEPTION 'Customer % has more than $50 payments and cannot rent more films.', NEW.customer_id;
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_tbl_rental_before_insert
BEFORE INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION movie_rental_prevention();


SELECT customer_id, SUM(amount) AS total_paid
FROM payment
GROUP BY customer_id
HAVING SUM(amount) < 50;

SELECT * FROM rental

INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (1004999, NOW(), 1, 110, NULL, 1, NOW());



-- Transaction-Based Questions (5)
-- 11) Write a transaction that inserts a customer and an initial rental in one atomic operation.

SELECT * FROM customer;
SELECT * FROM rental;

DO $$
DECLARE new_customer_id INT;	
BEGIN
	INSERT INTO customer (customer_id, store_id, first_name, last_name, email, address_id, activebool, create_date, last_update, active)
	VALUES (602, 1, 'Ramu', 'somu', 'ramsam@gmail.com', 1, true, CURRENT_DATE, NOW(),1)
	RETURNING customer_id INTO new_customer_id;

	INSERT INTO rental(rental_date, inventory_id, customer_id, staff_id, last_update)
	VALUES(NOW(), 1, new_customer_id, 1, NOW());
END;
$$ LANGUAGE plpgsql

SELECT * FROM customer WHERE customer_id = 602;
SELECT * FROM rental WHERE customer_id = 602;


-- 12) Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

SELECT * FROM inventory

DO $$
BEGIN
	UPDATE film SET rental_rate = rental_rate+1 
	WHERE film_id = 3;
	INSERT INTO inventory(film_id, store_id, last_update)
	VALUES (32, 1, NOW());
	RAISE NOTICE 'Update and Insertion successfully implemented';
EXCEPTION
	WHEN OTHERS THEN
		RAISE NOTICE 'Failed, rolling... back...';

		RAISE;
	END;
$$ LANGUAGE plpgsql


-- 13) Create a transaction that transfers an inventory item FROM one store to another.

DO $$
DECLARE
 	inventory_id_input INT := 100;
	 from_store_id INT :=1;
	 to_store_id INT := 2;
BEGIN
	IF NOT EXISTS (
	SELECT * FROM inventory 
	WHERE inventory_id = inventory_id_input AND
	store_id = from_store_id
	)
	THEN
	RAISE EXCEPTION 'Inventory Not found, Inventory id -- % ', inventory_id_input;
	END IF;

	UPDATE inventory SET
	store_id = to_store_id,
	last_update = CURRENT_TIMESTAMP 
	WHERE inventory_id = inventory_id_input;
	RAISE NOTICE 'Inventory Created successfully, Inventory id --%', inventory_id_input;
END;
$$;

SELECT * FROM inventory


-- 14) Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

SELECT * FROM payment

BEGIN;
	UPDATE payment
	SET amount = amount+1
	WHERE payment_id = 17503;

	UPDATE payment
	SET amount = amount+2
	WHERE payment_id = 17504;

	SAVEPOINT after_two_payments_update;

	UPDATE payment
	SET amount = amount+1
	WHERE payment_id = 17505;

	ROLLBACK TO SAVEPOINT after_two_payments_update;

COMMIT;

SELECT * FROM payment 
WHERE payment_id IN (17503,17504,17505)



-- 15) Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

DO $$
DECLARE 
	customer_id_input INT :=2;
BEGIN
	DELETE FROM payment WHERE customer_id = customer_id_input;

	DELETE FROM rental WHERE customer_id = customer_id_input;

	DELETE FROM customer WHERE customer_id = customer_id_input;

END;
$$
LANGUAGE plpgsql

SELECT * FROM customer WHERE customer_id = 2
SELECT * FROM rental WHERE customer_id = 2
SELECT * FROM payment WHERE customer_id =2
