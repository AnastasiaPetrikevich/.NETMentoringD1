---Task 2.2---
--1--
--По таблице Orders найти количество заказов с группировкой по годам.
--В результатах запроса надо возвращать две колонки c названиями Year и Total.
--Написать проверочный запрос, который вычисляет количество всех заказов
SELECT
	COUNT(OrderId) AS Total, 
	YEAR(OrderDate) AS Year
FROM Orders
GROUP BY YEAR(OrderDate);

SELECT
	COUNT(OrderId) AS Total
FROM Orders

--2--
--По таблице Orders найти количество заказов, cделанных каждым продавцом. Заказ для указанного продавца – это любая запись в таблице Orders,
--где в колонке EmployeeID задано значение для данного продавца. В результатах запроса надо возвращать колонку с именем продавца
--(Должно высвечиваться имя полученное конкатенацией LastName & FirstName. Эта строка LastName & FirstName должна быть получена отдельным запросом 
--в колонке основного запроса. Также основной запрос должен использовать группировку по EmployeeID.) с названием колонки ‘Seller’ 
--и колонку c количеством заказов возвращать с названием 'Amount'. Результаты запроса должны быть упорядочены по убыванию количества заказов
SELECT 
    (SELECT CONCAT(LastName,' ', FirstName) 
        FROM Employees
        WHERE Employees.EmployeeID = Orders.EmployeeID) 
	AS 'Seller',
    COUNT(OrderId) AS 'Amount'
FROM Orders
GROUP BY Orders.EmployeeID
ORDER BY 'Amount' DESC;

--3--
--По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя. 
--Необходимо определить это только для заказов, сделанных в 1998 году.
DECLARE 
	@year INT = 1998;

SELECT 
	EmployeeID,
	CustomerID,
	COUNT(OrderID) AS 'Amount'
FROM Orders
WHERE YEAR(OrderDate) = @year
GROUP BY EmployeeID, CustomerID;

--4--
--Найти покупателей и продавцов, которые живут в одном городе. Если в городе живут только один или несколько продавцов,
--или только один или несколько покупателей, то информация о таких покупателя и продавцах не должна попадать в результирующий набор.
--Не использовать конструкцию JOIN.
SELECT  City, 	
    Customers = STUFF(
                (SELECT ', ' + ContactName
				FROM Customers AS c
				WHERE c.City = e2.City	
				FOR XML PATH ('')), 1, 1, ''), 
	Employees = STUFF(
				(SELECT ', ' + FirstName + ' ' + LastName 
				FROM Employees AS e1 
				WHERE e1.City = e2.City
				FOR XML PATH ('')), 1, 1, '')
FROM Employees AS e2
GROUP BY City

--5--
--Найти всех покупателей, которые живут в одном городе.
SELECT  City, 	
    Customers = STUFF(
                (SELECT ', ' + ContactName 
				FROM Customers AS c1
				WHERE c1.City = c2.City
				FOR XML PATH ('')), 1, 1, '') 
FROM Customers AS c2
GROUP BY City

--6--
--По таблице Employees найти для каждого продавца его руководителя.
SELECT 
    Seller.FirstName AS 'Seller',
    (SELECT Manager.FirstName	
     FROM Employees AS Manager
     WHERE Manager.EmployeeID = Seller.ReportsTo) AS 'Manager'
FROM Employees AS Seller;