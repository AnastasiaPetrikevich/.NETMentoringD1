---Task 2.3---
--1--
--Определить продавцов, которые обслуживают регион 'Western' (таблица Region).
SELECT DISTINCT FirstName   
FROM Employees
	INNER JOIN EmployeeTerritories
		ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
	INNER JOIN Territories
        ON EmployeeTerritories.TerritoryID = Territories.TerritoryID
	INNER JOIN Region
        ON Region.RegionID = Territories.RegionID
WHERE Region.RegionDescription = 'Western';

--2--
--Выдать в результатах запроса имена всех заказчиков из таблицы Customers и суммарное количество их заказов из таблицы Orders. 
--Принять во внимание, что у некоторых заказчиков нет заказов, но они также должны быть выведены в результатах запроса. 
--Упорядочить результаты запроса по возрастанию количества заказов.
SELECT ContactName,
	COUNT(OrderId) AS 'OrdersCount'
FROM Customers 
    LEFT JOIN Orders
        ON Customers.CustomerId = Orders.CustomerId
GROUP BY Customers.CustomerID, Customers.ContactName
ORDER BY 'OrdersCount';
