IF OBJECT_ID('[Reports].[DataExportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataExportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataExportNotifications]
AS
	SELECT
		N.[NotificationNumber],
		N.[TypeId] AS [NotificationType],
		N.[CompetentAuthorityId],
		N.[Preconsented],
		NA.[StatusId] AS [Status],
		NA.[ReceivedDate] AS [NotificationReceived],
		NA.[PaymentReceivedDate] AS [PaymentReceived],
		NA.[CommencementDate] AS [AssessmentStarted],
		NA.[CompleteDate] AS [ApplicationCompleted],
		NA.[TransmittedDate] AS [Transmitted],
		NA.[AcknowlegedDate] AS [Acknowledged],
		-- Decision date will be the date it was withdrawn, objected or consented and it will only be one of these.
		CAST(COALESCE(NA.WithdrawnDate, COALESCE(NA.ObjectedDate, NA.ConsentedDate)) AS DATE) AS [DecisionDate]

	FROM		[Reports].[NotificationAssessment] AS NA

	INNER JOIN	[Reports].[Notification] AS N
	ON			[N].[Id] = [NA].[NotificationId]

	WHERE		[NA].[ImportOrExport] = 'Export'
GO