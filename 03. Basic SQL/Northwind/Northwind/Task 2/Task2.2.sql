---Task 2.2---
--1--
--�� ������� Orders ����� ���������� ������� � ������������ �� �����.
--� ����������� ������� ���� ���������� ��� ������� c ���������� Year � Total.
--�������� ����������� ������, ������� ��������� ���������� ���� �������
SELECT
	COUNT(OrderId) AS Total, 
	YEAR(OrderDate) AS Year
FROM Orders
GROUP BY YEAR(OrderDate);

SELECT
	COUNT(OrderId) AS Total
FROM Orders

--2--
--�� ������� Orders ����� ���������� �������, c�������� ������ ���������. ����� ��� ���������� �������� � ��� ����� ������ � ������� Orders,
--��� � ������� EmployeeID ������ �������� ��� ������� ��������. � ����������� ������� ���� ���������� ������� � ������ ��������
--(������ ������������� ��� ���������� ������������� LastName & FirstName. ��� ������ LastName & FirstName ������ ���� �������� ��������� �������� 
--� ������� ��������� �������. ����� �������� ������ ������ ������������ ����������� �� EmployeeID.) � ��������� ������� �Seller� 
--� ������� c ����������� ������� ���������� � ��������� 'Amount'. ���������� ������� ������ ���� ����������� �� �������� ���������� �������
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
--�� ������� Orders ����� ���������� �������, ��������� ������ ��������� � ��� ������� ����������. 
--���������� ���������� ��� ������ ��� �������, ��������� � 1998 ����.
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
--����� ����������� � ���������, ������� ����� � ����� ������. ���� � ������ ����� ������ ���� ��� ��������� ���������,
--��� ������ ���� ��� ��������� �����������, �� ���������� � ����� ���������� � ��������� �� ������ �������� � �������������� �����.
--�� ������������ ����������� JOIN.
SELECT C2.City, 
	Customers = STUFF(
		(SELECT ', ' + C1.ContactName
		FROM Customers AS C1 
		WHERE C2.City = C1.City 
		FOR XML PATH('')), 1, 1, ''), 
	Employees = STUFF(
		(SELECT ', ' + E1.FirstName + ' ' + E1.LastName 
		FROM Employees AS E1 
		WHERE E1.City = C2.City 
		FOR XML PATH('')), 1, 1, '') 
FROM 
	Customers AS C2, 
	Employees AS E2 
WHERE C2.City = E2.City 
GROUP BY C2.City;

--5--
--����� ���� �����������, ������� ����� � ����� ������.
SELECT  City, 	
    Customers = STUFF(
                (SELECT ', ' + ContactName 
				FROM Customers AS C1
				WHERE C1.City = C2.City
				FOR XML PATH ('')), 1, 1, '') 
FROM Customers AS C2
GROUP BY City;

--6--
--�� ������� Employees ����� ��� ������� �������� ��� ������������.
SELECT 
    Seller.FirstName AS 'Seller',
    (SELECT Manager.FirstName	
     FROM Employees AS Manager
     WHERE Manager.EmployeeID = Seller.ReportsTo) AS 'Manager'
FROM Employees AS Seller;