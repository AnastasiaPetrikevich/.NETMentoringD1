CREATE TABLE [dbo].[CreditCards](
        [CreditCardId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        [ExpirationDate] DATETIME DEFAULT(NULL),
        [CardHolderName] VARCHAR(200) NOT NULL,
        [EmployeeId] INT REFERENCES [dbo].[Employees]([EmployeeID])
        );