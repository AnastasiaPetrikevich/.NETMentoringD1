---Task 1.3---
--1--
--¬ыбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повтор€тьс€), где встречаютс€ продукты 
--с количеством от 3 до 10 включительно Ц это колонка Quantity в таблице Order Details. »спользовать оператор BETWEEN.
--«апрос должен возвращать только колонку OrderID.
SELECT OrderId
FROM [Order Details]
WHERE Quantity BETWEEN 3 AND 10;

--2--
--¬ыбрать всех заказчиков из таблицы Customers, у которых название страны начинаетс€ на буквы из диапазона b и g. 
--»спользовать оператор BETWEEN. ѕроверить, что в результаты запроса попадает Germany. 
--«апрос должен возвращать только колонки CustomerID и Country и отсортирован по Country.
SELECT CustomerID, Country
FROM Customers
WHERE SUBSTRING(Country, 1, 1) BETWEEN 'b' AND 'g'
ORDER BY Country;

--3--
--¬ыбрать всех заказчиков из таблицы Customers, у которых название страны начинаетс€ на буквы из диапазона b и g,
--не использу€ оператор BETWEEN.
SELECT CustomerID, Country
FROM Customers
WHERE SUBSTRING(Country, 1, 1) >= 'b' 
	AND SUBSTRING(Country, 1, 1) <='g'
ORDER BY Country;

SELECT CustomerID, Country
FROM Customers
WHERE SUBSTRING(Country, 1, 1) 
	IN ('b', 'c', 'd', 'e', 'f', 'g')
ORDER BY Country;