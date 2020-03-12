IF EXISTS (
	SELECT * 
	FROM SYS.TABLES AS SystemTables 
	WHERE SystemTables.[Name] = 'Region')
BEGIN
    EXEC SP_RENAME 'Region', 'Regions';
END

IF NOT EXISTS (SELECT * 
				FROM SYS.COLUMNS SystemColumns 
                WHERE SystemColumns.[OBJECT_ID] = OBJECT_ID(N'[dbo].[Customers]') 
				AND Name = 'FoundationDate')
BEGIN
    ALTER TABLE [dbo].[Customers]
    ADD [FoundationDate] DATETIME
END