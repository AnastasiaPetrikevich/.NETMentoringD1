---Task 1.1---
--1--
--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate)
--включительно и которые доставлены с ShipVia >= 2 Запрос должен возвращать только колонки OrderID, ShippedDate и ShipVia.
DECLARE
@minDate DATETIME = Convert(DATETIME, '1998-05-06'),
@shipVia INT = 2;

SELECT OrderID, ShippedDate, ShipVia 
FROM Orders
WHERE ShippedDate >= @minDate
AND ShipVia >= @shipVia;

--2--
--Написать запрос, который выводит только недоставленные заказы из таблицы Orders. В результатах запроса возвращать 
--для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’ (использовать системную функцию CASЕ). 
--Запрос должен возвращать только колонки OrderID и ShippedDate.
SELECT OrderID, 
	CASE
		WHEN ShippedDate IS NULL
		THEN 'Not Shipped' 
		END  
	AS ShippedDate
FROM Orders
WHERE ShippedDate IS NULL;

--3--
--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate) не включая эту дату 
--или которые еще не доставлены. В запросе должны возвращаться только колонки OrderID (переименовать в Order Number
--и ShippedDate (переименовать в Shipped Date). В результатах запроса возвращать для колонки ShippedDate вместо
--значений NULL строку ‘Not Shipped’, для остальных значений возвращать дату в формате по умолчанию.
DECLARE
@date DATETIME = Convert(DATETIME, '1998-05-06');

SELECT OrderID AS 'Order Number',
	CASE
		WHEN ShippedDate IS NULL
		THEN 'Not Shipped' 	
		ELSE CONVERT (VARCHAR(30), ShippedDate)
		END  
	AS 'Shipped Date'
FROM Orders
WHERE ShippedDate >= @date
OR ShippedDate IS NULL;
