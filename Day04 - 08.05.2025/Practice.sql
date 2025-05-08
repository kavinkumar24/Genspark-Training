
USE pubs


-- Create Out Parameter type in stored procedure

CREATE PROC proc_filterProducts(@pcpu VARCHAR(20), @pcount INT OUT)
AS
BEGIN
SET @pcount = (SELECT COUNT(*) FROM products 
WHERE TRY_CAST(json_value(details, '$.spec.cpu')AS NVARCHAR(20)) = @pcpu)
END 

DECLARE @cnt INT
EXEC proc_filterProducts 'i5', @cnt OUT
PRINT @cnt


-- Demo table creation
CREATE TABLE people
(id INT primary key,
name NVARCHAR(20),
age INT)

-- Bulk Insert with stored Procedure

CREATE or ALTER proc proc_BulkInsert(@filepath NVARCHAR(500))
AS
BEGIN
DECLARE @insertQuery NVARCHAR(max)

SET @insertQuery = 'BULK INSERT people FROM '''+ @filepath +'''
with(
FIRSTROW =2,
FIELDTERMINATOR='','',
ROWTERMINATOR = ''\n'')'
EXEC sp_executesql @insertQuery
END

proc_BulkInsert 'D:\Training-\Genspark-Training\Day04 - 08.05.2025\Data.csv'

SELECT * FROM people



--- Status log table 
CREATE TABLE BulkInsertLog
(LogId INT identity(1,1) primary key,
FilePath NVARCHAR(1000),
status NVARCHAR(50) constraint chk_status Check(status in('Success','Failed')),
Message NVARCHAR(1000),
InsertedOn DATETIME default GetDate())


-- Stored procedure for status indication along with insertion
-- Added a Structural handling along with try, catch
CREATE or ALTER proc proc_BulkInsert(@filepath NVARCHAR(500))
AS
BEGIN
Begin TRY
	DECLARE @insertQuery NVARCHAR(max)
	SET @insertQuery = 'BULK INSERT people FROM '''+ @filepath +'''
	with(
	FIRSTROW =2,
	FIELDTERMINATOR='','',
	ROWTERMINATOR = ''\n'')'

	EXEC sp_executesql @insertQuery

	INSERT into BulkInsertLog(filepath,status,message)
	values(@filepath,'Success','Bulk INSERT completed')
END TRY
BEGIN catch
		INSERT into BulkInsertLog(filepath,status,message)
		values(@filepath,'Failed',Error_Message())
END Catch
END

proc_BulkInsert 'D:\Data.csv'  -- Tried different path

SELECT * FROM BulkInsertLog

truncate TABLE people
----


SELECT * FROM authors

----- CTE ------
-- It is Common Table Expression

with cteAuthors
AS
(SELECT au_id, concat(au_fname,' ',au_lname) author_name FROM authors)

update cteAuthors SET au_fname = 'Anne' where au_fname = 'Ann'

SELECT * FROM cteAuthors


-- Pagination with CTE

DECLARE @page INT =1, @pageSize INT=10;
with PaginatedBooks AS
( SELECT  title_id,title, price, ROW_Number() over (order by price desc) AS RowNum
FROM titles
)
SELECT * FROM PaginatedBooks where rowNUm between((@page-1)*@pageSize) and (@page*@pageSize)

--CREATE a sp that will take the page number and size AS param and PRINT the books

CREATE or ALTER PROCEDURE proc_pagination (@page INT,  @pagesize INT)
AS
BEGIN
WITH PaginatedBooks AS
( SELECT  title_id,title, price, ROW_Number() over (order by price desc) AS RowNum
FROM titles
)
SELECT * FROM PaginatedBooks where rowNUm between((@page-1)*(@pageSize+1)) and (@page*@pageSize)

END


EXEC proc_pagination @page = 1, @pagesize = 10

EXEC proc_pagination 1, 10


-- OFFSET - Newer version it tell go beyond the certain limit and fetch the result

SELECT  title_id,title, price
FROM titles
order by price desc
offset 10 rows fetch next 10 rows only


-- Functions
-- (dbo - database owner)
  
-- Scalar function - Returns a single value

CREATE function  fn_CalculateTax(@baseprice float, @tax float)
returns float
AS
BEGIN
	return (@baseprice +(@baseprice*@tax/100))
END

SELECT dbo.fn_CalculateTax(1000,10) AS tax

SELECT title,dbo.fn_CalculateTax(price,12) AS tax FROM titles


-- Table value function - returns a table itself
  
-- New version - Faster

CREATE function fn_tableSample(@minprice float)
returns TABLE
AS
	return SELECT title,price FROM titles where price>= @minprice

SELECT * FROM dbo.fn_tableSample(10)


-- Older version of Table valued function
-- but supports more logic

CREATE function fn_tableSampleOld(@minprice float)
returns @Result TABLE(Book_Name NVARCHAR(100), price float)
AS
BEGIN
	INSERT into @Result SELECT title,price FROM titles where price>= @minprice
RETURN 
END


SELECT * FROM dbo.fn_tableSampleOld(10)



---- Cursors ----
-- Cursors are used to iterate over a set of rows returned by a query and perform operations on each row.
SELECT * FROM titles


DECLARE @ctitle VARCHAR(100), @ctype VARCHAR(20)

DECLARE cursor_business CURSOR
FOR SELECT title, type FROM titles

OPEN cursor_business 

FETCH NEXT FROM cursor_business INTO @ctitle, @ctype

WHILE @@FETCH_STATUS = 0
BEGIN 
	PRINT @ctitle+'-- ' +'type: ' + @ctype
	FETCH NEXT FROM cursor_business INTO @ctitle, @ctype
END

CLOSE cursor_business
DEALLOCATE cursor_business
