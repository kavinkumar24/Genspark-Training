-- Table Schema:	 
-- Create Tables with Integrity Constrains:
-- a)	EMP (empno - primary key, empname, salary, deptname - references entries in a deptname of department table with null constraint, bossno - references entries in an empno of emp table with null constraint)
-- b)	DEPARTMENT (deptname - primary key, floor, phone, empno - references entries in an empno of emp table not null)
-- c)	SALES (salesno - primary key, saleqty, itemname -references entries in a itemname of item table with not null constraint, deptname - references entries in a deptname of department table with not null constraint)
-- d)	ITEM (itemname - primary key, itemtype, itemcolor)


CREATE TABLE EMP(
 empno INT PRIMARY KEY,
 empname VARCHAR(100) NOT NULL,
 salary DECIMAL(10,2),
 CHECK(salary>=0),
 deptname VARCHAR(50),
 bossno INT,

 FOREIGN KEY (bossno) REFERENCES EMP(empno)
);

CREATE TABLE DEPARTMENT(
  deptname VARCHAR(50) PRIMARY KEY,
  floor VARCHAR(50),
  phone VARCHAR(10), 
  CHECK(LEN(phone)=10),
  empno INT NOT NULL,

  FOREIGN KEY(empno) REFERENCES EMP(empno)
)


ALTER TABLE EMP
ADD CONSTRAINT fk_dep
FOREIGN KEY (deptname)
REFERENCES DEPARTMENT(deptname);

CREATE TABLE ITEM (
  itemname VARCHAR(100) PRIMARY KEY,
  itemtype VARCHAR(100),
  itemcolor VARCHAR(20)
)

CREATE TABLE SALES(
 saleno INT PRIMARY KEY,
 saleqty INT,
 CHECK(saleqty>=0),
 itemname VARCHAR(100) NOT NULL,
 deptname VARCHAR(50) NOT NULL,
	
 FOREIGN KEY(itemname) REFERENCES ITEM(itemname),
 FOREIGN KEY(deptname) REFERENCES DEPARTMENT(deptname)
)


EXEC sp_rename 'DEPARTMENT.empno', 'MgrId', 'COLUMN'

INSERT INTO EMP(empno, empname, salary, deptname, bossno)
VALUES 
(1, 'Alice', 75000, NULL,NULL),
(2, 'Ned', 75000, NULL,1),
(3, 'Andrew', 25000, NULL,2),
(4, 'Clare', 22000, NULL,2),
(5, 'Todd', 38000, NULL,1),
(6,'Nancy', 22000,NULL, 5),
(7,'Brier', 43000,NULL, 1),
(8,'Sarah', 56000,NULL, 7),
(9,'Sophile', 35000,NULL, 1),
(10,'Sanjay', 15000,NULL, 3),
(11,'Rita', 15000,NULL, 4),
(12,'Gigi', 16000,NULL, 4),
(13,'Maggie', 11000,NULL, 4),
(14,'Paul', 15000,NULL, 3),
(15,'James', 15000,NULL, 3),
(16,'Pat', 15000,NULL, 3),
(17,'Mark', 15000,NULL, 3)

INSERT INTO DEPARTMENT (deptname, floor, phone, MgrId)
VALUES
('Books', 1, 1234567881, 4),
('Clothes', 2, 1234567824, 4),
('Equipment', 3, 1234567857, 3),
('Furniture', 4, 1234567814, 3),
('Navigation', 1, 1234567411, 3),
('Recreation', 2, 1234567829, 4),
('Acconting', 5, 1234567835, 5),
('Purchasing', 5, 1234567836, 7),
('Personnel', 5, 1234567837, 9),
('Marketing', 5, 1234567838, 2)


UPDATE EMP
SET deptname = 'Marketing'
where empno IN (2,3,4)

UPDATE EMP
SET deptname = 'Acconting'
where empno IN (5,6)

UPDATE EMP
SET deptname = 'Purchasing'
where empno IN (7,8)

UPDATE EMP
SET deptname = 'Personnel'
where empno IN (9)

UPDATE EMP
SET deptname = 'Navigation'
where empno IN (10)

UPDATE EMP
SET deptname = 'Books'
where empno IN (11)

UPDATE EMP
SET deptname = 'Clothes'
where empno IN (12,13)

UPDATE EMP
SET deptname = 'Equipment'
where empno IN (14,15)

UPDATE EMP
SET deptname = 'Furniture'
where empno IN (16)

UPDATE EMP
SET deptname = 'Recreation'
where empno IN (17)


INSERT INTO ITEM(itemname, itemtype, itemcolor)
VALUES('Pocket Knife-Nile','E', 'Brown'),
('Pocket Knife-Avon','E', 'Brown'),
('Compass','N', '--'),
('Geo positioning system', 'N', '--'),
('Elephant Polo stick','R', 'Bamboo'),
('Camel Saddle','R', 'Bamboo'),
('Sextant','N', '--'),
('Map measure', 'N', '--'),
('Boots-snake proof', 'C', 'Green'),
('Pith Helmet', 'C', 'Khaki'),
('Hat-polar Explorer', 'C', 'White'),
('Exploring in 10 Easy Leassons', 'B', '--'),
('Hammock', 'F', 'Khaki'),
('How to win Foreign Friends', 'B', '--'),
('Map case', 'E', 'Brown'),
('Safari Chair', 'F', 'Khaki'),
('Safari cooking kit', 'F', 'Khaki'),
('Stetson', 'C', 'Black'),
('Tent - 2 person', 'F', 'Khaki'),
('Tent -8 personn', 'F', 'Khaki')


INSERT INTO SALES(saleno, saleqty, itemname, deptname)
VALUES(101,2,'Boots-snake proof', 'Clothes'),
(102,1,'Pith Helmet', 'Clothes'),
(103,1,'Sextant', 'Navigation'),
(104,3,'Hat-polar Explorer', 'Clothes'),
(105,5,'Pith Helmet', 'Equipment'),
(106,2,'Pocket Knife-Nile', 'Clothes'),
(107,3,'Pocket Knife-Nile', 'Recreation'),
(108,1,'Compass', 'Navigation'),
(109,2,'Geo positioning system', 'Navigation'),
(110,5,'Map measure', 'Navigation'),
(111,1,'Geo positioning system', 'Books'),
(112,1,'Sextant', 'Books'),
(113,3,'Pocket Knife-Nile', 'Books'),
(114,1,'Pocket Knife-Nile', 'Navigation'),
(115,1,'Pocket Knife-Nile', 'Equipment'),
(116,1,'Sextant', 'Clothes'),
(117,1,'Sextant', 'Equipment'),
(118,1,'Sextant', 'Recreation'),
(119,1,'Sextant', 'Furniture'),
(120,1,'Pocket Knife-Nile', 'Furniture'),
(121,1,'Exploring in 10 Easy Leassons', 'Books'),
(122,1,'How to win Foreign Friends', 'Books'),
(123,1,'Compass', 'Books'),
(124,1,'Pith Helmet', 'Books'),
(125,1,'Elephant Polo stick', 'Recreation'),
(126,1,'Camel Saddle', 'Recreation')


SELECT * from EMP
SELECT * from DEPARTMENT
SELECT * from SALES
SELECT * from ITEM
