---Task 2.3---
--1--
--���������� ���������, ������� ����������� ������ 'Western' (������� Region).
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
--������ � ����������� ������� ����� ���� ���������� �� ������� Customers � ��������� ���������� �� ������� �� ������� Orders. 
--������� �� ��������, ��� � ��������� ���������� ��� �������, �� ��� ����� ������ ���� �������� � ����������� �������. 
--����������� ���������� ������� �� ����������� ���������� �������.
SELECT ContactName,
	COUNT(OrderId) AS 'OrdersCount'
FROM Customers 
    LEFT JOIN Orders
        ON Customers.CustomerId = Orders.CustomerId
GROUP BY Customers.CustomerID, Customers.ContactName
ORDER BY 'OrdersCount';
