USE pubs

SELECT title, pub_name
FROM publishers LEFT JOIN titles
ON titles.pub_id = publishers.pub_id


SELECT * from titles

SELECT * from authors

SELECT * from titleauthor

SELECT title, pub_name 
FROM titles JOIN publishers
ON titles.pub_id = publishers.pub_id


-- print the publisher details of the publisher who has never published
SELECT *
FROM publishers 
WHERE pub_id not in 
(SELECT DISTINCT pub_id from titles)

-- Select the author_id for all the books, print the author_id and book name
select au_id, title from titleauthor join titles on titles.title_id = titleauthor.title_id;

select * from titleauthor

-- select also authour name so join in a already join query
SELECT CONCAT(authors.au_fname, ' ', authors.au_lname) AS Author_name, titles.title AS Book_name
FROM authors JOIN titleauthor
ON titleauthor.au_id = authors.au_id
JOIN titles ON titles.title_id = titleauthor.title_id



SELECT * from publishers
SELECT * from titles
SELECT * FROM Sales

-- Print the publisher's name, book name and the order date	of the books
SELECT publishers.pub_name, titles.title AS Book_Name, Sales.ord_date
FROM publishers JOIN titles on titles.pub_id = publishers.pub_id
JOIN sales ON titles.title_id = sales.title_id


-- Print the publisher name and first book sale date for all the publishers

SELECT publishers.pub_name, MIN(Sales.ord_date) AS first_sale
FROM publishers 
LEFT JOIN titles on titles.pub_id = publishers.pub_id
LEFT JOIN sales ON titles.title_id = sales.title_id
GROUP BY publishers.pub_name
ORDER BY first_sale DESC


-- Print the bookname and the store address of the sale

SELECT * FROM stores

SELECT * from titles
SELECT * from sales

SELECT titles.title, CONCAT(stores.stor_address, ' ', stores.city, ' ' , stores.state, ' ' , stores.zip) AS Store_address
FROM titles JOIN Sales 
ON sales.title_id = titles.title_id
JOIN stores ON stores.stor_id = sales.stor_id
ORDER BY 1;

---- Stored Procedure -----

Create PROCEDURE proc_FirstProcedure
as
begin
   print 'Hello world!'
end
GO
EXEC proc_FirstProcedure



--- 
-- Created a Insert procedure for the table Products
create table Products
(id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null,
details nvarchar(max))
Go
CREATE PROCEDURE proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
AS
BEGIN
    insert into Products(name,details) values(@pname,@pdetails)
END
GO
proc_InsertProduct 'Laptop','{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}'
GO

-- 
select * from Products

SELECT JSON_QUERY(details, '$.spec') Produc_Sepcification from Products

--
CREATE PROCEDURE proc_UpdateProductSpec (@pid int, @newvalue varchar(20))
AS
BEGIN
    UPDATE Products SET details = JSON_MODIFY(details, '$.spec.ram', @newvalue) where id = @pid
END

EXEC proc_UpdateProductSpec 1, '32GB'
---

SELECT id, name, JSON_VALUE(details, '$.brand') 'Brand Name'
from Products



-- JSON_MODIFY function for Bulk Insert

 create table Posts
  (id int constraint pk_postId primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))
Go

  select * from Posts

  create proc proc_BulkInsertPosts(@jsondata nvarchar(max))
  as
  begin
	  insert into Posts(user_id,id,title,body)
	  select userId,id,title,body from openjson(@jsondata)
	  with (userId int,id int, title varchar(100), body varchar(max))
  end

  delete from Posts

  proc_BulkInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'


  -- Filtration using JSON_VALUE
  SELECT * from Products
  WHERE TRY_CAST(json_value(details, '$.spec.cpu')as nvarchar(20)) = 'i10'

  --create a procedure that brings post by taking the user_id as parameter

CREATE PROCEDURE proc_PostByUserId (@puser_id INT)
  AS
  BEGIN
	SELECT * from Posts where user_id = @puser_id;
  END
 
 DROP PROCEDURE IF EXISTS proc_PostByUserId
 

SELECT * from POSTS
EXEC proc_PostByUserId 1