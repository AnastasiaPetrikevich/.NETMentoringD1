---Task 1.3---
--1--
--������� ��� ������ (OrderID) �� ������� Order Details (������ �� ������ �����������), ��� ����������� �������� 
--� ����������� �� 3 �� 10 ������������ � ��� ������� Quantity � ������� Order Details. ������������ �������� BETWEEN.
--������ ������ ���������� ������ ������� OrderID.
SELECT OrderId
FROM [Order Details]
WHERE Quantity BETWEEN 3 AND 10;

--2--
--������� ���� ���������� �� ������� Customers, � ������� �������� ������ ���������� �� ����� �� ��������� b � g. 
--������������ �������� BETWEEN. ���������, ��� � ���������� ������� �������� Germany. 
--������ ������ ���������� ������ ������� CustomerID � Country � ������������ �� Country.
SELECT CustomerID, Country
FROM Customers
WHERE SUBSTRING(Country, 1, 1) BETWEEN 'b' AND 'g'
ORDER BY Country;

--3--
--������� ���� ���������� �� ������� Customers, � ������� �������� ������ ���������� �� ����� �� ��������� b � g,
--�� ��������� �������� BETWEEN.
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