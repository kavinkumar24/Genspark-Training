
--- task
-- Partial rollback example in online e-commerce platform


------------------------products-table----------------------
CREATE TABLE tbl_products
(
  product_id SERIAL PRIMARY KEY,
  name TEXT,
  stock INT
);

INSERT INTO tbl_products (name, stock)
VALUES ('Titan Watch', 10),
('Polo t-shirts', 20);
------------------------------------------------------------

------------------------orders-table------------------------
CREATE TABLE orders
(
order_id SERIAL PRIMARY KEY,
customer_id INT, 
product_id INT NOT NULL REFERENCES tbl_products(product_id),
quantity INT,
status TEXT
);

INSERT INTO orders (customer_id, product_id, quantity, status)
VALUES (101, 1, 10, 'Purchased');
------------------------------------------------------------

------------------------payments-table----------------------

CREATE TABLE payments (
payment_id SERIAL PRIMARY KEY,
order_id INT NOT NULL REFERENCES orders(order_id),
amount NUMERIC(10,2),
status TEXT
);

------------------------------------------------------------

BEGIN;
	UPDATE tbl_products 
	SET stock = stock-1
	WHERE product_id = 1 AND stock>0;

	INSERT INTO orders (customer_id, product_id, quantity, status)
	VALUES (103, 1, 2, 'Pending');

	SAVEPOINT before_payment;

	INSERT INTO payments (order_id, amount, status)   -- Store Null value to non - null columns (order_id)
	VALUES (1,1000.00, 'Failed');

	ROLLBACK TO SAVEPOINT before_payment;

	UPDATE orders 
	SET status = 'Payment Failed'
	WHERE customer_id = 103 AND product_id =1;
	
COMMIT;

ABORT
SELECT * FROM tbl_products
SELECT * FROM orders
SELECT * FROM payments
----



