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
--����� ���� �����������, ������� ����� � ����� ������.
SELECT  City, 	
    Customers = STUFF(
                (SELECT ', ' + ContactName 
				FROM Customers AS c1
				WHERE c1.City = c2.City
				FOR XML PATH ('')), 1, 1, '') 
FROM Customers AS c2
GROUP BY City

--6--
--�� ������� Employees ����� ��� ������� �������� ��� ������������.
SELECT 
    Seller.FirstName AS 'Seller',
    (SELECT Manager.FirstName	
     FROM Employees AS Manager
     WHERE Manager.EmployeeID = Seller.ReportsTo) AS 'Manager'
FROM Employees AS Seller;