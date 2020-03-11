---Task 2.1---
--1--
--����� ����� ����� ���� ������� �� ������� Order Details � ������ ���������� ����������� ������� � ������ �� ���. 
--����������� ������� ������ ���� ���� ������ � ����� �������� � ��������� ������� 'Totals'.
SELECT SUM(Quantity * UnitPrice * ( 1 - Discount )) AS Totals
FROM [Order Details];

--2--
--�� ������� Orders ����� ���������� �������, ������� ��� �� ���� ���������� (�.�. � ������� ShippedDate ��� �������� ���� ��������).
--������������ ��� ���� ������� ������ �������� COUNT. �� ������������ ����������� WHERE � GROUP.
SELECT COUNT(*)-COUNT(ShippedDate) AS Totals 
FROM Orders

--3--
--�� ������� Orders ����� ���������� ��������� ����������� (CustomerID), ��������� ������.
--������������ ������� COUNT � �� ������������ ����������� WHERE � GROUP.
SELECT DISTINCT COUNT(CustomerID) AS Totals 
FROM Orders