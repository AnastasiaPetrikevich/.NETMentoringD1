---Task 2.4---
--1--
--������ ���� ����������� (������� CompanyName � ������� Suppliers), � ������� ��� ���� �� ������
--�������� �� ������ (UnitsInStock � ������� Products ����� 0). 
--������������ ��������� SELECT ��� ����� ������� � �������������� ��������� IN.
SELECT CompanyName
FROM Suppliers
WHERE Suppliers.SupplierID 
IN (SELECT Products.SupplierID
    FROM Products
    WHERE Products.UnitsInStock = 0);

--2--
--������ ���� ���������, ������� ����� ����� 150 �������. ������������ ��������� SELECT.
SELECT EmployeeID
FROM Employees
WHERE (SELECT COUNT(OrderID) 
       FROM Orders
       WHERE Orders.EmployeeID = Employees.EmployeeID) > 150;

--3--
--������ ���� ���������� (������� Customers), ������� �� ����� �� ������ ������ (��������� �� ������� Orders). ������������ �������� EXISTS.
SELECT CustomerId
FROM Customers
WHERE NOT EXISTS 
	(SELECT OrderId
	 FROM Orders
	 WHERE Orders.CustomerID = Customers.CustomerID);
