INSERT INTO [Notification].[FinancialGuarantee]
(
	[Id],
	[Status],
	[ReceivedDate],
	[CompletedDate],
	[CreatedDate],
	[NotificationApplicationId]
)
SELECT	(SELECT CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		1,
		NULL,
		NULL,
		GETDATE(),
		N.Id
FROM	[Notification].[Notification] AS N
WHERE	N.Id NOT IN (SELECT NotificationApplicationId FROM [Notification].[FinancialGuarantee])
