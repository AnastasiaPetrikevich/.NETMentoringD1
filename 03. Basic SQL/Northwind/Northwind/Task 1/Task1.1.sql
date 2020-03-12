---Task 1.1---
--1--
--������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� (������� ShippedDate)
--������������ � ������� ���������� � ShipVia >= 2 ������ ������ ���������� ������ ������� OrderID, ShippedDate � ShipVia.
DECLARE
@minDate DATETIME = Convert(DATETIME, '1998-05-06'),
@shipVia INT = 2;

SELECT OrderID, ShippedDate, ShipVia 
FROM Orders
WHERE ShippedDate >= @minDate
AND ShipVia >= @shipVia;

--2--
--�������� ������, ������� ������� ������ �������������� ������ �� ������� Orders. � ����������� ������� ���������� 
--��� ������� ShippedDate ������ �������� NULL ������ �Not Shipped� (������������ ��������� ������� CAS�). 
--������ ������ ���������� ������ ������� OrderID � ShippedDate.
SELECT OrderID, 
	CASE
		WHEN ShippedDate IS NULL
		THEN 'Not Shipped' 
		END  
	AS ShippedDate
FROM Orders
WHERE ShippedDate IS NULL;

--3--
--������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� (ShippedDate) �� ������� ��� ���� 
--��� ������� ��� �� ����������. � ������� ������ ������������ ������ ������� OrderID (������������� � Order Number
--� ShippedDate (������������� � Shipped Date). � ����������� ������� ���������� ��� ������� ShippedDate ������
--�������� NULL ������ �Not Shipped�, ��� ��������� �������� ���������� ���� � ������� �� ���������.
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
