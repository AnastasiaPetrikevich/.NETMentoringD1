---Task 1.4---
--1--
--� ������� Products ����� ��� �������� (������� ProductName), ��� ����������� ��������� 'chocolade'. 
--��������, ��� � ��������� 'chocolade' ����� ���� �������� ���� ����� 'c' � �������� - ����� ��� ��������, ������� ������������� ����� �������.
SELECT ProductName
FROM Products
WHERE ProductName LIKE '%cho_olate%';