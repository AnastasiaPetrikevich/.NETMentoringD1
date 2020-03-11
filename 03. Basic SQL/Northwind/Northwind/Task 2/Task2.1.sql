---Task 2.1---
--1--
--Ќайти общую сумму всех заказов из таблицы Order Details с учетом количества закупленных товаров и скидок по ним. 
--–езультатом запроса должна быть одна запись с одной колонкой с названием колонки 'Totals'.
SELECT SUM(Quantity * UnitPrice * ( 1 - Discount )) AS Totals
FROM [Order Details];

--2--
--ѕо таблице Orders найти количество заказов, которые еще не были доставлены (т.е. в колонке ShippedDate нет значени€ даты доставки).
--»спользовать при этом запросе только оператор COUNT. Ќе использовать предложени€ WHERE и GROUP.
SELECT COUNT(*)-COUNT(ShippedDate) AS Totals 
FROM Orders

--3--
--ѕо таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы.
--»спользовать функцию COUNT и не использовать предложени€ WHERE и GROUP.
SELECT DISTINCT COUNT(CustomerID) AS Totals 
FROM Orders