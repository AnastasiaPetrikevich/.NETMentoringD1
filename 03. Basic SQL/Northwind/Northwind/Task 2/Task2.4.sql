---Task 2.4---
--1--
--Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), у которых нет хотя бы одного
--продукта на складе (UnitsInStock в таблице Products равно 0). 
--Использовать вложенный SELECT для этого запроса с использованием оператора IN.
SELECT CompanyName
FROM Suppliers
WHERE Suppliers.SupplierID 
IN (SELECT Products.SupplierID
    FROM Products
    WHERE Products.UnitsInStock = 0);

--2--
--Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.
SELECT EmployeeID
FROM Employees
WHERE (SELECT COUNT(OrderID) 
       FROM Orders
       WHERE Orders.EmployeeID = Employees.EmployeeID) > 150;

--3--
--Выдать всех заказчиков (таблица Customers), которые не имеют ни одного заказа (подзапрос по таблице Orders). Использовать оператор EXISTS.
SELECT CustomerId
FROM Customers
WHERE NOT EXISTS 
	(SELECT OrderId
	 FROM Orders
	 WHERE Orders.CustomerID = Customers.CustomerID);
