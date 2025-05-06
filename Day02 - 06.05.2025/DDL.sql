
--categories
--id, name, status

CREATE TABLE categories(
  id varchar(50) PRIMARY KEY,
  name varchar(100),
  status varchar(20)
)

 ALTER TABLE categories
 ALTER COLUMN name varchar NOT NULL;
 
--country
--id, name

CREATE TABLE country(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL
)

--state
--id, name, country_id

CREATE TABLE state(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  country_id INT,
  FOREIGN KEY (country_id) REFERENCES country(id)
)
 
--City
--id, name, state_id

CREATE TABLE city(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  state_id INT,
  FOREIGN KEY (state_id) REFERENCES state(id)
)
 
--area
--zipcode, name, city_id

CREATE TABLE area(
  zipcode INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  city_id INT,
  FOREIGN KEY (city_id) REFERENCES city(id)
)
 
--address
--id, door_number, addressline1, zipcode

CREATE TABLE address(
  id INT PRIMARY KEY,
  door_number varchar(100) NOT NULL,
  addressline1 VARCHAR(255),
  zipcode INT,
  FOREIGN KEY (zipcode) REFERENCES area(zipcode)
)

 
--supplier
--id, name, contact_person, phone, email, address_id, status

CREATE TABLE supplier(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  contact_person varchar(100) NOT NULL,
  phone INT,
  email VARCHAR(100),
  address_id INT,
  FOREIGN KEY (address_id) REFERENCES address(id)
)

--product
--id, Name, unit_price, quantity, description, image
 
 CREATE TABLE product(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  unit_price DECIMAL NOT NULL,
  qunatity INT NOT NULL,
  description VARCHAR(255),
  image_path VARCHAR(255)
)


--product_supplier
--transaction_id, product_id, supplier_id, date_of_supply, quantity,

CREATE TABLE product_supplier(
  transaction_id INT PRIMARY KEY,
  product_id INT,
  supplier_id INT,
  date_of_supply DATE,
  quantity INT,
  
  FOREIGN KEY (product_id) REFERENCES product(id),
  FOREIGN KEY (supplier_id) REFERENCES supplier(id)
)

 
--Customer
--id, Name, Phone, age, address_id
 
  
 CREATE TABLE customer(
  id INT PRIMARY KEY,
  name varchar(100) NOT NULL,
  phone INT NOT NULL,
  age INT,
  address_id INT

  FOREIGN KEY (address_id) REFERENCES address(id)
)

ALTER TABLE customer
ADD CONSTRAINT chk_age_min
CHECK (age >=18)

--order
--order_number, customer_id, Date_of_order, amount, order_status

   
 CREATE TABLE orders(
  order_number INT PRIMARY KEY,
  customer_id INT,
  Date_of_order DATE,
  amount DECIMAL(10,2),
  order_status VARCHAR(50)

  FOREIGN KEY(customer_id) REFERENCES customer(id)
)
 
--order_details
--id, order_number, product_id, quantity, unit_price

  CREATE TABLE order_details(
  id INT PRIMARY KEY,
  order_number INT,
  product_id INT,
  quantity INT,
  unit_price DECIMAL(10,2),

  FOREIGN KEY(order_number) REFERENCES orders(order_number)
)