
-- 1. List all orders with the customer name and the employee who handled the order.
SELECT ord.OrderID, cus.CompanyName, CONCAT(emp.FirstName, ' ', emp.LastName) As Sales_Employee
FROM Orders ord
JOIN Customers cus ON ord.CustomerID = cus.CustomerID
JOIN Employees emp ON ord.EmployeeID = emp.EmployeeID 

-- 2) Get a list of products along with their category and supplier name.
SELECT p.ProductName, c.CategoryName, s.CompanyName As SupplierName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
JOIN Suppliers s ON p.SupplierID = s.SupplierID


-- 3) Show all orders and the products included in each order with quantity and unit price.
SELECT o.OrderId, p.ProductName, od.Quantity, od.UnitPrice
FROM Orders o
JOIN [Order Details] od ON o.OrderID = od.OrderID
JOIN Products p ON od.ProductID = p.ProductID

-- 4) List employees who report to other employees (manager-subordinate relationship).
SELECT * FROM Employees

SELECT CONCAT(e2.FirstName,' ', e2.LastName) Employee, CONCAT(e1.FirstName,' ', e1.LastName) Manager
FROM Employees e1
RIGHT JOIN Employees e2 ON e1.EmployeeID = e2.ReportsTo
ORDER BY Manager


-- 5) Display each customer and their total order count.

SELECT c.companyname, COUNT(o.orderid) AS Total_Order_Count
FROM Customers c
JOIN Orders o ON c.CustomerID = o.CustomerID
GROUP BY c.companyname
ORDER BY Total_Order_Count DESC


-- 6) Find the average unit price of products per category.
SELECT c.CategoryName, AVG(p.UnitPrice) AS Average_unit_price
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
GROUP BY c.CategoryName

-- 7) List customers where the contact title starts with 'Owner'.

SELECT CustomerID, CompanyName, ContactTitle 
FROM Customers
WHERE ContactTitle LIKE 'OWNER%'

-- 8) Show the top 5 most expensive products.
SELECT * FROM Products

SELECT TOP 5 ProductID, ProductName, UnitPrice
FROM Products
ORDER BY UnitPrice DESC;

-- Return the total sales amount (quantity × unit price) per order.
SELECT * FROM [Order Details]

SELECT  OrderID, SUM(Quantity * UnitPrice) AS Total_sales_per_order
FROM [Order Details]
GROUP BY OrderID
ORDER BY Total_sales_per_order DESC;


-- 10) Create a stored procedure that returns all orders for a given customer ID.

SELECT * FROM Orders

CREATE or ALTER  PROCEDURE proc_GetCustomerOrders @CustomerID NVARCHAR(100)
AS
BEGIN 
	SELECT OrderID, CustomerID, ShipName, ShipAddress, ShipCity, ShipCountry FROM Orders 
	WHERE CustomerID = @CustomerID
END
GO

EXEC proc_GetCustomerOrders @CustomerID = 'VINET'


-- 11) Write a stored procedure that inserts a new product.

CREATE OR ALTER PROCEDURE proc_InsertProduct
(@productName NVARCHAR(40), @nsupplierID INT, @categoryID INT, @quantityperUnit NVARCHAR(50), @unitPrice DECIMAL, 
@reorderLevel INT, @Discontinued INT)
AS
BEGIN
    INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, ReorderLevel, Discontinued)
    VALUES (@productName, @nsupplierID, @categoryID, @quantityperUnit, @unitPrice, @reorderLevel, @Discontinued)
END

EXEC proc_InsertProduct 'Sample1', 20, 2, '15-boxes X 2 kg', 21, 20, 0

SELECT * FROM Products



-- 12) Create a stored procedure that returns total sales per employee.

CREATE PROCEDURE Proc_GetTotalSales @EmployeeID INT
AS
BEGIN
	SELECT e.EmployeeID, CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
	SUM(od.UnitPrice * od.Quantity * (1-od.Discount)) AS TotalSales
	FROM Employees e
	JOIN Orders o 
	ON e.EmployeeID = o.EmployeeID
	JOIN [Order Details] od 
	ON o.OrderID =od.OrderID
	WHERE e.EmployeeID = @EmployeeID
	GROUP BY e.EmployeeID, e.FirstName, e.LastName
END
GO

EXEC Proc_GetTotalSales @EmployeeID = 2


 -- 13) Use a CTE to rank products by unit price within each category.

 WITH cte_rankProducts
 AS 
 ( SELECT ProductID, ProductName, CategoryID, UnitPrice, 
 ROW_NUMBER() OVER(PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS RankPrice
 FROM Products
 )
 SELECT * FROM cte_rankProducts 
 ORDER BY CategoryID, RankPrice


 -- 14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

 WITH cte_totalRevenue_Product
 AS
 ( SELECT p.ProductID, p.ProductName, SUM(od.UnitPrice * od.Quantity * (1-od.Discount)) AS TotalRevenue
 FROM Products p
 JOIN [Order Details] od 
 ON p.ProductID = od.ProductID
 GROUP BY p.ProductID, P.ProductName
 )
 SELECT * FROM cte_totalRevenue_Product
 WHERE TotalRevenue>1000
 ORDER BY TotalRevenue DESC;


 -- 15) Use a CTE with recursion to display employee hierarchy.

 WITH cte_empHierachy 
 AS
 (SELECT EmployeeID, CONCAT(FirstName,' ', LastName) AS Emp_Name, ReportsTo, 0 AS Employee_Level  
 FROM Employees 
 WHERE ReportsTo IS NULL

 UNION ALL

 SELECT e.EmployeeID, CONCAT(e.FirstName,' ', e.LastName) AS Emp_Name, e.ReportsTo, eh.Employee_Level +1
 FROM Employees e
 JOIN cte_empHierachy eh ON e.ReportsTo = eh.EmployeeID
 )
 SELECT * FROM cte_empHierachy
 ORDER BY Employee_Level, ReportsTo, EmployeeID;