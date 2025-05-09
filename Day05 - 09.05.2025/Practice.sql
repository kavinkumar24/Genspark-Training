-- SELECT Queries
-- List all films with their length and rental rate, sorted by length descending.
-- Columns: title, length, rental_rate

SELECT title, length, rental_rate 
FROM film
ORDER BY length DESC

-- Find the top 5 customers who have rented the most films.
-- Hint: Use the rental and customer tables.
SELECT * from rental 
SELECT * from customer

SELECT c.customer_id, CONCAT(c.first_name, ' ', c.last_name) AS Name, COUNT(r.rental_id) AS total
FROM rental r
JOIN customer c ON r.customer_id = c.customer_id
GROUP BY c.customer_id, Name
ORDER BY total DESC
LIMIT 5;

-- Display all films that have never been rented.
-- Hint: Use LEFT JOIN between film and inventory → rental.
SELECT * from film
SELECT * from inventory
SELECT * from rental

SELECT f.film_id, f.title, r.rental_id
FROM film f
LEFT JOIN inventory i ON f.film_id = i.film_id
LEFT JOIN rental r ON i.inventory_id = r.inventory_id
WHERE r.rental_id IS NULL;

-- JOIN Queries
-- List all actors who appeared in the film ‘Academy Dinosaur’.
-- Tables: film, film_actor, actor
SELECT * from film
SELECT * from film_actor
SELECT * from actor

SELECT f.title, CONCAT(a.first_name, ' ', a.last_name) AS Actor_name
FROM film f
JOIN film_actor fa ON f.film_id = fa.film_id
JOIN actor a ON fa.actor_id = a.actor_id
WHERE f.title = 'Academy Dinosaur'


-- List each customer along with the total number of rentals they made and the total amount paid.
-- Tables: customer, rental, payment
SELECT * from customer
SELECT * from rental
SELECT * from payment 

SELECT CONCAT(c.first_name, ' ', c.last_name) AS Customer_name, COUNT(r.rental_id) AS Total_rental,
SUM(p.amount) AS Total_amount_paid
FROM customer c
JOIN rental r ON c.customer_id = r.customer_id
JOIN payment p ON c.customer_id = p.customer_id
GROUP BY c.customer_id, p.amount;


-- CTE-Based Queries
-- Using a CTE, show the top 3 rented movies by number of rentals.
-- Columns: title, rental_count

	WITH cte_filmRentalCount 
	AS ( SELECT f.film_id, f.title, COUNT(r.rental_id) AS rental_count 
	FROM film f
	JOIN inventory i ON f.film_id = i.film_id
	JOIN rental r ON i.inventory_id = r.inventory_id
	GROUP BY f.film_id, f.title
	)

SELECT  film_id, title, rental_count 
FROM cte_filmRentalCount
ORDER BY rental_count DESC
LIMIT 3

-- Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.
SELECT * from rental

WITH cte_customerRentalCounts AS (
SELECT customer_id, COUNT(rental_id) AS total_rentals
FROM rental
GROUP BY customer_id
),
cte_averageRentals AS (
  SELECT AVG(total_rentals) AS avgerage_rentals
  FROM cte_customerRentalCounts
)
SELECT c.customer_id, CONCAT(c.first_name, ' ', c.last_name) AS Customer_name, cr.total_rentals 
FROM cte_customerRentalCounts cr
JOIN cte_averageRentals ar ON 1=1
JOIN customer c ON c.customer_id = cr.customer_id
WHERE cr.total_rentals > ar.avgerage_rentals
ORDER BY cr.total_rentals DESC;

--  Function Questions
-- Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)
CREATE FUNCTION get_total_rentals(customer_id_input INT)
RETURNS INT AS $$
DECLARE total_no_rentals INT;
BEGIN
	SELECT COUNT(*) INTO total_no_rentals  FROM rental 
	WHERE customer_id = customer_id_input
	GROUP BY customer_id;
	RETURN total_no_rentals;
END;
$$ LANGUAGE plpgsql;


SELECT get_total_rentals(2) 



-- Stored Procedure Questions
-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)

SELECT * from film  ORDER BY film_id

CREATE PROCEDURE update_rental_rate(film_id_input INT, new_rate NUMERIC)
AS $$
BEGIN 
	UPDATE film SET rental_rate = new_rate 
	WHERE film_id = film_id_input;
END;
$$ LANGUAGE plpgsql

CALL update_rental_rate(1,3.69)


	